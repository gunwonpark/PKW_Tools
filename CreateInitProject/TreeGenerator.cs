using System.Collections.Generic;
using System.IO;

namespace PKW
{
    public static class TreeGenerator
    {
        public static int IDCounter;
        public static List<TreeElement> Start(string _projectName)
        {
            IDCounter = 0;
            var treeElements = new List<TreeElement>();

            var root = new TreeElement("Root", -1, IDCounter);
            treeElements.Add(root);

            if (!Directory.Exists($"Assets/{_projectName}"))
            {
                return treeElements;
            }

            var subRoot = new TreeElement("2.Scripts", 0, ++IDCounter);
            treeElements.Add(subRoot);
            ScriptDirectoriesToTree(subRoot, $"Assets/{_projectName}/2.Scripts", ref IDCounter, treeElements);
            return treeElements;
        }
        public static void ScriptDirectoriesToTree(TreeElement parent, string path, ref int id, List<TreeElement> treeElements)
        {
            var directories = Directory.GetDirectories(path);

            foreach (var dir in directories)
            {
                var dirInfo = new DirectoryInfo(dir);
                var folderElement = new TreeElement(dirInfo.Name, parent.depth + 1, ++IDCounter);
                treeElements.Add(folderElement);

                // Recursively add subdirectories
                ScriptDirectoriesToTree(folderElement, dir, ref id, treeElements);
            }
        }
    }
}
