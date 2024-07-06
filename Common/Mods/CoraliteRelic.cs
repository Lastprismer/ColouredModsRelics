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
    public class CoraliteRelic : ModRainbowRelics
    {
        public override string ModName => "Coralite";

        public CoraliteRelic() : base()
        {
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

        public override string BaseRelicItemType => "Coralite.Core.Prefabs.Items.BaseRelicItem";

        public override string BaseRelicTileType => "Coralite.Core.Prefabs.Tiles.BaseBossRelicTile";

        public override IEnumerable<ModItem> GetRelicItems()
        {
            return base.GetRelicItems().Where(i => i.Name != "GelThrone" && i.Name != "BloodiancieRelic");
        }

        public override IEnumerable<ModTile> GetRelicTiles()
        {
            return base.GetRelicTiles().Where(t => t.Name != "GelThroneTile" && t.Name != "BloodiancieRelicTile");
        }
    }
}
