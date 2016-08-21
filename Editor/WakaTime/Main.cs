using UnityEditor;
using WakaTime.Unity;

namespace WakaTime
{
    [InitializeOnLoad]
    public static class Main
    {
        static readonly WakaTimeUnity3dPlugin _plugin;

        static Main()
        {
            _plugin = new WakaTimeUnity3dPlugin(default(Editor));
        }

        public static void OnAssetChanged(string assetName)
        {
            _plugin.OnDocumentOpened(assetName);
        }

        public static void OnAssetSaved(string assetName)
        {
            _plugin.OnDocumentChanged(assetName);
        }

        const string KEY_ENABLED = "wakatime_enabled";
        static bool _enabled = false;
        public static bool IsEnabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                EditorPrefs.SetBool(KEY_ENABLED, value);

                if (value)
                {
                    _plugin.CheckPrerequisites();
                }
            }
        }
    }
}
