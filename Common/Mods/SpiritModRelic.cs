using Humanizer;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
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

        public override void InitHooks()
        {
            try
            {
                var method = Mod.Code.GetType("SpiritMod.Tiles.Relics.BaseRelic`1").MakeGenericType(Mod.Code.GetType("SpiritMod.Items.Placeable.Relics.AtlasRelicItem")).GetMethod("SpecialDraw", BindingFlags.Public | BindingFlags.Instance);
                Hooks = [new ILHook(method, Manip)];
                return;
            }
            catch (Exception e)
            {
                ColouredModsRelics.Instance.Logger.Error("Exception: 魂灵ILHook加载失败，请尝试检查游戏完整性，或将信息报告给圣物上色 Mod 开发者：{0}".FormatWith(e.Message));
                Hooks = [];
                return;
            }
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

        private static readonly string[] relics = ["AtlasRelic", "AvianRelic", "DuskingRelic", "FrostSaucerRelic", "InfernonRelic", "MJWRelic", "OccultistRelic", "RlyehianRelic", "ScarabeusRelic", "StarplateRelic", "VinewrathRelic"];

        public override IEnumerable<ModItem> GetRelicItems()
        {
            List<ModItem> items = new();
            foreach (var relic in relics)
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
            foreach (var relic in relics)
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
