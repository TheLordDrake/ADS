using System;
using System.Linq;
using AlgernonCommons.Translation;
using AlgernonCommons.UI;
using ColossalFramework.UI;
using UnityEngine;

namespace ADS.Source
{
    public class OptionsPanel :  UIPanel
    {
        // TODO: Create a string map to make the names Human Readable (Natural text)
        private static readonly KeyCode[] KeyCodes = new KeyCode[]
        {
            KeyCode.LeftAlt,
            KeyCode.RightAlt,
            KeyCode.LeftCommand,
            KeyCode.RightCommand, // TODO: Remove
            KeyCode.LeftControl,
            KeyCode.RightControl,
            KeyCode.LeftShift,
            KeyCode.RightShift
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsPanel"/> class
        /// </summary>
        public OptionsPanel()
        {
            // Auto layout
            autoLayout = true;
            autoLayoutDirection = LayoutDirection.Vertical;
            var helper = new UIHelper(this);

            var languageGroup = helper.AddGroup(Translations.Translate(("SET_LANGUAGE")));
            var languageDropDown = (UIDropDown)languageGroup.AddDropdown(
                Translations.Translate("SET_LANGUAGE"),
                Translations.LanguageList,
                Translations.Index,
                (value) =>
                {
                    Translations.Index = value;
                    OptionsPanelManager<OptionsPanel>.LocaleChanged();
                });

            var hotkeyGroup = helper.AddGroup(Translations.Translate("HOTKEY"));
            var hotKeyDropDown = (UIDropDown)hotkeyGroup.AddDropdown(
                Translations.Translate("HOTKEY"),
                KeyCodes.Select(x => x.ToString()).ToArray(),
                Array.IndexOf(KeyCodes, ModSettings.Hotkey) >= 0
                    ? Array.IndexOf(KeyCodes, ModSettings.Hotkey)
                    : 0,
                OnHotKeyChanged
            );
        }

        private static void OnHotKeyChanged(int val)
        {
            ModSettings.Hotkey = KeyCodes[val];
        }
    }
}
