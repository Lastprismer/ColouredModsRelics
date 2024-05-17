using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using static Terraria.ModLoader.ModContent;

namespace ColouredModsRelics.Common
{
    public static class RainbowUtils
    {
        public static Asset<Texture2D> Request(string path) => Request<Texture2D>(path, AssetRequestMode.AsyncLoad);

    }
}
