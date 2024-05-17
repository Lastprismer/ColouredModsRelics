using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ColouredModsRelics.Common.Mods
{
    public class SpiritModRelic : ModRainbowRelics
    {
        public override string ModName => "SpiritMod";

        public override MethodBase HookOrigin => Mod.Code.GetType("SpiritMod.Tiles.Relics.BaseRelic`1").MakeGenericType(Mod.Code.GetType("SpiritMod.Items.Placeable.Relics.AtlasRelicItem")).GetMethod("SpecialDraw", BindingFlags.Public | BindingFlags.Instance);

        public override ILContext.Manipulator Manip => il =>
        {
            ILCursor cursor = new(il);

            if (cursor.TryGotoNext(MoveType.After, i => i.MatchLdarg0(), i => i.MatchLdfld(out _), i => i.MatchCallvirt(out _)))
            {
                cursor.EmitLdarg0();
                cursor.EmitCall(typeof(ModTile).GetProperty("Type", BindingFlags.Instance | BindingFlags.Public).GetGetMethod());
                cursor.EmitDelegate<Func<Texture2D, int, Texture2D>>((tex, i) => Active ? ColoredRelicTileAssets[i].Value : tex);
            }
        };

        private static readonly string[] Relics = ["AtlasRelic", "AvianRelic", "DuskingRelic", "FrostSaucerRelic", "InfernonRelic", "MJWRelic", "OccultistRelic", "RlyehianRelic", "ScarabeusRelic", "StarplateRelic", "VinewrathRelic"];

        public override IEnumerable<ModItem> GetRelicItems()
        {
            List<ModItem> items = new();
            foreach (var relic in Relics)
                if (Mod.TryFind(relic + "Item", out ModItem relicItem))
                    items.Add(relicItem);
                else
                {
                    ColouredModsRelics.Instance.Logger.Warn($"{ModName}: needs update");
                    break;
                }
            return items;
        }

        public override IEnumerable<ModTile> GetRelicTiles()
        {
            List<ModTile> tiles = new();
            foreach (var relic in Relics)
                if (Mod.TryFind(relic, out ModTile relicTile))
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
