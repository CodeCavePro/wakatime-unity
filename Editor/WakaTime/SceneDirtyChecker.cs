using UnityEditor;

namespace WakaTime
{

    [InitializeOnLoad]
    public static class SceneDirtyChecker
    {

        public static bool sceneIsDirty;

        static SceneDirtyChecker()
        {
            Undo.postprocessModifications += OnPostProcessModifications;
            Undo.undoRedoPerformed += OnUndoRedo;
        }

        static void OnUndoRedo()
        {
            string path = Main.GetProjectPath() + EditorApplication.currentScene;
            Main.OnSceneChanged(path);
        }

        static UndoPropertyModification[] OnPostProcessModifications(UndoPropertyModification[] propertyModifications)
        {
            sceneIsDirty = true;

            string path = Main.GetProjectPath() + EditorApplication.currentScene;
            Main.OnSceneChanged(path);

            return propertyModifications;
        }
    }
}