using ColouredModsRelics.Common.Config;
using ColouredModsRelics.Core;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace ColouredModsRelics.Common.Mods
{
    public class CatalystRelic : ModRainbowRelics
    {
        public override string ModName => "CatalystMod";

        public CatalystRelic() : base()
        {
            HookInfo = new MethodBaseInfo(ModName, "CatalystMod.Tiles.Furniture.BossRelics", "SpecialDraw", BindingFlags.Public | BindingFlags.Instance);
        }

        public override ILContext.Manipulator Manip => il =>
        {
            ILCursor cursor = new(il);

            if (cursor.TryGotoNext(MoveType.After, i => i.MatchLdcI4(2), i => i.MatchCall(out _), i => i.MatchCallvirt(out _)))
            {
                cursor.EmitDelegate((Texture2D tex) => Active ? ColoredRelicTileAssets[RelicTileTypes[0]].Value : tex);
            }
        };

        public override IEnumerable<ModItem> GetRelicItems()
        {
            if (Mod.TryFind("AstrageldonRelic", out ModItem relicItem))
                return [relicItem];
            else
            {
                ColouredModsRelics.Instance.Logger.Warn($"{ModName}: needs update");
                return [];
            }
        }

        public override IEnumerable<ModTile> GetRelicTiles()
        {
            if (Mod.TryFind("BossRelics", out ModTile relicTile))
                return [relicTile];
            else
            {
                ColouredModsRelics.Instance.Logger.Warn($"{ModName}: needs update");
                return [];
            }
        }
    }
}
