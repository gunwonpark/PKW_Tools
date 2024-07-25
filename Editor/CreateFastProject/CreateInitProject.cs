using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;
using static UnityEngine.Application;

public enum ScriptType
{
    Normal,
    Manager,
}

namespace PKW
{
    public class CreateInitProject : EditorWindow
    {
        private string _projectName;
        private Dictionary<ScriptType, List<string>> _scriptNames = new Dictionary<ScriptType, List<string>>();
        private string _newScriptName = "";
        private bool _isManagerScript = false;

        [MenuItem("PKW_Tool/CreateInitProject")]
        public static void ShowWindow()
        {
            CreateInitProject window = GetWindow<CreateInitProject>("Project Initialization");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Enter your project name", EditorStyles.boldLabel);
            _projectName = EditorGUILayout.TextField("Name:", _projectName);

            GUILayout.Space(10);
            GUILayout.Label("Enter script names", EditorStyles.boldLabel);
            _newScriptName = EditorGUILayout.TextField("Script Name:", _newScriptName);
            _isManagerScript = EditorGUILayout.Toggle("Is Manager Script:", _isManagerScript);
            if (GUILayout.Button("Add Script"))
            {
                if (!string.IsNullOrEmpty(_newScriptName))
                {
                    ScriptType scriptType = _isManagerScript ? ScriptType.Manager : ScriptType.Normal;
                    _scriptNames.TryGetValue(scriptType, out List<string> scriptList);
                    if (scriptList == null)
                    {
                        scriptList = new List<string>();
                        _scriptNames.Add(scriptType, scriptList);
                    }
                    scriptList.Add(_newScriptName);
                    _newScriptName = "";
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Script name cannot be empty", "OK");
                }
            }

            GUILayout.Space(10);
            GUILayout.Label("Scripts to be created:");

            foreach (KeyValuePair<ScriptType, List<string>> scriptName in _scriptNames)
            {
                GUILayout.Label($"Type : {scriptName.Key}");
                foreach (string value in scriptName.Value)
                {
                    GUILayout.Label(value);
                }
                GUILayout.Space(5);
            }

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("CreateDefaultProject"))
                {
                    FastProjectMaker.CreateDefaultProject("_Project");
                    Close();
                }
                if (GUILayout.Button("Create Project"))
                {
                    if (!string.IsNullOrEmpty(_projectName) && _scriptNames.Count > 0)
                    {

                        FastProjectMaker.CreateProject(_projectName, _scriptNames);
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error", "Project name and at least one script name are required", "OK");
                    }
                    Close();
                }
            }
            GUILayout.EndHorizontal();
        }
    }

    public static class FastProjectMaker
    {
        // �ɼ�â�� PKW_Tool�޴��� �߰��ȴ�
        public static void CreateDefaultProject(string _projectName)
        {
            string rootProjectName = _projectName;
            CreateDefaultFolders(rootProjectName);
            CreateDefaultScripts(rootProjectName);
            Refresh();
        }
        public static void CreateProject(string _projectName, Dictionary<ScriptType, List<string>> _scriptsName)
        {
            string rootProjectName = _projectName;
            CreateDefaultFolders(rootProjectName);
            CreateScripts(rootProjectName, _scriptsName);
            Refresh();
        }
        private static void CreateDefaultFolders(string root)
        {
            // ������� root ���丮�� �̸� �״����� �׾ȿ� ������ų �������� �����ϸ� �ȴ�
            CreateDirectorys(root, "1.Arts", "2.Scripts", "3.Prefabs", "4.Resources", "5.ScriptableObjects", "6.Scenes");
        }
        private static void CreateDirectorys(string root, params string[] dir)
        {
            string fullPath = Combine(dataPath, root);
            foreach (string newDirectory in dir)
            {
                CreateDirectory(Combine(fullPath, newDirectory));
            }

        }
        private static void CreateDefaultScripts(string root)
        {
            string scriptPath = Combine(dataPath, root, "2.Scripts");

            // �Ϲ����� ��ũ��Ʈ ���� ���
            // string testScrip = "TestScript";
            // CreateScript(scriptPath, testScrip, BaseTemplate);

            // Manager ��ũ��Ʈ ���� ���
            List<string> createManagerScripts = new List<string>()
            {
                "GameManager",
                "DataManager",
                "ResourceManager",
                "UIManager",
                "SoundManager",
                "EffectManager",
                "InputManager",
            };

            scriptPath = Combine(scriptPath, "Managers");
            CreateDirectory(scriptPath);
            foreach (string scriptName in createManagerScripts)
            {
                CreateScript(scriptPath, scriptName, BaseTemplate);
            }
        }

        /// <summary>
        /// ��ũ��Ʈ�� 2.Scripts ������ �����ϴ� �Լ�
        /// </summary>
        /// <param name="root">root ���丮</param>
        /// <param name="scriptNames">������ ��ũ��Ʈ���� �̸�</param>
        /// <param name="scriptType">��ũ��Ʈ�� ������ ���� ������ ���� ��ġ ����</param>
        /// <param name="template">��ũ��Ʈ ���ø� ����</param>
        private static void CreateScripts(string root, Dictionary<ScriptType, List<string>> scriptsName, string template = BaseTemplate)
        {
            string scriptPath = Combine(dataPath, root, "2.Scripts");

            foreach (KeyValuePair<ScriptType, List<string>> scriptName in scriptsName)
            {
                switch (scriptName.Key)
                {
                    case ScriptType.Manager:
                        scriptPath = Combine(scriptPath, "Managers");
                        CreateDirectory(scriptPath);
                        break;
                    default:
                        break;
                }

                foreach (string name in scriptName.Value)
                {
                    CreateScript(scriptPath, name, template);
                }

            }

        }
        private static void CreateScript(string path, string scriptName, string template)
        {
            string fullPath = Combine(path, scriptName + ".cs");
            //�̹� �����Ǿ� �ִ� ��ũ��Ʈ�� �ٽ� �������� �ʴ´�.
            if (!File.Exists(fullPath))
            {
                File.WriteAllText(fullPath, template.Replace("#SCRIPTNAME#", scriptName));
            }
        }

        private const string BaseTemplate =
@"using UnityEngine;

public class #SCRIPTNAME# : MonoBehaviour
{
    void Init()
    {

    }
}";

    }
}

