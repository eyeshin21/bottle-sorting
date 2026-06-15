using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil
{
    public interface ISingletonScriptableObject
    {
        public void OnLoad();
#if UNITY_EDITOR
#endif
    }

    public class SingletonScriptableObject<T> : ScriptableObject
        , ISingletonScriptableObject
        where T : ScriptableObject, ISingletonScriptableObject
    {
        public static bool Active => _instance != null;
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var type = typeof(T);

                    // Try to load in "Resources"
                    _instance = Resources.Load<T>(type.Name);
                    if (_instance != null)
                    {
                        _instance.OnLoad();
                        return _instance;
                    }

#if UNITY_EDITOR
                    // Try to load in "Editor Default Resources/Resources"
                    var path = $"Assets/Editor Default Resources/Resources/{type.Name}.asset";
                    _instance = AssetDatabase.LoadAssetAtPath<T>(path);
                    if (_instance == null)
                    {
                        _instance = Resources.Load<T>($"EditorConfigs/{type.Name}");
                    }
#endif
                }

                return _instance;
            }
        }
        public virtual void OnLoad()
        {
        }
#if UNITY_EDITOR
      
#endif
    }
//
// #if UNITY_EDITOR
//     [CustomEditor(typeof(SingletonScriptableObject<>), true)]
//     public class SingletonScriptableObjectEditor : Editor
//     {
//         private Dictionary<string, GUIContent> _contents = new Dictionary<string, GUIContent>();
//
//         public override void OnInspectorGUI()
//         {
//             var iScriptableObject = target as ISingletonScriptableObject;
//             if (iScriptableObject != null)
//             {
//                 serializedObject.Update();
//
//                 iScriptableObject.OnInspectorGUI(serializedObject, _contents);
//
//                 serializedObject.ApplyModifiedProperties();
//                 
//                 // if (!iScriptableObject.OnInspectorGUI(serializedObject, _contents))
//                 // {
//                 //     base.OnInspectorGUI();
//                 // }
//             }
//             else
//             {
//                 EditorGUILayout.HelpBox("Scriptable object required!", MessageType.Warning);
//             }
//         }
//     }
// #endif
}