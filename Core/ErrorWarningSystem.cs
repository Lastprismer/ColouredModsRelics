/*using ColouredModsRelics.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ColouredModsRelics.Core

{
    public class ErrorWarningSystem : ModSystem
    {
        private bool hasPopupError;

        private FieldInfo modloader_public_static_bool_isLoading;

        private UserInterface errorPopupUserInterface;

        private ErrorWarningPopupUI errorWarningPopupUI;

        public override void Load()
        {
            modloader_public_static_bool_isLoading = typeof(ModLoader).GetField("isLoading", BindingFlags.Static | BindingFlags.NonPublic);
            On_Main.UpdateUIStates += On_Main_UpdateUIStates;

            errorPopupUserInterface = new();
            errorWarningPopupUI = new();
            errorPopupUserInterface.SetState(errorWarningPopupUI);
            errorWarningPopupUI.Activate();
        }

        public override void OnModLoad()
        {
            hasPopupError = false;
        }

        public override void Unload()
        {
            On_Main.UpdateUIStates -= On_Main_UpdateUIStates;
        }

        private void On_Main_UpdateUIStates(On_Main.orig_UpdateUIStates orig, GameTime gameTime)
        {
            bool isLoading = (bool)modloader_public_static_bool_isLoading.GetValue(null);
            if (Main.gameMenu && !isLoading)
            {
                UpdateGameMenu(gameTime);
            }
            orig(gameTime);
        }

        private void UpdateGameMenu(GameTime gameTime)
        {
            if (!hasPopupError)
            {
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            base.ModifyInterfaceLayers(layers);
        }
    }
}
*/