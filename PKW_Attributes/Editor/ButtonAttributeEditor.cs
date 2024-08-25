using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace PKW_Attributes
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ButtonAttributeEditor : Editor
    {
        private MonoBehaviour monoBehaviour;
        private void OnEnable()
        {
            monoBehaviour = target as MonoBehaviour;
        }
        public override void OnInspectorGUI()
        {
            // ���� �����ϴ� Inspector�� �׸���
            base.OnInspectorGUI();
            // ��� �żҵ带 Ȯ���� ����
            MethodInfo[] methods = monoBehaviour.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (MethodInfo method in methods)
            {
                ButtonAttribute buttonAttribute = Attribute.GetCustomAttribute(method, typeof(ButtonAttribute)) as ButtonAttribute;

                if (buttonAttribute != null)
                {
                    // ��ư �̸� ����
                    string buttonLabel = string.IsNullOrEmpty(buttonAttribute.ButtonName) ? method.Name : buttonAttribute.ButtonName;

                    if (GUILayout.Button(buttonLabel))
                    {
                        method.Invoke(monoBehaviour, buttonAttribute.Parameter);
                    }
                }
            }
        }
    }
}
