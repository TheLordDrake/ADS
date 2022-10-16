using System.IO;
using System.Xml.Serialization;
using AlgernonCommons.XML;
using ColossalFramework.IO;
using UnityEngine;

namespace ADS.Source
{
    /// <summary>
    /// XML settings file
    /// </summary>
    [XmlRoot("AdvancedDistrictSnapping")]
    public class ModSettings : SettingsXMLBase
    {
        // Settings file name
        [XmlIgnore] private static readonly string SettingsFileName =
            Path.Combine(DataLocation.localApplicationData, "ADS.xml");

        /// <summary>
        /// Hotkey to disable snapping
        /// </summary>
        [XmlElement("Hotkey")]
        public KeyCode XmlHotKey
        {
            get => Hotkey;
            set => Hotkey = value;
        }

        internal static KeyCode Hotkey { get; set; } = KeyCode.LeftAlt;

        /// <summary>
        /// Loads setting from file
        /// </summary>
        internal static void Load() => XMLFileUtils.Load<ModSettings>(SettingsFileName);

        /// <summary>
        /// Saves settings to file
        /// </summary>
        internal static void Save() => XMLFileUtils.Save<ModSettings>(SettingsFileName);
    }
}
