using UnityEngine;

namespace Anvil.Legacy
{
    public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<T>(Helper.GetClassName<T>());
                }
                return _instance;
            }
        }

#if UNITY_EDITOR
        public static void Clear()
        {
            if (_instance != null)
            {
                //Log.Warning($"Clear {typeof(T)}");
                _instance = null;
            }
        }

        public void Save()
        {
            EditorHelper.Save(this);
        }
#endif
    }
}