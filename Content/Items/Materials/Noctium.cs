using Terraria.ModLoader;

namespace Noctum.Content.Items.Materials
{
    public class Noctium : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;

            Item.maxStack = Terraria.Item.CommonMaxStack;
            Item.value = Terraria.Item.buyPrice(silver: 1);
        }
    }
}