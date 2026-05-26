using UnityEngine;
using System.Diagnostics;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public static partial class EditorHelper
    {
#if UNITY_EDITOR
        static readonly Vector2 PositionLabelOffset = new Vector2(-0.1f, -0.3f);

        static GUIStyle _positionLabelStyle;
        static GUIStyle PositionLabelStyle => _positionLabelStyle ??= GUIHelper.CreateLabelStyle().SetFontSize(18).SetTextColor(Color.yellow).SetBold();

        public static T GetWindow<T>() where T : EditorWindow
        {
            return EditorWindow.GetWindow<T>();
        }

        public static T GetWindowWithRect<T>(Rect rect) where T : EditorWindow
        {
            return EditorWindow.GetWindowWithRect<T>(rect);
        }

        public static void AddDelayCall(Callback callback)
        {
            EditorApplication.delayCall += () => callback?.Invoke();
        }

        public static T AddCollider<T>(GameObject gameObject) where T : Collider2D
        {
            var collider = gameObject.GetComponent<Collider2D>();
            if (collider == null)
            {
                T ret = gameObject.AddComponent<T>();
                SetDirty(gameObject);
                return ret;
            }

            LegacyLog.Warning($"Existed collider {collider.GetType()}");
            return null;
        }

        public static void ConnectGameObjectToPrefab(ref GameObject gameObject, GameObject prefab)
        {
#pragma warning disable 0618
            gameObject = PrefabUtility.ConnectGameObjectToPrefab(gameObject, prefab);
#pragma warning restore 0618
        }

        public static void FocusSceneView()
        {
            EditorWindow.FocusWindowIfItsOpen<SceneView>();
        }

        public static void FocusSceneView(Tool tool)
        {
            EditorWindow.FocusWindowIfItsOpen<SceneView>();
            Tools.current = tool;
        }

        public static void FocusSceneViewAndSetActiveGameObject(GameObject gameObject, Callback callback = null)
        {
            EditorWindow.FocusWindowIfItsOpen<SceneView>();
            EditorApplication.delayCall += () =>
            {
                Selection.activeGameObject = gameObject;
                callback?.Invoke();
            };
        }

        public static void RepaintSceneView()
        {
            EditorWindow.FocusWindowIfItsOpen<SceneView>();
        }

        public static void FocusHierachyAndSetActiveGameObject(GameObject gameObject)
        {
            EditorApplication.ExecuteMenuItem("Window/General/Hierarchy");
            Selection.activeGameObject = gameObject;
        }

        public static bool PositionHandle(Transform transform, string label, ref Vector3 localPos, bool usePositionHandle = false)
        {
            var worldPos = transform.position;
            var pos = worldPos;
            pos.x += localPos.x;
            pos.y += localPos.y;
            Handles.Label(pos.Add(PositionLabelOffset), label, PositionLabelStyle);
            EditorGUI.BeginChangeCheck();
            var newPos = (usePositionHandle ? Handles.PositionHandle(pos, Quaternion.identity) : Handles.FreeMoveHandle(pos/*APIUpdater: , Quaternion.identity*/, 0.15f, Vector3.zero, Handles.CircleHandleCap)) - worldPos;
            if (EditorGUI.EndChangeCheck())
            {
                localPos.x = newPos.x;
                localPos.y = newPos.y;
                return true;
            }

            return false;
        }
#endif

        [Conditional("UNITY_EDITOR")]
        public static void Set<T>(T obj, string undoName, Callback callback) where T : Object
        {
#if UNITY_EDITOR
            Undo.RecordObject(obj, undoName);
            callback();
            EditorUtility.SetDirty(obj);
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void SetDirty(UnityEngine.Object obj)
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(obj);
#endif
        }

        [Conditional("UNITY_EDITOR")]
        public static void Save(UnityEngine.Object obj)
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(obj);
            AssetDatabase.SaveAssetIfDirty(obj);
#endif
        }
    }
}