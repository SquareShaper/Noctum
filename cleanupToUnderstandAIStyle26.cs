using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Noctum.Content.Projectiles.Minions
{
    public class NoctiaMinion2 : ModProjectile
    {


		private void AI_026()
			{
				bool flag2 = false;
				bool flag3 = false;
				bool ownerIsBelowUs = false;
				bool flag5 = false;
				bool beingPetByOwner = Main.player[Projectile.owner].petting.isPetting && Main.player[Projectile.owner].petting.proj == Projectile.whoAmI;
				



				bool aiZeroIsMinusOneOrTwo = Projectile.ai[0] == -1f || Projectile.ai[0] == -2f;
				bool aiZeroIsMinusOne = Projectile.ai[0] == -1f;
				bool aiZeroIsMinusTwo = Projectile.ai[0] == -2f;
				


				if (aiZeroIsMinusOneOrTwo)
					Projectile.timeLeft = 2;

				int offsetFromOwnerForIdling = 40 * (Projectile.minionPos + 1) * Main.player[Projectile.owner].direction;
				if (Main.player[Projectile.owner].Center.X < Projectile.Center.X - 10f + (float)offsetFromOwnerForIdling)
					flag2 = true;
				else if (Main.player[Projectile.owner].Center.X > Projectile.Center.X + 10f + (float)offsetFromOwnerForIdling)
					flag3 = true;
			

				if (aiZeroIsMinusOne) {
					flag2 = false;
					flag3 = true;
				}

				if (aiZeroIsMinusTwo) {
					flag2 = false;
					flag3 = false;
				}

				
				
				
				
				
				
				bool aiOneIsZero = Projectile.ai[1] == 0f;

				if (aiOneIsZero) {
					int num92 = 500;


					num92 += 40 * Projectile.minionPos;
					if (Projectile.localAI[0] > 0f)
						num92 += 500;

					if (Projectile.localAI[0] > 0f)
						num92 += 400;
					

					bool flag18 = false;

					// find distance to Projectile.owner
					Vector2 ourCenterPos = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
					float xDistToOwner = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - ourCenterPos.X;
					float yDistToOwner = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - ourCenterPos.Y;
					float distToOwner = (float)Math.Sqrt(xDistToOwner * xDistToOwner + yDistToOwner * yDistToOwner);
					
					if (!aiZeroIsMinusOneOrTwo) {
						if (distToOwner > 2000f) {
							Projectile.position.X = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - (float)(Projectile.width / 2);
							Projectile.position.Y = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - (float)(Projectile.height / 2);
						}
						else if (distToOwner > (float)num92 || (Math.Abs(yDistToOwner) > 300f && !(Projectile.localAI[0] > 0f))) {
							if (yDistToOwner > 0f && Projectile.velocity.Y < 0f)
								Projectile.velocity.Y = 0f;

							if (yDistToOwner < 0f && Projectile.velocity.Y > 0f)
								Projectile.velocity.Y = 0f;

							flag18 = true;
						}
					}

					if (flag18) {
						Projectile.ai[0] = 1f;
					}
				}

				if (Projectile.ai[0] != 0f && !aiZeroIsMinusOneOrTwo) {
					float num101 = 0.2f;
					int num102 = 200;


					Projectile.tileCollide = false;
					Vector2 vector11 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
					float num103 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector11.X;

					num103 -= (float)(40 * Main.player[Projectile.owner].direction);
					float num104 = 700f;

					bool flag20 = false;
					int num105 = -1;
					for (int num106 = 0; num106 < Main.maxNPCs; num106++) {
						if (!Main.npc[num106].CanBeChasedBy(this))
							continue;

						float num107 = Main.npc[num106].position.X + (float)(Main.npc[num106].width / 2);
						float num108 = Main.npc[num106].position.Y + (float)(Main.npc[num106].height / 2);
						if (Math.Abs(Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - num107) + Math.Abs(Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - num108) < num104) {
							if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[num106].position, Main.npc[num106].width, Main.npc[num106].height))
								num105 = num106;

							flag20 = true;
							break;
						}
					}

					if (!flag20)
						num103 -= (float)(40 * Projectile.minionPos * Main.player[Projectile.owner].direction);

					if (flag20 && num105 >= 0)
						Projectile.ai[0] = 0f;
				

					float num109 = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - vector11.Y;

					float num110 = (float)Math.Sqrt(num103 * num103 + num109 * num109);
					float num111 = num110;
					float num112 = 10f;
					float num113 = num110;
					

					if (num110 < (float)num102 && Main.player[Projectile.owner].velocity.Y == 0f && Projectile.position.Y + (float)Projectile.height <= Main.player[Projectile.owner].position.Y + (float)Main.player[Projectile.owner].height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height)) {
						Projectile.ai[0] = 0f;
						if (Projectile.velocity.Y < -6f)
							Projectile.velocity.Y = -6f;
					}

					if (num110 < 60f) {
						num103 = Projectile.velocity.X;
						num109 = Projectile.velocity.Y;
					}
					else {
						num110 = num112 / num110;
						num103 *= num110;
						num109 *= num110;
					}

					
					if (Projectile.velocity.X < num103) {
						Projectile.velocity.X += num101;
						if (Projectile.velocity.X < 0f)
							Projectile.velocity.X += num101 * 1.5f;
					}

					if (Projectile.velocity.X > num103) {
						Projectile.velocity.X -= num101;
						if (Projectile.velocity.X > 0f)
							Projectile.velocity.X -= num101 * 1.5f;
					}

					if (Projectile.velocity.Y < num109) {
						Projectile.velocity.Y += num101;
						if (Projectile.velocity.Y < 0f)
							Projectile.velocity.Y += num101 * 1.5f;
					}

					if (Projectile.velocity.Y > num109) {
						Projectile.velocity.Y -= num101;
						if (Projectile.velocity.Y > 0f)
							Projectile.velocity.Y -= num101 * 1.5f;
					}

					if ((double)Projectile.velocity.X > 0.5)
						Projectile.spriteDirection = -1;
					else if ((double)Projectile.velocity.X < -0.5)
						Projectile.spriteDirection = 1;
					

					
					int num117 = (int)(Projectile.Center.X / 16f);
					int num118 = (int)(Projectile.Center.Y / 16f);
					if (Main.tile[num117, num118] != null && Main.tile[num117, num118].wall > 0) {
						Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
						Projectile.frameCounter += (int)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y));
						if (Projectile.frameCounter > 5) {
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}

						if (Projectile.frame > 7)
							Projectile.frame = 4;

						if (Projectile.frame < 4)
							Projectile.frame = 7;
					}
					else {
						Projectile.frameCounter++;
						if (Projectile.frameCounter > 2) {
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}

						if (Projectile.frame < 8 || Projectile.frame > 10)
							Projectile.frame = 8;

						Projectile.rotation = Projectile.velocity.X * 0.1f;
					}
				}
				else {
					bool flag23 = false;
					Vector2 vector15 = Vector2.Zero;
					bool flag24 = false;

					float num163 = 40 * Projectile.minionPos;
					int num164 = 60;
					Projectile.localAI[0] -= 1f;
					if (Projectile.localAI[0] < 0f)
						Projectile.localAI[0] = 0f;

					if (Projectile.ai[1] > 0f) {
						Projectile.ai[1] -= 1f;
					}
					else {
						float myX = Projectile.position.X;
						float myY = Projectile.position.Y;
						float num167 = 100000f;
						float num168 = num167;
						int indexOfOurTargetInMainNPCArray = -1;
						NPC npcTargetedByOwner = Projectile.OwnerMinionAttackTargetNPC;
						if (npcTargetedByOwner != null && npcTargetedByOwner.CanBeChasedBy(this)) {
							float x = npcTargetedByOwner.Center.X;
							float y = npcTargetedByOwner.Center.Y;
							float num170 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - x) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - y);
							if (num170 < num167) {
								if (indexOfOurTargetInMainNPCArray == -1 && num170 <= num168) {
									num168 = num170;
									myX = x;
									myY = y;
								}

								if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, npcTargetedByOwner.position, npcTargetedByOwner.width, npcTargetedByOwner.height)) {
									num167 = num170;
									myX = x;
									myY = y;
									indexOfOurTargetInMainNPCArray = npcTargetedByOwner.whoAmI;
								}
							}
						}

						if (indexOfOurTargetInMainNPCArray == -1) {
							for (int num171 = 0; num171 < Main.maxNPCs; num171++) {
								if (!Main.npc[num171].CanBeChasedBy(this))
									continue;

								float num172 = Main.npc[num171].position.X + (float)(Main.npc[num171].width / 2);
								float num173 = Main.npc[num171].position.Y + (float)(Main.npc[num171].height / 2);
								float num174 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num172) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num173);
								if (num174 < num167) {
									if (indexOfOurTargetInMainNPCArray == -1 && num174 <= num168) {
										num168 = num174;
										myX = num172;
										myY = num173;
									}

									if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[num171].position, Main.npc[num171].width, Main.npc[num171].height)) {
										num167 = num174;
										myX = num172;
										myY = num173;
										indexOfOurTargetInMainNPCArray = num171;
									}
								}
							}
						}

						if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
							Projectile.tileCollide = true;

						if (indexOfOurTargetInMainNPCArray == -1 && num168 < num167) {
							num167 = num168;
						}
						else if (indexOfOurTargetInMainNPCArray >= 0) {
							flag23 = true;
							vector15 = new Vector2(myX, myY) - Projectile.Center;

							if (Main.npc[indexOfOurTargetInMainNPCArray].position.Y > Projectile.position.Y + (float)Projectile.height) {
								int num175 = (int)(Projectile.Center.X / 16f);
								int num176 = (int)((Projectile.position.Y + (float)Projectile.height + 1f) / 16f);
								if (Main.tile[num175, num176] != null && Main.tile[num175, num176].active() && TileID.Sets.Platforms[Main.tile[num175, num176].type])
									Projectile.tileCollide = false;
							}

							Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
							Rectangle value3 = new Rectangle((int)Main.npc[indexOfOurTargetInMainNPCArray].position.X, (int)Main.npc[indexOfOurTargetInMainNPCArray].position.Y, Main.npc[indexOfOurTargetInMainNPCArray].width, Main.npc[indexOfOurTargetInMainNPCArray].height);
							int num177 = 10;
							value3.X -= num177;
							value3.Y -= num177;
							value3.Width += num177 * 2;
							value3.Height += num177 * 2;
							if (rectangle.Intersects(value3)) {
								flag24 = true;
								Vector2 vector16 = Main.npc[indexOfOurTargetInMainNPCArray].Center - Projectile.Center;
								if (Projectile.velocity.Y > 0f && vector16.Y < 0f)
									Projectile.velocity.Y *= 0.5f;

								if (Projectile.velocity.Y < 0f && vector16.Y > 0f)
									Projectile.velocity.Y *= 0.5f;

								if (Projectile.velocity.X > 0f && vector16.X < 0f)
									Projectile.velocity.X *= 0.5f;

								if (Projectile.velocity.X < 0f && vector16.X > 0f)
									Projectile.velocity.X *= 0.5f;

								if (vector16.Length() > 14f) {
									vector16.Normalize();
									vector16 *= 14f;
								}

								Projectile.rotation = (Projectile.rotation * 5f + vector16.ToRotation() + (float)Math.PI / 2f) / 6f;
								Projectile.velocity = (Projectile.velocity * 9f + vector16) / 10f;
								for (int num178 = 0; num178 < 1000; num178++) {
									if (Projectile.whoAmI != num178 && Projectile.owner == Main.projectile[num178].owner && Main.projectile[num178].type >= ProjectileID.VenomSpider && Main.projectile[num178].type <= ProjectileID.DangerousSpider && (Main.projectile[num178].Center - Projectile.Center).Length() < 15f) {
										float num179 = 0.5f;
										if (Projectile.Center.Y > Main.projectile[num178].Center.Y) {
											Main.projectile[num178].velocity.Y -= num179;
											Projectile.velocity.Y += num179;
										}
										else {
											Main.projectile[num178].velocity.Y += num179;
											Projectile.velocity.Y -= num179;
										}

										if (Projectile.Center.X > Main.projectile[num178].Center.X) {
											Projectile.velocity.X += num179;
											Main.projectile[num178].velocity.X -= num179;
										}
										else {
											Projectile.velocity.X -= num179;
											Main.projectile[num178].velocity.Y += num179;
										}
									}
								}
								
							}
						}

						float num180 = 300f;
						if ((double)Projectile.position.Y > Main.worldSurface * 16.0)
							num180 = 150f;

						num180 = 500f;
						if ((double)Projectile.position.Y > Main.worldSurface * 16.0)
							num180 = 250f;
						

						if (num167 < num180 + num163 && indexOfOurTargetInMainNPCArray == -1) {
							float num181 = myX - (Projectile.position.X + (float)(Projectile.width / 2));
							if (num181 < -5f) {
								flag2 = true;
								flag3 = false;
							}
							else if (num181 > 5f) {
								flag3 = true;
								flag2 = false;
							}
						}

						bool flag25 = false;
						if (Projectile.localAI[1] > 0f) {
							flag25 = true;
							Projectile.localAI[1] -= 1f;
						}

						if (indexOfOurTargetInMainNPCArray >= 0 && num167 < 800f + num163) {
							Projectile.friendly = true;
							Projectile.localAI[0] = num164;
							float num182 = myX - (Projectile.position.X + (float)(Projectile.width / 2));
							if (num182 < -10f) {
								flag2 = true;
								flag3 = false;
							}
							else if (num182 > 10f) {
								flag3 = true;
								flag2 = false;
							}

							if (myY < Projectile.Center.Y - 100f && num182 > -50f && num182 < 50f && Projectile.velocity.Y == 0f) {
								float num183 = Math.Abs(myY - Projectile.Center.Y);
								if (num183 < 120f)
									Projectile.velocity.Y = -10f;
								else if (num183 < 210f)
									Projectile.velocity.Y = -13f;
								else if (num183 < 270f)
									Projectile.velocity.Y = -15f;
								else if (num183 < 310f)
									Projectile.velocity.Y = -17f;
								else if (num183 < 380f)
									Projectile.velocity.Y = -18f;
							}

							if (flag25) {
								Projectile.friendly = false;
								if (Projectile.velocity.X < 0f)
									flag2 = true;
								else if (Projectile.velocity.X > 0f)
									flag3 = true;
							}
						}
						else {
							Projectile.friendly = false;
						}
						
					}

					if (Projectile.ai[1] != 0f || beingPetByOwner) {
						flag2 = false;
						flag3 = false;
					}

					bool flag26 = true;

					if (!flag24) {
						Projectile.rotation = 0f;
					}

					float num184 = 0.08f;
					float num185 = 6.5f;

					num185 = 6f;
					num184 = 0.2f;
					if (num185 < Math.Abs(Main.player[Projectile.owner].velocity.X) + Math.Abs(Main.player[Projectile.owner].velocity.Y)) {
						num185 = Math.Abs(Main.player[Projectile.owner].velocity.X) + Math.Abs(Main.player[Projectile.owner].velocity.Y);
						num184 = 0.3f;
					}
					

					num184 *= 2f;

					if (aiZeroIsMinusOneOrTwo)
						num185 = 6f;

					if (flag2) {
						if ((double)Projectile.velocity.X > -3.5)
							Projectile.velocity.X -= num184;
						else
							Projectile.velocity.X -= num184 * 0.25f;
					}
					else if (flag3) {
						if ((double)Projectile.velocity.X < 3.5)
							Projectile.velocity.X += num184;
						else
							Projectile.velocity.X += num184 * 0.25f;
					}
					else {
						Projectile.velocity.X *= 0.9f;
						if (Projectile.velocity.X >= 0f - num184 && Projectile.velocity.X <= num184)
							Projectile.velocity.X = 0f;
					}

					if (flag2 || flag3) {
						int num186 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
						int num187 = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16;

						if (flag2)
							num186--;

						if (flag3)
							num186++;

						num186 += (int)Projectile.velocity.X;
						if (WorldGen.InWorld(num186, num187) && WorldGen.SolidTile(num186, num187) && Main.tile[num186, num187] != null && !TileID.Sets.ForAdvancedCollision.ClosedDoors[Main.tile[num186, num187].type])
							flag5 = true;
					}

					if (Main.player[Projectile.owner].position.Y + (float)Main.player[Projectile.owner].height - 8f > Projectile.position.Y + (float)Projectile.height)
						ownerIsBelowUs = true;

					Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
					if (Projectile.velocity.Y == 0f) {
						if (!ownerIsBelowUs && (Projectile.velocity.X < 0f || Projectile.velocity.X > 0f)) {
							int num188 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
							int j2 = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16 + 1;
							if (flag2)
								num188--;

							if (flag3)
								num188++;

							WorldGen.SolidTile(num188, j2);
						}

						if (flag5) {
							int num189 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
							int num190 = (int)(Projectile.position.Y + (float)Projectile.height) / 16;
							if (WorldGen.SolidTileAllowBottomSlope(num189, num190) || Main.tile[num189, num190].halfBrick() || Main.tile[num189, num190].slope() > 0) {
								try {
									num189 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
									num190 = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16;
									if (flag2)
										num189--;

									if (flag3)
										num189++;

									num189 += (int)Projectile.velocity.X;
									if (!WorldGen.SolidTile(num189, num190 - 1) && !WorldGen.SolidTile(num189, num190 - 2))
										Projectile.velocity.Y = -5.1f;
									else if (!WorldGen.SolidTile(num189, num190 - 2))
										Projectile.velocity.Y = -7.1f;
									else if (WorldGen.SolidTile(num189, num190 - 5))
										Projectile.velocity.Y = -11.1f;
									else if (WorldGen.SolidTile(num189, num190 - 4))
										Projectile.velocity.Y = -10.1f;
									else
										Projectile.velocity.Y = -9.1f;
								}
								catch {
									Projectile.velocity.Y = -9.1f;
								}

							}
						}
					}

					if (Projectile.velocity.X > num185)
						Projectile.velocity.X = num185;

					if (Projectile.velocity.X < 0f - num185)
						Projectile.velocity.X = 0f - num185;

					if (flag26) {
						if (Projectile.velocity.X < 0f)
							Projectile.direction = -1;

						if (Projectile.velocity.X > 0f)
							Projectile.direction = 1;

						if (Projectile.velocity.X > num184 && flag3)
							Projectile.direction = 1;

						if (Projectile.velocity.X < 0f - num184 && flag2)
							Projectile.direction = -1;
					}


					if (Projectile.direction == -1)
						Projectile.spriteDirection = 1;

					if (Projectile.direction == 1)
						Projectile.spriteDirection = -1;
					

					bool weDidntMoveSinceLastFrame = Projectile.position.X - Projectile.oldPosition.X == 0f;
					
					
					int i4 = (int)(Projectile.Center.X / 16f);
					int num231 = (int)(Projectile.Center.Y / 16f);
					int num232 = 0;
					Tile tileSafely5 = Framing.GetTileSafely(i4, num231);
					Tile tileSafely6 = Framing.GetTileSafely(i4, num231 - 1);
					Tile tileSafely7 = Framing.GetTileSafely(i4, num231 + 1);
					if (tileSafely5.wall > 0)
						num232++;

					if (tileSafely6.wall > 0)
						num232++;

					if (tileSafely7.wall > 0)
						num232++;

					if (num232 > 1) {
						float num233 = 9f;
						float num234 = 40 * (Projectile.minionPos + 1);
						Vector2 v11 = Main.player[Projectile.owner].Center - Projectile.Center;
						if (flag23) {
							v11 = vector15;
							num234 = 10f;
						}

						if (v11.Length() < num234) {
							Projectile.velocity *= 0.9f;
							if ((double)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) < 0.1)
								Projectile.velocity *= 0f;
						}
						else if (v11.Length() < 800f || !flag23) {
							Projectile.velocity = (Projectile.velocity * 9f + v11.SafeNormalize(Vector2.Zero) * num233) / 10f;
						}

						if (v11.Length() >= num234) {
							Projectile.spriteDirection = Projectile.direction;
							Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
						}
						else {
							Projectile.rotation = v11.ToRotation() + (float)Math.PI / 2f;
						}

						Projectile.frameCounter += (int)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y));
						if (Projectile.frameCounter > 8) {
							Projectile.frame++;
							Projectile.frameCounter = 0;
						}

						if (Projectile.frame > 7)
							Projectile.frame = 4;

						if (Projectile.frame < 4)
							Projectile.frame = 7;
					}
					else {
						if (!flag24)
							Projectile.rotation = 0f;

						Projectile.spriteDirection = -Projectile.direction;
						if (!flag24 && Projectile.frame >= 4 && Projectile.frame <= 7) {
							Vector2 vector18 = Main.player[Projectile.owner].Center - Projectile.Center;
							if (flag23)
								vector18 = vector15;

							float num235 = 0f - vector18.Y;
							if (!(vector18.Y > 0f)) {
								if (num235 < 120f)
									Projectile.velocity.Y = -10f;
								else if (num235 < 210f)
									Projectile.velocity.Y = -13f;
								else if (num235 < 270f)
									Projectile.velocity.Y = -15f;
								else if (num235 < 310f)
									Projectile.velocity.Y = -17f;
								else if (num235 < 380f)
									Projectile.velocity.Y = -18f;
							}
						}

						if (flag24) {
							Projectile.frameCounter++;
							if (Projectile.frameCounter > 4) {
								Projectile.frame++;
								Projectile.frameCounter = 0;
							}

							if (Projectile.frame >= 8)
								Projectile.frame = 4;

							if (Projectile.frame <= 3)
								Projectile.frame = 7;
						}
						else if (Projectile.velocity.Y >= 0f && Projectile.velocity.Y <= 0.8f) {
							if (weDidntMoveSinceLastFrame) {
								Projectile.frame = 0;
								Projectile.frameCounter = 0;
							}
							else if (Projectile.velocity.X < -0.8f || Projectile.velocity.X > 0.8f) {
								Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
								Projectile.frameCounter++;
								if (Projectile.frameCounter > 8) {
									Projectile.frame++;
									Projectile.frameCounter = 0;
								}

								if (Projectile.frame > 2)
									Projectile.frame = 0;
							}
							else {
								Projectile.frame = 0;
								Projectile.frameCounter = 0;
							}
						}
						else {
							Projectile.frameCounter = 0;
							Projectile.frame = 3;
						}

						Projectile.velocity.Y += 0.4f;
						if (Projectile.velocity.Y > 10f)
							Projectile.velocity.Y = 10f;
					}
					
				}
		}
	}
}