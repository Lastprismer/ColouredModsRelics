using System.Collections.Generic;
using System.Reflection;
using System;
using Terraria.ModLoader;
using System.Linq;
using MonoMod.Utils;

namespace ColouredModsRelics
{
    public class ColouredModsRelics : Mod
    {
        public static ColouredModsRelics Instance { get; private set; }
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
