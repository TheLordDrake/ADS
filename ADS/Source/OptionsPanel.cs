using System;
using System.Linq;
using ColossalFramework.UI;
using UnityEngine;

namespace ADS.Source
{
    public class OptionsPanel :  UIPanel
    {
        private static readonly KeyCode[] KeyCodes = new KeyCode[]
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
                KeyCodes.Select(x => x.ToString()).ToArray(),
                Array.IndexOf(KeyCodes, ModSettings.HotKey) >= 0
                    ? Array.IndexOf(KeyCodes, ModSettings.HotKey)
                    : 0,
                OnHotKeyChanged
            );
        }

        private static void OnHotKeyChanged(int val)
        {
            ModSettings.HotKey = KeyCodes[val];
        }
    }
}
