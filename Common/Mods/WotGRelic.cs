using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace ColouredModsRelics.Common.Mods
{
    public class WotGRelic : ModRainbowRelics
    {
        public override string ModName => "NoxusBoss";

        public readonly string[] relics = ["NamelessDeityRelic", "NoxusRelic"];
        public override MethodBase HookOrigin => Mod.Code.GetType("NoxusBoss.Core.Autoloaders.RelicAutoloader+AutoloadableRelicTile").GetMethod("SpecialDraw", BindingFlags.Public | BindingFlags.Instance);

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

        public override IEnumerable<ModItem> GetRelicItems()
        {
            List<ModItem> items = new();
            foreach (string relic in relics)
            {
                if (Mod.TryFind(relic + "Item", out ModItem relicItem))
                    items.Add(relicItem);
                else
                {
                    ColouredModsRelics.Instance.Logger.Warn($"{ModName}: needs update");
                }
            }
            return items;
        }
        public override IEnumerable<ModTile> GetRelicTiles()
        {
            List<ModTile> tiles = new();
            foreach (string relic in relics)
            {
                if (Mod.TryFind(relic + "Tile", out ModTile relicTile))
                    tiles.Add(relicTile);
                else
                {
                    ColouredModsRelics.Instance.Logger.Warn($"{ModName}: needs update");
                }
            }
            return tiles;
        }
    }
}
