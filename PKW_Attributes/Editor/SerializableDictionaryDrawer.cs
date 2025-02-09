using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEngine;

namespace PKW_Attributes
{
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
    public class SerializableDictionaryDrawer : PropertyDrawer
    {           
        // 한글 주석 깨지나?
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            var keys = property.FindPropertyRelative("keys");
            var values = property.FindPropertyRelative("values");

            int count = keys.arraySize;

            Rect foldoutRect = new Rect(position.x, position.y, position.width, position.height); 
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            
            Rect countRect = new Rect(position.x + position.width - 48, position.y, 48, position.height);
            using (new EditorGUI.DisabledScope(true))  
            {
                EditorGUI.IntField(countRect, count);  
            }

            if (property.isExpanded)
            {
                // 탭 키
                EditorGUI.indentLevel++;
                for (int i = 0; i < count; i++)
                {
                    using(new EditorGUILayout.HorizontalScope())
                    {
                        var type = keys.GetArrayElementAtIndex(i).type;

                        //키 값은 수정할 수 없도록 만든다
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUILayout.PropertyField(keys.GetArrayElementAtIndex(i), GUIContent.none);
                        EditorGUI.EndDisabledGroup();
                        EditorGUILayout.PropertyField(values.GetArrayElementAtIndex(i), GUIContent.none);
                    }
                }
            }
            
            EditorGUI.EndProperty();

        }
    }
}
