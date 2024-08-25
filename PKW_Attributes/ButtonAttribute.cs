using System;
using UnityEngine;

namespace PKW_Attributes
{
    public enum ButtonEnableMode
    {
        // 항상
        Always,
        // 에디터 상에서만
        Editor,
        // Playmode 상에서만
        Playmode
    }

    // 하나의 매소드를 대상으로 동작
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
