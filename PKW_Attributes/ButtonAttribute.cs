using System;
using UnityEngine;

namespace PKW_Attributes
{
    public enum ButtonEnableMode
    {
        // �׻�
        Always,
        // ������ �󿡼���
        Editor,
        // Playmode �󿡼���
        Playmode
    }

    // �ϳ��� �żҵ带 ������� ����
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string ButtonName { get; }
        public System.Object[] Parameter { get; private set; }
        public ButtonEnableMode ButtonEnableMode { get; private set; }

        public ButtonAttribute(string buttonName = "", ButtonEnableMode ButtonEnableMode = ButtonEnableMode.Always)
        {
            this.ButtonName = buttonName;
            this.ButtonEnableMode = ButtonEnableMode;
        }

        public ButtonAttribute(string _buttonName = "", params System.Object[] _parameter)
        {
            this.ButtonName = _buttonName;
            this.Parameter = _parameter;
        }
    }
}
