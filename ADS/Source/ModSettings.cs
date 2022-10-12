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
        [XmlElement("HotKey")]
        public static KeyCode HotKey
        {
            get => Patches.HotKey;
            set=> Patches.HotKey = value;
        }

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
