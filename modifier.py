import re

supported_mods = [
    "CalValEX",
    "CalamityHunt",
    "CatalystMod",
    "Consolaria",
    "ContinentOfJourney",
    "Coralite",
    "DDMod",
    "NoxusBoss",
    "PrimeRework",
    "Redemption",
    "SOTS",
    "SpiritMod",
    "Spooky",
    "StarlightRiver",
    "ThoriumMod",
]


def findn(s, sub, n):
    val = -1
    for _ in range(n):
        val = s.find(sub, val + 1)
    return val


def update(file_path, regex=None, func=None, func_code=None):
    with open(file_path, "r", encoding="utf8") as f:
        code = f.read()

    if func is not None:
        match = re.search(regex, code)
        line = match.group(0)
        if match:
            updated_line = func(line)
            updated_code = code.replace(line, updated_line)
            with open(file_path, "w", encoding="utf8") as f:
                f.write(updated_code)
                print(f"update {file_path}")
                return

    if func_code is not None:
        updated_code = func_code(code)
        with open(file_path, "w", encoding="utf8") as f:
            f.write(updated_code)
            print(f"update {file_path}")
            return


if __name__ == "__main__":

    def update_loader(line) -> str:
        return (
            line[: findn(line, "[", 2)]
            + "["
            + ", ".join((f'"{s}"' for s in supported_mods))
            + "];"
        )

    def update_build(line) -> str:
        return "softAfter = " + ", ".join(supported_mods) + "\n"

    def update_config(code) -> str:
        sorted_quick_toggles = "\n".join(
            [" " * 12 + f'QuickToggle("{mod}", {mod});' for mod in supported_mods]
        )
        sorted_bool_vars = "\n\n".join(
            [
                " " * 8 + "[DefaultValue(true)]\n" + " " * 8 + f"public bool {mod};"
                for mod in supported_mods
            ]
        )

        sorted_code = r"""
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

{sorted_quick_toggles}
        }

        [Header("RainbowHeader")]
{sorted_bool_vars}
    }
}""".replace(
            r"{sorted_quick_toggles}", sorted_quick_toggles
        ).replace(
            r"{sorted_bool_vars}", sorted_bool_vars
        )

        return sorted_code

    supported_mods.sort()

    update(
        "Core\RainbowLoader.cs",
        r"public static readonly string\[\] SupportedMods = \[.*?\];",
        update_loader,
    )

    update("build.txt", r"softAfter = .*?\n", update_build)

    update("Common\Config\RainbowConfig.cs", func_code=update_config)
