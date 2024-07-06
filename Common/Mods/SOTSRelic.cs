using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ColouredModsRelics.Common.Mods
{
    // 卧槽屎山
    public class SOTSRelic : ModRainbowRelics
    {
        public override string ModName => "SOTS";

        public SOTSRelic() : base() {
            HookInfo = new MethodBaseInfo(ModName, "SOTS.Items.Banners.SOTSRelics", "SpecialDraw");
        }

        public override ILContext.Manipulator Manip => il =>
        {
            ILCursor cursor = new(il);

            // 除了蛾子
            if (cursor.TryGotoNext(MoveType.After, i => i.MatchLdarg0(), i => i.MatchLdfld(out _), i => i.MatchCallvirt(out _)))
            {
                cursor.EmitDelegate((Texture2D tex) => Active ? ColoredRelicTileAssets[RelicTileTypes[0]].Value : tex);
            }
            // 蛾子
            if (cursor.TryGotoNext(MoveType.After, i => i.MatchLdarg0(), i => i.MatchLdfld(out _), i => i.MatchCallvirt(out _)))
            {
                cursor.EmitDelegate((Texture2D tex) => Active ? ColoredRelicTileAssets[RelicTileTypes[1]].Value : tex);
            }
        };

        public override string BaseRelicItemType => "SOTS.Items.Banners.ModRelic";

        public override void LoadTileTextures()
        {
            if (Mod.TryFind("SOTSRelics", out ModTile tile))
            {
                int type = tile.Type;
                RelicTileTypes.Add(type);
                ColoredRelicTileAssets[type] = RainbowUtils.Request(GetTilePath() + "Relic");
                // 暂存在type + 1
                RelicTileTypes.Add(type + 1);
                ColoredRelicTileAssets[type + 1] = RainbowUtils.Request(GetTilePath() + "GlowmothRelic2");
            }
            else
            {
                ColouredModsRelics.Instance.Logger.Warn($"{ModName}: needs update");
            }
        }
    }
}
