
using ColouredModsRelics.Core;
using System.ComponentModel;
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

            QuickToggle("CalValEX", CalValEX);
            QuickToggle("CalamityHunt", CalamityHunt);
            QuickToggle("CatalystMod", CatalystMod);
            QuickToggle("Consolaria", Consolaria);
            QuickToggle("ContinentOfJourney", ContinentOfJourney);
            QuickToggle("Coralite", Coralite);
            QuickToggle("DDMod", DDMod);
            QuickToggle("NoxusBoss", NoxusBoss);
            QuickToggle("PrimeRework", PrimeRework);
            QuickToggle("Redemption", Redemption);
            QuickToggle("SOTS", SOTS);
            QuickToggle("SpiritMod", SpiritMod);
            QuickToggle("Spooky", Spooky);
            QuickToggle("StarlightRiver", StarlightRiver);
            QuickToggle("ThoriumMod", ThoriumMod);
        }

        [Header("RainbowHeader")]
        [DefaultValue(true)]
        public bool CalValEX;

        [DefaultValue(true)]
        public bool CalamityHunt;

        [DefaultValue(true)]
        public bool CatalystMod;

        [DefaultValue(true)]
        public bool Consolaria;

        [DefaultValue(true)]
        public bool ContinentOfJourney;

        [DefaultValue(true)]
        public bool Coralite;

        [DefaultValue(true)]
        public bool DDMod;

        [DefaultValue(true)]
        public bool NoxusBoss;

        [DefaultValue(true)]
        public bool PrimeRework;

        [DefaultValue(true)]
        public bool Redemption;

        [DefaultValue(true)]
        public bool SOTS;

        [DefaultValue(true)]
        public bool SpiritMod;

        [DefaultValue(true)]
        public bool Spooky;

        [DefaultValue(true)]
        public bool StarlightRiver;

        [DefaultValue(true)]
        public bool ThoriumMod;
    }
}