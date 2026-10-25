using Microsoft.Xna.Framework;
using Noctum.Content.Items.Materials;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Noctum.Content.Projectiles.Minions
{
    public class NoctiaMinionItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Type] = true; // summon anywhere on controller
            ItemID.Sets.LockOnIgnoresCollision[Type] = true; // more compat stuff?

            ItemID.Sets.StaffMinionSlotsRequired[Type] = 1; // only one minion slot required for the summon
        }

        public override void SetDefaults()
        {
            // weapon stats
            Item.damage = 3;
            Item.knockBack = 3f;
            Item.mana = 5;

            // sprite stuff
            Item.width = 40;
            Item.height = 40;

            // animations
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;

            // value
            Item.value = Terraria.Item.sellPrice(silver: 3);
            Item.rare = ItemRarityID.Orange;

            // sound
            Item.UseSound = SoundID.Item44;

            // Summon weapon specifics
            Item.noMelee = true; // can't whack enemies with this
            Item.DamageType = DamageClass.Summon; // make summon mult scale this
            Item.buffType = ModContent.BuffType<NoctiaMinionBuff>(); // actually give the minion buff
            Item.shoot = ModContent.ProjectileType<NoctiaMinion>(); // actually summon the right minion
            Item.autoReuse = true; // keep swinging when holding down
            Item.reuseDelay = 2; // copied from DesertTigerStaff (4607)
            
            Item.shootSpeed = 10f; // copied from DesertTigerStaff (4607)
            Item.knockBack = 2.5f; // copied from DesertTigerStaff (4607)
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Change where the minion is spawned here
            position = Main.MouseWorld;
            player.LimitPointToPlayerReachableArea(ref position);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            return true; // returning true makes the game summon our projectile - if we set it to fale, we could summon it ourselves (or do checks)
        }

        public override void AddRecipes()
        {
            // mek recipe
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Noctium>())
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}