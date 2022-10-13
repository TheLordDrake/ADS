using AlgernonCommons;
using AlgernonCommons.Patching;
using ColossalFramework.IO;
using ICities;

namespace ADS.Source
{
    public class ADSMod : PatcherMod<OptionsPanel, PatcherBase>, IUserMod
    {
        public override string BaseName => "Advanced District Snapping";
        public string Description => "Allows disabling snapping to networks when painting districts.";
        public override string HarmonyID => "com.thelorddrake.ads";

        public override void LoadSettings()
        {
            Logging.Message(DataLocation.localApplicationData);
            Logging.Message("KeyCode: " + ModSettings.HotKey);
            ModSettings.Load();
        }

        public override void SaveSettings() => ModSettings.Save();
    }

    public sealed class Loading : PatcherLoadingBase<OptionsPanel, PatcherBase> {}
}
