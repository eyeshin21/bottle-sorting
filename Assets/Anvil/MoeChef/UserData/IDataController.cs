using System;

namespace Anvil
{
    public delegate string NameGetter(string key);

    public interface IBaseDataController
    {
        void ClearInitStatus();
    }

    public interface IDataController<T> : IBaseDataController
    {
        string Key { get; }
        T Value { get; set; }
#if DEBUG_MODE
        void OnGUI(Action<T> setHandler = null);
#endif
    }
}