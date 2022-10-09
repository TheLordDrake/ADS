using AlgernonCommons.Patching;
using ColossalFramework.UI;
using ICities;

namespace ADS
{
    public class ADSMod : PatcherMod<UIPanel, PatcherBase>, IUserMod
    {
        public override string BaseName => "Advanced District Snapping";
        public string Description => "DESC: TODO";
        public override string HarmonyID => "com.thelorddrake.ads";

        public override void LoadSettings() { }

        public override void SaveSettings() { }
    }
}
