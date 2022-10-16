using AlgernonCommons;
using AlgernonCommons.Patching;
using AlgernonCommons.Translation;
using ColossalFramework.IO;
using ICities;

namespace ADS.Source
{
    public class ADSMod : PatcherMod<OptionsPanel, PatcherBase>, IUserMod
    {
        public override string BaseName => "Advanced District Snapping";
        public string Description => Translations.Translate("MOD_DESCRIPTION");
        public override string HarmonyID => "com.thelorddrake.ads";

        public override void LoadSettings()
        {
            ModSettings.Load();
        }

        public override void SaveSettings() => ModSettings.Save();
    }

    public sealed class Loading : PatcherLoadingBase<OptionsPanel, PatcherBase> {}
}
