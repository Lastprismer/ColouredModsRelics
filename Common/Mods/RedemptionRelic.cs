using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ColouredModsRelics.Common.Mods
{
    public class RedemptionRelic : ModRainbowRelics
    {
        public override string ModName => "Redemption";

        public RedemptionRelic(): base() {
            HookInfo = new MethodBaseInfo(ModName, BaseRelicTileType, "SpecialDraw");
        }
        public override ILContext.Manipulator Manip => il =>
        {
            ILCursor cursor = new(il);

            if (cursor.TryGotoNext(MoveType.After, i => i.MatchLdarg0(), i => i.MatchLdfld(out _), i => i.MatchCallvirt(out _)))
            {
                cursor.EmitDelegate((Texture2D tex) => Active ? ColoredRelicTileAssets[RelicTileTypes[0]].Value : tex);
            }
        };

        public override string BaseRelicItemType => "Redemption.Items.Placeable.Trophies.BaseRelicItem";

        public override string BaseRelicTileType => "Redemption.Tiles.Trophies.RelicTile";

        public override IEnumerable<ModTile> GetRelicTiles()
        {
            if (Mod.TryFind("RelicTile", out ModTile tile))
                return [tile];
            else
            {
                ColouredModsRelics.Instance.Logger.Warn($"{ModName}: needs update");
                return [];
            }
        }
    }
}
