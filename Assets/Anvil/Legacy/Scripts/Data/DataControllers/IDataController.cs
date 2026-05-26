using UnityEngine;

namespace Anvil.Legacy
{
    public interface IBaseDataController
    {
        string Key { get; }
        void CopyLocal(string key);
        void Clear();
#if DEBUG_MODE
        IDebugGUIController DebugGUIController { get; }
#endif
    }

    public interface IDataController<T> : IBaseDataController
    {
        T Value { get; set; }
#if DEBUG_MODE
        IDataController<T> SetDebugSetHandler(Handler<T> setHandler);
        IDataController<T> SetDebugGUIController(IDebugGUIController guiController);
#endif
    }

    public interface IArrayDataController<T> : IBaseDataController
    {
        T[] Values { get; }
        T GetValue(int index);
        void SetValue(int index, T value);
        void SetLength(int length);
        void SetValues(T[] values);
    }

#if DEBUG_MODE
    public class DebugDataController
    {
        protected string _name;
        public virtual IDebugGUIController DebugGUIController => new DebugTodoGUIController(_name);

        static GUIStyle _keyLabelStyle;
        static GUIStyle KeyLabelStyle => _keyLabelStyle ??= GUIHelper.CreateLabelStyle(Color.green);

        static GUIStyle _keyToggleStyle;
        static GUIStyle KeyToggleStyle => _keyToggleStyle ??= GUIHelper.CreateToggleStyle(Color.green);

        protected static void Label(string label)
        {
            GUILayout.Label(label, KeyLabelStyle);
        }

        protected static bool Button(string text)
        {
            return GUILayout.Button(text);
        }

        protected static bool Toggle(string label, bool value)
        {
            return GUILayout.Toggle(value, label, KeyToggleStyle);
        }
    }
#endif
}