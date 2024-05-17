using ColouredModsRelics.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace ColouredModsRelics.Common.Mods
{
    public class CalamityHuntRelic : ModRainbowRelics
    {
        private Hook hook;
        public override string ModName => "CalamityHunt";
        private delegate void orig_SpecialDraw(object self, int i, int j, SpriteBatch spriteBatch);
        public override void LoadHook()
        {
            hook = new(Mod.Code.GetType("CalamityHunt.Content.Tiles.Autoloaded.AutoloadedBossRelicTile").GetMethod("SpecialDraw", BindingFlags.Public | BindingFlags.Instance), On_SpecialDraw);
            hook.Apply();
        }
        public override void UnloadHook()
        {
            hook?.Dispose();
            hook = null;
        }
        public override IEnumerable<ModItem> GetRelicItems()
        {
            if (Mod.TryFind("GoozmaRelicItem", out ModItem relicItem))
                return [relicItem];
            else
            {
                ColouredModsRelics.Instance.Logger.Warn($"{ModName}: needs update");
                return [];
            }
        }
        public override IEnumerable<ModTile> GetRelicTiles()
        {
            if (Mod.TryFind("GoozmaRelicTile", out ModTile relicTile))
                return [relicTile];
            else
            {
                ColouredModsRelics.Instance.Logger.Warn($"{ModName}: needs update");
                return [];
            }
        }

        private static void On_SpecialDraw(orig_SpecialDraw orig, object self, int i, int j, SpriteBatch spriteBatch)
        {
            if (!RainbowLoader.TryGetInstance("CalamityHunt", out var relic) || !relic.Active)
            {
                orig(self, i, j, spriteBatch);
                return;
            }
            Vector2 offScreen = Main.drawToScreen ? Vector2.Zero : new(Main.offScreenRange);
            Point p = new(i, j);
            Tile tile = Main.tile[p];
            if (tile != null && tile.HasTile)
            {
                Texture2D texture = relic.ColoredRelicTileAssets[relic.RelicTileTypes[0]].Value;
                int frameY = tile.TileFrameX / 54;
                Rectangle frame = Utils.Frame(texture, 1, 1, 0, frameY, 0, 0);
                Vector2 origin = Utils.Size(frame) / 2f;
                Vector2 worldPos = Utils.ToWorldCoordinates(p, 24f, 64f);
                Color color = Lighting.GetColor(p.X, p.Y);
                SpriteEffects effects = (tile.TileFrameY / 72 != 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                float offset = (float)Math.Sin(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / 5f);
                Vector2 drawPos = worldPos + offScreen - Main.screenPosition + new Vector2(0f, -40f) + new Vector2(0f, offset * 4f);
                spriteBatch.Draw(texture, drawPos, frame, color, 0f, origin, 1f, effects, 0f);
                float scale = (float)Math.Sin(Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / 2f) * 0.3f + 0.7f;
                Color effectColor = (color * 0.1f * scale) with { A = 0 };
                for (int h = 0; h < 6; h++)
                {
                    spriteBatch.Draw(texture, drawPos + Utils.ToRotationVector2(MathHelper.TwoPi * h / 6f) * (6f + offset * 2f), frame, effectColor, 0f, origin, 1f, effects, 0f);
                }
            }
        }
    }
}
