using ColouredModsRelics.Core;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace ColouredModsRelics.Common.Config
{
    public class RainbowConfig : ModConfig
    {
        public static RainbowConfig Instance { get; private set; }

        public override ConfigScope Mode => ConfigScope.ClientSide;

        public override void OnLoaded()
        {
            Instance = this;
        }

        public override void OnChanged()
        {
            void QuickToggle(string mod, bool value)
            {
                if (RainbowLoader.TryGetInstance(mod, out var relic))
                {
                    relic.ToggleTextures(value);
                }
            }

            QuickToggle("ThoriumMod", ThoriumMod);
            QuickToggle("Consolaria", Consolaria);
            QuickToggle("CatalystMod", CatalystMod);
            QuickToggle("CalamityHunt", CalamityHunt);
            QuickToggle("NoxusBoss", NoxusBoss);
            QuickToggle("CalValEX", CalValEX);
            QuickToggle("SOTS", SOTS);
            QuickToggle("SpiritMod", SpiritMod);
        }

        [Header("RainbowHeader")]
        [DefaultValue(true)]
        public bool ThoriumMod;

        [DefaultValue(true)]
        public bool Consolaria;

        [DefaultValue(true)]
        public bool CatalystMod;

        [DefaultValue(true)]
        public bool CalamityHunt;

        [DefaultValue(true)]
        public bool NoxusBoss;

        [DefaultValue(true)]
        public bool CalValEX;

        [DefaultValue(true)]
        public bool SOTS;

        [DefaultValue(true)]
        public bool SpiritMod;
    }
}
