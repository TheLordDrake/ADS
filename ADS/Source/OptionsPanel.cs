using System;
using System.Collections.Generic;
using System.Linq;
using AlgernonCommons.Translation;
using AlgernonCommons.UI;
using ColossalFramework.UI;
using UnityEngine;

namespace ADS.Source
{
    public class OptionsPanel :  UIPanel
    {
        private static readonly KeyCode[] KeyCodes = {
            KeyCode.LeftAlt,
            KeyCode.RightAlt,
            KeyCode.LeftCommand,
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
            var localizedKeyCodes = new Dictionary<string, string>
            {
                { KeyCode.LeftAlt.ToString(), Translations.Translate("LEFT_ALT") },
                { KeyCode.RightAlt.ToString(), Translations.Translate("RIGHT_ALT") },
                { KeyCode.LeftCommand.ToString(), Translations.Translate("LEFT_COMMAND") },
                { KeyCode.LeftControl.ToString(), Translations.Translate("LEFT_CTRL") },
                { KeyCode.RightControl.ToString(), Translations.Translate("RIGHT_CTRL") },
                { KeyCode.LeftShift.ToString(), Translations.Translate("LEFT_SHIFT") },
                { KeyCode.RightShift.ToString(), Translations.Translate("RIGHT_SHIFT") }
            };

            // Auto layout
            autoLayout = true;
            autoLayoutDirection = LayoutDirection.Vertical;
            var helper = new UIHelper(this);

            var languageGroup = helper.AddGroup(Translations.Translate(("SET_LANGUAGE")));
            var languageDropDown = (UIDropDown)languageGroup.AddDropdown(
                Translations.Translate("SET_LANGUAGE"),
                Translations.LanguageList,
                Translations.Index,
                value =>
                {
                    Translations.Index = value;
                    OptionsPanelManager<OptionsPanel>.LocaleChanged();
                });

            var hotkeyGroup = helper.AddGroup(Translations.Translate("HOTKEY"));
            var hotKeyDropDown = (UIDropDown)hotkeyGroup.AddDropdown(
                Translations.Translate("HOTKEY"),
                KeyCodes.Select(x => localizedKeyCodes[x.ToString()]).ToArray(),
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
