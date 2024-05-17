using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ColouredModsRelics.Common.Mods
{
    public class CalValEXRelic : ModRainbowRelics
    {
        public override string ModName => "CalValEX";

        public override MethodBase HookOrigin => ModLoader.GetMod("CalamityMod").Code.GetType(BaseRelicTileType).GetMethod("SpecialDraw", BindingFlags.Public | BindingFlags.Instance);

        public override ILContext.Manipulator Manip => il =>
        {
            ILCursor cursor = new(il);

            if (cursor.TryGotoNext(MoveType.After, i => i.MatchLdarg0(), i => i.MatchLdfld(out _), i => i.MatchCallvirt(out _)))
            {
                cursor.EmitLdarg0();
                cursor.EmitCall(typeof(ModTile).GetProperty("Type", BindingFlags.Instance | BindingFlags.Public).GetGetMethod());
                cursor.EmitDelegate<Func<Texture2D, int, Texture2D>>((tex, type) => (Active && type == RelicTileTypes[0]) ? ColoredRelicTileAssets[type].Value : tex);
            }
        };

        public override string BaseRelicTileType => "CalamityMod.Tiles.BaseTiles.BaseBossRelic";

        public override IEnumerable<ModItem> GetRelicItems()
        {
            if (Mod.TryFind("MeldosaurusRelic", out ModItem relicItem))
                return [relicItem];
            else
            {
                ColouredModsRelics.Instance.Logger.Warn($"{ModName}: needs update");
                return [];
            }
        }

        public override IEnumerable<ModTile> GetRelicTiles()
        {
            if (Mod.TryFind("MeldosaurusRelicPlaced", out ModTile relicTile))
                return [relicTile];
            else
            {
                ColouredModsRelics.Instance.Logger.Warn($"{ModName}: needs update");
                return [];
            }
        }
    }
}
