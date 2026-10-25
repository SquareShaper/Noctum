
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Noctum.Content.Projectiles.Minions
{
    public class NoctiaMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // set the frames of the sprite sheet
            Main.projFrames[Type] = 4;

            // Allow it to use right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;

            // Make it a minion/pet
            Main.projPet[Type] = true;

            // Make it replace minions when summoned - act like normal summon
            ProjectileID.Sets.MinionSacrificable[Type] = true;

            // Make it not disturb cultists
            ProjectileID.Sets.CultistIsResistantTo[Type] = true;
        }

        public override void SetDefaults()
        {
            // Sprite stuff
            Projectile.width = 40;
            Projectile.height = 40;

            // Make it move through walls
            Projectile.tileCollide = false;

			// Make it ignore water
			Projectile.ignoreWater = true;

            // Some minion toggles
            // Make it hurt enemies on contact
            Projectile.friendly = true;
            // Declare it a minion
            Projectile.minion = true;
            // Set damagetype
            Projectile.DamageType = DamageClass.Summon;
            // Costs 1 minion slot
            Projectile.minionSlots = 1f;
            // Give it infinite pierce, so it doesn't break when hitting enemies
            Projectile.penetrate = -1;
        }

        public override bool? CanCutTiles()
        {
            return false; // can it break pots and grass?
        }

        public override bool MinionContactDamage()
        {
            return true; // should it deal contact damage?
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (!CheckActive(owner))
            {
                return;
            }

            GeneralBehavior(owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
			SearchForTargets(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
			Movement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);
			Visuals();
        }

        private void GeneralBehavior(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition)
        {
            Vector2 idlePosition = owner.Center;
            idlePosition.Y -= 48f; // 3 tiles above the owner

            // If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
			// The index is projectile.minionPos
			float minionPositionOffsetX = (10 + Projectile.minionPos * 40) * -owner.direction;
			idlePosition.X += minionPositionOffsetX; // Go behind the player

            // Teleport to player if distance is too big
			vectorToIdlePosition = idlePosition - Projectile.Center;
			distanceToIdlePosition = vectorToIdlePosition.Length();

            if (Main.myPlayer == owner.whoAmI && distanceToIdlePosition > 2000f) {
				// Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
				// and then set netUpdate to true
				Projectile.position = idlePosition;
				Projectile.velocity *= 0.1f;
				Projectile.netUpdate = true;
			}

            // If your minion is flying, you want to do this independently of any conditions
			float overlapVelocity = 0.04f;

			// Fix overlap with other minions
			foreach (var other in Main.ActiveProjectiles) {
				if (other.whoAmI != Projectile.whoAmI && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width) {
					if (Projectile.position.X < other.position.X) {
						Projectile.velocity.X -= overlapVelocity;
					}
					else {
						Projectile.velocity.X += overlapVelocity;
					}

					if (Projectile.position.Y < other.position.Y) {
						Projectile.velocity.Y -= overlapVelocity;
					}
					else {
						Projectile.velocity.Y += overlapVelocity;
					}
				}
			}
        }

        private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter) {
			// Starting search distance
			distanceFromTarget = 700f;
			targetCenter = Projectile.position;
			foundTarget = false;

			// This code is required if your minion weapon has the targeting feature
			if (owner.HasMinionAttackTargetNPC) {
				NPC npc = Main.npc[owner.MinionAttackTargetNPC];
				float between = Vector2.Distance(npc.Center, Projectile.Center);

				// Reasonable distance away so it doesn't target across multiple screens
				if (between < 2000f) {
					distanceFromTarget = between;
					targetCenter = npc.Center;
					foundTarget = true;
				}
			}

			if (!foundTarget) {
				// This code is required either way, used for finding a target
				foreach (var npc in Main.ActiveNPCs) {
					if (npc.CanBeChasedBy()) {
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
						// Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
						// The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
						bool closeThroughWall = between < 100f;

						if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall)) {
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}

			// friendly needs to be set to true so the minion can deal contact damage
			// friendly needs to be set to false so it doesn't damage things like target dummies while idling
			// Both things depend on if it has a target or not, so it's just one assignment here
			// You don't need this assignment if your minion is shooting things instead of dealing contact damage
			Projectile.friendly = foundTarget;
		}

        private void Movement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition) {
			// Default movement parameters (here for attacking)
			float speed = 8f;
			float inertia = 20f;

			if (foundTarget) {
				// Minion has a target: attack (here, fly towards the enemy)
				if (distanceFromTarget > 40f) {
					// The immediate range around the target (so it doesn't latch onto it when close)
					Vector2 direction = targetCenter - Projectile.Center;
					direction.Normalize();
					direction *= speed;

					Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
				}
			}
			else {
				// Minion doesn't have a target: return to player and idle
				if (distanceToIdlePosition > 600f) {
					// Speed up the minion if it's away from the player
					speed = 12f;
					inertia = 60f;
				}
				else {
					// Slow down the minion if closer to the player
					speed = 4f;
					inertia = 80f;
				}

				if (distanceToIdlePosition > 20f) {
					// The immediate range around the player (when it passively floats about)

					// This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
					vectorToIdlePosition.Normalize();
					vectorToIdlePosition *= speed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
				}
				else if (Projectile.velocity == Vector2.Zero) {
					// If there is a case where it's not moving at all, give it a little "poke"
					Projectile.velocity.X = -0.15f;
					Projectile.velocity.Y = -0.05f;
				}
			}
		}

        private void Visuals() {
			// So it will lean slightly towards the direction it's moving - also, speeeeen
			Projectile.rotation = Projectile.rotation + Projectile.velocity.X * 0.05f;

			// This is a simple "loop through all frames from top to bottom" animation
			int frameSpeed = 5;

			Projectile.frameCounter++;

			if (Projectile.frameCounter >= frameSpeed) {
				Projectile.frameCounter = 0;
				Projectile.frame++;

				if (Projectile.frame >= Main.projFrames[Type]) {
					Projectile.frame = 0;
				}
			}

			// Some visuals here
			Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.75f);
		}


        // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
		private bool CheckActive(Player owner) {
			if (owner.dead || !owner.active) {
				owner.ClearBuff(ModContent.BuffType<NoctiaMinionBuff>());

				return false;
			}

			if (owner.HasBuff(ModContent.BuffType<NoctiaMinionBuff>())) {
				Projectile.timeLeft = 2;
			}

			return true;
		}
    }
}