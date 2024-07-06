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
    public class ContinentOfJourneyRelic : ModRainbowRelics
    {
        public override string ModName => "ContinentOfJourney";

        private static readonly string[] relics = { "DiverRelic", "EverlastingFallingWhaleRelic", "LunarRelic", "MarquisMoonsquidRelic", "PriestessRodRelic", "ScarabBeliefRelic", "SolarRelic", "TheLifebringerRelic", "TheMaterealizerRelic", "TheMotherbrainRelic", "TheOverwatcherRelic", "TheSonRelic", "WallofShadowRelic" };

        private const string tileNamespace = "ContinentOfJourney.Tiles.";

        public override void InitHooks()
        {
            Hooks = [];
            for (int idx = 0; idx < relics.Length; idx++)
            {
                MethodBaseInfo info = new(ModName, tileNamespace + relics[idx], "SpecialDraw");
                var hook = info.MakeILHook(il =>
                {
                    ILCursor cursor = new(il);

                    if (cursor.TryGotoNext(MoveType.After, i => i.MatchLdarg0(), i => i.MatchLdfld(out _), i => i.MatchCallvirt(out _)))
                    {
                        int breakScopeChain = idx;
                        cursor.EmitDelegate((Texture2D tex) => Active ? ColoredRelicTileAssets[RelicTileTypes[breakScopeChain]].Value : tex);
                    }
                });
                if (hook != null)
                {
                    Hooks.Add(hook);
                }
            }
            return;
        }

        public override IEnumerable<ModItem> GetRelicItems()
        {
            List<ModItem> items = [];
            foreach (var r in relics)
            {
                if (Mod.TryFind(r, out ModItem relicItem))
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
