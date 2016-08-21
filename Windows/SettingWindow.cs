using UnityEngine;
using UnityEditor;

namespace WakaTime
{
    public class SettingWindow : EditorWindow
    {
        const string WAKATIME_URL = "https://wakatime.com/";

        [MenuItem("Window/WakaTime")]
        public static void ShowWindow()
        {
            GetWindow(typeof(SettingWindow));
        }

        void OnGUI()
        {
            GUILayout.Label("WakaTime configuration", EditorStyles.boldLabel);

            if (GUILayout.Button("Visit " + WAKATIME_URL))
            {
                Application.OpenURL(WAKATIME_URL);
            }

            EditorGUILayout.Separator();

            Main.IsEnabled = EditorGUILayout.Toggle("Enabled", Main.IsEnabled);

            EditorGUILayout.Separator();


            WakaTimeConfigFile.ApiKey = EditorGUILayout.TextField("API key", WakaTimeConfigFile.ApiKey);
            if (Main.IsEnabled && (WakaTimeConfigFile.ApiKey == null || string.IsNullOrEmpty(WakaTimeConfigFile.ApiKey)))
            {
                EditorGUILayout.HelpBox("API Key is required", MessageType.Error, false);
            }

            EditorGUILayout.Separator();

            WakaTimeConfigFile.Debug = EditorGUILayout.Toggle("Debug", WakaTimeConfigFile.Debug);
            EditorGUILayout.HelpBox("Debug messages will appear in the console if this option is enabled. Mostly used for test purposes.", MessageType.Info, true);
        }

        public static bool IsFocused()
        {
            return focusedWindow is SettingWindow;
        }

        public static EditorWindow GetWindow()
        {
            return GetWindow(typeof(SettingWindow));
        }
    }
}