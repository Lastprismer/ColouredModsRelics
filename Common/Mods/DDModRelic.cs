using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ColouredModsRelics.Common.Mods
{
    public class DDModRelic : ModRainbowRelics
    {
        private List<Asset<Texture2D>> extraTextures;
        public override string ModName => "DDMod";

        public DDModRelic() {
            extraTextures = new();
        }

        private static readonly string[] relicItems = ["LifeGuardRelic", "流星歼灭者圣物", "StarguardRelic", "哥布林巫师首领圣物", "枯萎的橡果之灵圣物", "流星掘地者圣物", "狱火蛇圣物"];

        private static readonly string[] relicTiles = ["LifeGuardRelicTile", "MeteorAnnihilatorRelicTile", "StraGuardRelicTile", "哥布林巫师首领圣物Tile", "枯萎橡果之灵圣物Tile", "流星掘地者圣物Tile", "狱火蛇圣物Tile"];

        private const string relicTileNamespace = "DDmod.Content.Tiles.Relic.";

        public override IEnumerable<ModItem> GetRelicItems()
        {
            List<ModItem> items = [];
            foreach (var r in relicItems)
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
            foreach (var r in relicTiles)
                if (Mod.TryFind(r, out ModTile relicTile))
                    tiles.Add(relicTile);
                else
                {
                    ColouredModsRelics.Instance.Logger.Warn($"{ModName}: needs update");
                    break;
                }
            return tiles;
        }

        public override void InitHooks()
        {
            Hooks = [];
            for (int idx = 0; idx < relicTiles.Length; idx++)
            {
                MethodBaseInfo info = new(ModName, relicTileNamespace + relicTiles[idx], "SpecialDraw");
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
            extraTextures.Add(RainbowUtils.Request(GetTilePath() + "LifeGuardRelicTile2"));
            extraTextures.Add(RainbowUtils.Request(GetTilePath() + "StraGuardRelicTile2"));

            MethodBaseInfo info2 = new(ModName, "DDmod.Content.Tiles.Relic.LifeGuardRelicTile", "PostDraw");
            var hook2 = info2.MakeILHook(il =>
            {
                ILCursor cursor = new(il);

                if (cursor.TryGotoNext(MoveType.After, i => i.MatchLdarg0(), i => i.MatchLdfld(out _), i => i.MatchCallvirt(out _)))
                {
                    cursor.EmitDelegate((Texture2D tex) => Active ? extraTextures[0].Value : tex);
                }
            });
            if (hook2 != null)
            {
                Hooks.Add(hook2);
            }

            info2 = new(ModName, "DDmod.Content.Tiles.Relic.StraGuardRelicTile", "PostDraw");
            hook2 = info2.MakeILHook(il =>
            {
                ILCursor cursor = new(il);

                if (cursor.TryGotoNext(MoveType.After, i => i.MatchLdarg0(), i => i.MatchLdfld(out _), i => i.MatchCallvirt(out _)))
                {
                    cursor.EmitDelegate((Texture2D tex) => Active ? extraTextures[1].Value : tex);
                }
            });
            if (hook2 != null)
            {
                Hooks.Add(hook2);
            }

            return;
        }
    }
}
