using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace PKW
{
    public class EditorHelper : MonoBehaviour
    {
#if Test
        const string path = "Assets/Editor/Enums";
#else
    const string path = "Pakages/PKW_Tools/Enums";
#endif
        internal static void CreateEnumStructure(string enumName, StringBuilder sb)
        {
            string enumPath = path;
            if (!Directory.Exists(enumPath))
            {
                Directory.CreateDirectory(enumPath);
            }

            string fileName = Path.Combine(enumPath, enumName + ".cs");

            File.WriteAllText(fileName, enumTemplate.Replace("$ENUM_NAME$", enumName).Replace("$ENUM_DATA$", sb.ToString()));

            AssetDatabase.ImportAsset(fileName);
        }

        const string enumTemplate = @"
public enum $ENUM_NAME$
{
$ENUM_DATA$
}
";
    }
}
