using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using static Terraria.ModLoader.ModContent;

namespace ColouredModsRelics.Common
{
    public static class RainbowUtils
    {
        public static Asset<Texture2D> Request(string path) => Request<Texture2D>(path, AssetRequestMode.AsyncLoad);

        public static void InvalidILError(string modName)
        {
            ColouredModsRelics.Instance.Logger.Warn($"Exception: {modName} ILHook运行失败，请尝试检查游戏完整性，或将信息报告给圣物上色Mod开发者：");
        }
    }
}
