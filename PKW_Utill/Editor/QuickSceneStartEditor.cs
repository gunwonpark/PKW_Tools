using UnityEditor;

namespace PKW_Tool
{
    public class QuickSceneStartEditor : EditorWindow
    {
        [MenuItem("Scene/Select Scene")]
        private static void Init()
        {
            var window = GetWindow<QuickSceneStartEditor>();
            window.titleContent = new UnityEngine.GUIContent("Select Scene");
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Select Scene");

            foreach (var scene in UnityEditor.EditorBuildSettings.scenes)
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);
                if (UnityEngine.GUILayout.Button(sceneName))
                {
                    if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scene.path);
                    }
                }
            }
        }
    }
}
