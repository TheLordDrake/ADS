using System;
using System.Linq;
using AlgernonCommons;
using ColossalFramework.UI;
using UnityEngine;

namespace ADS.Source
{
    public class OptionsPanel :  UIPanel
    {
        private static readonly KeyCode[] _keyCodes = new KeyCode[]
        {
            KeyCode.LeftAlt,
            KeyCode.RightAlt,
            KeyCode.LeftCommand,
            KeyCode.RightCommand,
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

            var hotkeyGroup = helper.AddGroup("HotKey");
            var hotKeyDropDown = (UIDropDown)hotkeyGroup.AddDropdown(
                "HotKey",
                _keyCodes.Select(x => x.ToString()).ToArray(),
                Array.IndexOf(_keyCodes, ModSettings.HotKey) >= 0
                    ? Array.IndexOf(_keyCodes, ModSettings.HotKey)
                    : 0,
                OnHotKeyChanged
            );
        }

        private static void OnHotKeyChanged(int val)
        {
            Logging.Message("Setting HotKey: " + _keyCodes[val]);
            ModSettings.HotKey = _keyCodes[val];
            Logging.Message("HotKey: " + ModSettings.HotKey);
        }
    }
}
