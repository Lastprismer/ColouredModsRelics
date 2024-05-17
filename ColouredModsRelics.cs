using Terraria.ModLoader;

namespace ColouredModsRelics
{
    public class ColouredModsRelics : Mod
    {
        internal static ColouredModsRelics Instance { get; private set; }
        public override void Load()
        {
            Instance = this;
        }

        public override void Unload()
        {
            Instance = null;
        }
    }
}
