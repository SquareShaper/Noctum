
using Terraria;
using Terraria.ModLoader;

namespace Noctum.Content.Projectiles.Minions
{
    public class NoctiaMinionBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true; // remove on exiting world
            Main.buffNoTimeDisplay[Type] = true; // don't show a time
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // check if player has minion, else remove buff
            if (player.ownedProjectileCounts[ModContent.ProjectileType<NoctiaMinion>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            } else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}