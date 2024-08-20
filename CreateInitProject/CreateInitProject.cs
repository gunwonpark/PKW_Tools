using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

using static System.IO.Directory;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;
using static UnityEngine.Application;

namespace PKW
{
    public class CreateInitProject : EditorWindow
    {
        private string _projectName;
        private string _newScriptName = "";

        private TreeViewState _treeViewState;
        private CustomTreeView _treeView;
        private SearchField _searchField;

        private string _scriptFolderName;

        private Dictionary<string, List<string>> _scriptNames = new Dictionary<string, List<string>>();


        Rect TreeViewRect
        {
            get { return new Rect(10, 20, 200, position.height); }
        }
        Rect ContentRect
        {
            get { return new Rect(10 + 10 + 200, 20, 360, position.height); }
        }

        [MenuItem("PKW_Tool/CreateInitProject")]
        public static void Init()
        {
            CreateInitProject window = GetWindow<CreateInitProject>("Project Initialization");
            window.Show();
        }
        private void OnEnable()
        {
            _projectName = EditorPrefs.GetString("ProjectName", "_project");
            if (_treeViewState == null)
                _treeViewState = new TreeViewState();

            var treeModel = new TreeModel<TreeElement>(TreeGenerator.Start(_projectName));
            _treeView = new CustomTreeView(_treeViewState, treeModel);
            _treeView.OnSelectionChanged += GetSelectedFolderFullName;

            _searchField = new SearchField();
            _searchField.downOrUpArrowKeyPressed += _treeView.SetFocusAndEnsureSelectedItem;

        }
        private void OnDisable()
        {
            EditorPrefs.SetString("ProjectName", _projectName);
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(TreeViewRect);
            SearchBar();
            ShowFolderList();
            GUILayout.EndArea();

            GUILayout.BeginArea(ContentRect);
            ProjectName();
            GUILayout.Space(10);
            AddScript();
            GUILayout.Space(10);
            ShowWaitingScripts();

            GUILayout.Space(10);
            ButtonBar();
            GUILayout.EndArea();

        }
        private void SearchBar()
        {
            _treeView.searchString = _searchField.OnToolbarGUI(_treeView.searchString);
        }
        private void ShowFolderList()
        {
            Rect rect = GUILayoutUtility.GetRect(100, 300, 200, 400);
            _treeView.OnGUI(rect);
        }

        private void ProjectName()
        {
            GUILayout.Label("Enter your project name", EditorStyles.boldLabel);
            _projectName = EditorGUILayout.TextField("Name:", _projectName);
        }

        private void AddScript()
        {
            // ��ũ��Ʈ �̸� �Է�
            GUILayout.Label("Enter script names", EditorStyles.boldLabel);
            _newScriptName = EditorGUILayout.TextField("Script Name:", _newScriptName);

            // �� ��ũ��Ʈ ���� �̸� ǥ��
            EditorGUILayout.LabelField("Selected Folder:", _scriptFolderName);

            if (GUILayout.Button("Add Script"))
            {
                if (CheckScriptName(_newScriptName) && CheckFolderName(_scriptFolderName))
                {
                    _scriptNames.TryGetValue(_scriptFolderName, out List<string> scriptList);
                    if (scriptList == null)
                    {
                        scriptList = new List<string>();
                        _scriptNames.Add(_scriptFolderName, scriptList);
                    }
                    if (!scriptList.Contains(_newScriptName))
                    {
                        scriptList.Add(_newScriptName);
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error", "Script name already exists", "OK");
                    }
                    _newScriptName = "";
                }
            }
        }

        private bool CheckFolderName(string scriptFolderName)
        {
            if (string.IsNullOrEmpty(scriptFolderName))
            {
                EditorUtility.DisplayDialog("Error", "please select folder or init project", "OK");
                return false;
            }
            return true;
        }

        private bool CheckScriptName(string scriptName)
        {
            // ����ְų� ������ ������ false
            if (string.IsNullOrEmpty(scriptName))
            {
                EditorUtility.DisplayDialog("Error", "Script name is required", "OK");
                return false;
            }
            if (scriptName.Contains(" "))
            {
                EditorUtility.DisplayDialog("Error", "Script name cannot contain spaces", "OK");
                return false;
            }
            return true;
        }
        private void ShowWaitingScripts()
        {
            GUILayout.Label("Scripts to be created:");

            foreach (KeyValuePair<string, List<string>> scriptName in _scriptNames)
            {
                GUILayout.Label($"FolderPath : {scriptName.Key}");
                foreach (string value in scriptName.Value)
                {
                    GUILayout.Label(value);
                }
                GUILayout.Space(10);
            }
        }
        private void ButtonBar()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("CreateDefaultProject"))
                {
                    if (!string.IsNullOrEmpty(_projectName))
                    {
                        FastProjectMaker.CreateDefaultProject(_projectName);
                        Close();
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error", "Project name is required", "OK");
                    }
                }
                if (GUILayout.Button("CreateScripts"))
                {
                    if (!string.IsNullOrEmpty(_projectName) && _scriptNames.Count > 0)
                    {
                        FastProjectMaker.CreateScripts(_projectName, _scriptNames);
                        Close();
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error", "Project name and at least one script name are required", "OK");
                    }
                }
                if (GUILayout.Button("Create Project"))
                {
                    if (!string.IsNullOrEmpty(_projectName))
                    {
                        FastProjectMaker.CreateProject(_projectName, _scriptNames);
                        Close();
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("Error", "Project name and at least one script name are required", "OK");
                    }
                }
            }
        }

        void GetSelectedFolderFullName(IList<string> elements)
        {
            _scriptFolderName = Combine(elements.ToArray());
        }
    }

    public static class FastProjectMaker
    {
        public static void CreateDefaultProject(string _projectName)
        {
            string rootProjectName = _projectName;
            CreateDefaultFolders(rootProjectName);
            CreateDefaultScripts(rootProjectName);
            Refresh();
        }
        public static void CreateProject(string _projectName, Dictionary<string, List<string>> _scriptsName)
        {
            string rootProjectName = _projectName;
            CreateDefaultFolders(rootProjectName);
            string _template = BaseTemplate;
            _template = _template.Replace("#PROJECTNAME#", rootProjectName);
            CreateScripts(rootProjectName, _scriptsName, _template);
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
        public static void CreateScripts(string root, Dictionary<string, List<string>> scriptsName, string template = BaseTemplate)
        {
            string scriptPath = Combine(dataPath, root, "2.Scripts");

            template = template.Replace("#PROJECTNAME#", root);

            foreach (KeyValuePair<string, List<string>> scriptName in scriptsName)
            {
                string folder = GetFolderName(scriptName.Key);
                string path = scriptPath;
                if (!string.IsNullOrEmpty(folder))
                {
                    path = Combine(path, folder);
                }

                if (folder.Contains("Managers"))
                {
                    template = BaseManagerTemplate;
                }

                foreach (string name in scriptName.Value)
                {
                    CreateScript(path, name, template);
                }
            }

            Refresh();
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
        private static string GetFolderName(string name)
        {
            return string.Join('\\', name.Split('\\').Skip(1));
        }
        private const string BaseTemplate =
@"using UnityEngine;

namespace #PROJECTNAME#
{
    public class #SCRIPTNAME# : MonoBehaviour
    {
        public void Init()
        {

        }
    }
}";

        private const string BaseManagerTemplate =
@"namespace #PROJECTNAME#
{
    public class #SCRIPTNAME#
    {
        private #SCRIPTNAME# _instance;
        public #SCRIPTNAME# Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new #SCRIPTNAME#();
                }
                return _instance;
            }
        }
        public void Init()
        {

        }

        public void Clear()
        {

        }
    }
}";
    }
}

