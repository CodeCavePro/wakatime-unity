using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace WakaTime.Unity
{
    public class WakaTimeUnity3dPlugin : WakaTimeIdePlugin<Editor>
    {
        Unity3dLogger _unityLogger;
        Scene? _currentScene;

        public WakaTimeUnity3dPlugin(Editor editor) : base(editor)
        {
            _currentScene = null;
        }

        public override void BindEditorEvents()
        {
            SceneManager.activeSceneChanged += SceneManager_ActiveSceneChanged;

            EditorApplication.hierarchyWindowChanged += EditorApplication_OnWindowChanged;
            EditorApplication.playmodeStateChanged += EditorApplication_PlaymodeStateChanged;
            EditorApplication.projectWindowChanged += EditorApplication_ProjectWindowChanged;

            Undo.postprocessModifications += Undo_OnPostProcessModifications;
            Undo.undoRedoPerformed += Undo_OnUndoRedo;
        }

        public override void Dispose(bool disposing)
        {
            // TODO release managed resources
            SceneManager.activeSceneChanged -= SceneManager_ActiveSceneChanged;

            EditorApplication.hierarchyWindowChanged -= EditorApplication_OnWindowChanged;
            EditorApplication.playmodeStateChanged -= EditorApplication_PlaymodeStateChanged;
            EditorApplication.projectWindowChanged -= EditorApplication_ProjectWindowChanged;

            Undo.postprocessModifications -= Undo_OnPostProcessModifications;
            Undo.undoRedoPerformed -= Undo_OnUndoRedo;
        }

        void SceneManager_ActiveSceneChanged(Scene scene0, Scene scene1)
        {
            _currentScene = scene1;
        }

        void EditorApplication_OnWindowChanged()
        {
            SceneSoftChanged();
        }

        void EditorApplication_PlaymodeStateChanged()
        {
            SceneChanged();
        }

        void EditorApplication_ProjectWindowChanged()
        {
            SceneSoftChanged();
        }

        public void OnSceneChanged(string path)
        {
            OnDocumentChanged(path);
        }

        UndoPropertyModification[] Undo_OnPostProcessModifications(UndoPropertyModification[] modifications)
        {
            if (modifications != null && modifications.Any())
            {
                Undo_OnUndoRedo();
            }

            return modifications;
        }

        void Undo_OnUndoRedo()
        {
            SceneChanged();
        }

        void SceneChanged()
        {
            var solution = GetActiveSolutionPath();
            if (solution == null && string.IsNullOrEmpty(solution.Trim()))
            {
                return;
            }

            OnDocumentChanged(solution);
        }

        void SceneSoftChanged()
        {
            var solution = GetActiveSolutionPath();
            if (solution == null && string.IsNullOrEmpty(solution.Trim()))
            {
                return;
            }

            OnDocumentOpened(solution);
        }

        public override string GetActiveSolutionPath()
        {
            return (_currentScene != null)
                ? _currentScene.GetValueOrDefault().path
                : null;
        }

        public override string GetProjectName()
        {
            var parts = Application.dataPath.Split('/');
            var projectName = parts[parts.Length - 2];

            return projectName;
        }

        public override EditorInfo GetEditorInfo()
        {
            return new EditorInfo
            {
                Name = "unity",
                Version = editorObj.GetType().Assembly.GetName().Version,
                PluginKey = "wakatime-unity",
                PluginName = "WakaTime",
                PluginVersion = typeof(WakaTimeUnity3dPlugin).Assembly.GetName().Version,
            };
        }

        public override ILogService GetLogger()
        {
            if (_unityLogger == null)
                _unityLogger = new Unity3dLogger();

            return _unityLogger;
        }

        public override IDownloadProgressReporter GetReporter()
        {
            return new DownloadProgressWindow();
        }

        public override void PromptApiKey()
        {
            SettingWindow.ShowWindow();
        }

        public override void SettingsPopup()
        {
            SettingWindow.ShowWindow();
        }
    }
}
