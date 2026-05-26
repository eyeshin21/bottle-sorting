using UnityEngine;
using System;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public static partial class EditorHelper
    {
#if UNITY_EDITOR
        static object[] _parametersExpand = new object[] { null, true };
        static object[] _parametersCollapse = new object[] { null, false };

        static Type _sceneHierarchyWindowType = null;
        static Type SceneHierarchyWindowType
        {
            get
            {
                if (_sceneHierarchyWindowType == null)
                {
                    var assembly = typeof(EditorWindow).Assembly;
                    _sceneHierarchyWindowType = assembly.GetType("UnityEditor.SceneHierarchyWindow");
                }
                return _sceneHierarchyWindowType;
            }
        }

        static MethodInfo _setExpandedRecursive = null;
        static MethodInfo SetExpandedRecursiveImpl => _setExpandedRecursive ??= SceneHierarchyWindowType.GetMethod("SetExpandedRecursive");

        public static void Expand(GameObject gameObject)
        {
            SetExpandedRecursive(gameObject, true);
            var transform = gameObject.transform;
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                SetExpandedRecursive(transform.GetChild(i).gameObject, false);
            }
        }

        public static void ExpandRecursive(GameObject gameObject)
        {
            SetExpandedRecursive(gameObject, true);
        }

        public static void SetExpandedRecursive(GameObject gameObject, bool expand)
        {
            SetExpandedRecursive(gameObject.GetInstanceID(), expand);
        }

        public static void SetExpandedRecursive(int instanceID, bool expand)
        {
            var hierachyWindow = EditorWindow.GetWindow(SceneHierarchyWindowType);
            if (expand)
            {
                _parametersExpand[0] = instanceID;
                SetExpandedRecursiveImpl?.Invoke(hierachyWindow, _parametersExpand);
            }
            else
            {
                _parametersCollapse[0] = instanceID;
                SetExpandedRecursiveImpl?.Invoke(hierachyWindow, _parametersCollapse);
            }
        }

        //[MenuItem("CONTEXT/GameObject/Expand GameObjects")]
        //[MenuItem("GameObject/Expand GameObjects", priority = 40)]
        //private static void ExpandGameObjects()
        //{
        //    SetExpandedRecursive(Selection.activeGameObject.GetInstanceID(), true);
        //}

        //[MenuItem("CONTEXT/GameObject/Collapse GameObjects")]
        //[MenuItem("GameObject/Collapse GameObjects", priority = 40)]
        //private static void CollapseGameObjects()
        //{
        //    SetExpandedRecursive(Selection.activeGameObject.GetInstanceID(), false);
        //}

        //[MenuItem("GameObject/Expand GameObjects", validate = true)]
        //[MenuItem("GameObject/Collapse GameObjects", validate = true)]
        //private static bool CanExpandOrCollapse()
        //{
        //    return Selection.activeGameObject != null;
        //}
#endif
        public static void CheckSetActiveObject(UnityEngine.Object activeObject)
        {
#if UNITY_EDITOR
            if (activeObject != null)
            {
                Selection.activeObject = activeObject;
            }
#endif
        }
    }
}