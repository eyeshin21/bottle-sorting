#if DEBUG_MODE
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public class DebugBehaviour : MonoBehaviour
    {
        public virtual void OnSceneGUI()
        {

        }

        public virtual void OnInspectorGUI()
        {

        }

        protected virtual void RepaintGizmos()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                // Ensure continuous Update calls
                EditorApplication.QueuePlayerLoopUpdate();
                SceneView.RepaintAll();
            }
#endif
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DebugBehaviour), true), CanEditMultipleObjects, DisallowMultipleComponent]
    public class DebugBehaviourEditor : Editor
    {
        void OnSceneGUI()
        {
            if (!Application.isPlaying)
            {
                Handles.BeginGUI();
                {
                    (target as DebugBehaviour).OnSceneGUI();
                }
                Handles.EndGUI();
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            (target as DebugBehaviour).OnInspectorGUI();
        }
    }
#endif
}
#endif