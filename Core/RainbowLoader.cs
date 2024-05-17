using ColouredModsRelics.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace ColouredModsRelics.Core
{
    [Autoload(Side = ModSide.Client)]
    public class RainbowLoader : ModSystem
    {
        public readonly static string[] SupportedMods = ["ThoriumMod", "Consolaria", "CatalystMod", "CalamityHunt", "NoxusBoss", "CalValEX", "SOTS", "SpiritMod"];

        // 单例
        public static List<ModRainbowRelics> RainbowRelicIntances = new();

        // ID表
        public static Dictionary<string, int> ModID = new();

        public override void Load()
        {
            IEnumerable<Type> modsRecord = AssemblyManager.GetLoadableTypes(Mod.Code).Where(type => type.IsSubclassOf(typeof(ModRainbowRelics)) && !type.IsAbstract && !type.ContainsGenericParameters);
            foreach (Type type in modsRecord)
            {
                ModRainbowRelics relic = Activator.CreateInstance(type) as ModRainbowRelics;
                if (ModLoader.TryGetMod(relic.ModName, out _) && SupportedMods.Contains(relic.ModName))
                {
                    ModID[relic.ModName] = RainbowRelicIntances.Count;
                    RainbowRelicIntances.Add(relic);
                    relic.LoadHook();
                }
            }
        }

        public override void Unload()
        {
            foreach (ModRainbowRelics mod in RainbowRelicIntances)
            {
                mod.UnloadHook();
            }
        }

        public override void PostSetupContent()
        {
            foreach (ModRainbowRelics mod in RainbowRelicIntances)
            {
                mod.LoadTileTextures();
                mod.LoadItemTextures();
            }
        }
        public static bool TryGetInstance(string name, out ModRainbowRelics relic)
        {
            if (ModID.TryGetValue(name, out int value))
            {
                relic = RainbowRelicIntances[value];
                return true;
            }
            relic = null;
            return false;

        }
    }
}
