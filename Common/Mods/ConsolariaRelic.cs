using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.ModLoader;

namespace ColouredModsRelics.Common.Mods
{
    public class ConsolariaRelic : ModRainbowRelics
    {
        public override string ModName => "Consolaria";

        private static readonly string[] bosses = { "LepusRelic", "TurkorRelic", "OcramRelic" };

        public ConsolariaRelic() : base() {
            HookInfo = new MethodBaseInfo(ModName, BaseRelicTileType, "SpecialDraw");
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

        public override string BaseRelicTileType => "Consolaria.Content.Tiles.BossRelic";

        public override IEnumerable<ModItem> GetRelicItems()
        {
            List<ModItem> items = new();
            foreach (var relic in bosses)
                if (Mod.TryFind(relic, out ModItem relicItem))
                    items.Add(relicItem);
                else
                {
                    ColouredModsRelics.Instance.Logger.Warn($"{ModName}: needs update");
                    break;
                }
            return items;
        }
    }
}
