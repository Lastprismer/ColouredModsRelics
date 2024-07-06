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
    public class StarlightRiverRelic : ModRainbowRelics
    {
        public override string ModName => "StarlightRiver";

        public StarlightRiverRelic() : base() {
            HookInfo = new(ModName, "StarlightRiver.Content.Tiles.Relics.BaseRelic", "SpecialDraw");
        }

        public override ILContext.Manipulator Manip => il =>
        {
            ILCursor cursor = new(il);

            if (cursor.TryGotoNext(MoveType.After, i => i.MatchLdarg0(), i => i.MatchLdfld(out _), i => i.MatchCallvirt(out _)))
            {
                cursor.EmitLdarg0();
                cursor.EmitCall(ModTile_Type);
                cursor.EmitDelegate<Func<Texture2D, int, Texture2D>>((tex, i) => Active ? ColoredRelicTileAssets[i].Value : tex);
            }
        };

        private static readonly string[] relics = { "AuroracleRelic", "CeirosRelic" };

        public override IEnumerable<ModItem> GetRelicItems()
        {
            List<ModItem> items = [];
            foreach (var r in relics)
            {
                if (Mod.TryFind(r + "Item", out ModItem relicItem))
                    items.Add(relicItem);
                else
                {
                    ColouredModsRelics.Instance.Logger.Warn($"{ModName}: needs update");
                    break;
                }
            }
            return items;
        }

        public override IEnumerable<ModTile> GetRelicTiles()
        {
            List<ModTile> tiles = [];
            foreach (var r in relics)
                if (Mod.TryFind(r, out ModTile relicTile))
                    tiles.Add(relicTile);
                else
                {
                    ColouredModsRelics.Instance.Logger.Warn($"{ModName}: needs update");
                    break;
                }
            return tiles;
        }
    }
}
