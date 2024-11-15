using UnityEditor;
using UnityEngine;

namespace PKW_Attributes
{
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
    public class SerializableDictionaryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            var keys = property.FindPropertyRelative("keys");
            var values = property.FindPropertyRelative("values");

            int count = keys.arraySize;

            if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true))
            {
                // ег е╟
                EditorGUI.indentLevel++;
                for (int i = 0; i < count; i++)
                {
                    EditorGUILayout.PropertyField(keys.GetArrayElementAtIndex(i), GUIContent.none);
                    EditorGUILayout.PropertyField(values.GetArrayElementAtIndex(i), GUIContent.none);
                }

                if (GUILayout.Button("Add"))
                {
                    keys.arraySize++;
                    values.arraySize++;
                }
            }


        }
    }
}
