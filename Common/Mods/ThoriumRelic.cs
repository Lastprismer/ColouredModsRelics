using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace ColouredModsRelics.Common.Mods
{
    public class ThoriumRelic : ModRainbowRelics
    {
        public override string ModName => "ThoriumMod";

        public ThoriumRelic() : base() {
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

        public override string BaseRelicItemType => "ThoriumMod.Items.Placeable.Relics.RelicItemBase";

        public override string BaseRelicTileType => "ThoriumMod.Tiles.RelicTile";

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
