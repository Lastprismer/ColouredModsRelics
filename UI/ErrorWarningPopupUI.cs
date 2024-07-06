using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.GameInput;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.Audio;
using Terraria.ID;

namespace ColouredModsRelics.UI
{
    public class ErrorWarningPopupUI : UIState
    {
        private bool shouldBeRemoved;

        private bool isMouseHovering;

        private UIElement warningPanel;

        public ErrorWarningPopupUI() {
            shouldBeRemoved = false;
            isMouseHovering = false;
        }

        public override void OnInitialize()
        {
            UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("Workshop.Publish"), 0.7f, large: true);
            uITextPanel.Width.Set(-10f, 0.5f);
            uITextPanel.Height.Set(50f, 0f);
            uITextPanel.VAlign = 1f;
            uITextPanel.Top.Set(-40, 0f);
            uITextPanel.HAlign = 1f;
            uITextPanel.OnMouseOver += FadedMouseOver;
            uITextPanel.OnMouseOut += FadedMouseOut;
            // uITextPanel.OnLeftClick += Click_Publish;
            uITextPanel.SetSnapPoint("publish", 0);
            Append(uITextPanel);
            warningPanel = uITextPanel;
        }

        private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(SoundID.MenuTick);
            ((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
            ((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
        }

        private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
        {
            ((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
            ((UIPanel)evt.Target).BorderColor = Color.Black;
        }
    }
}
