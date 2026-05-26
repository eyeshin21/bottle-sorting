#if UNITY_EDITOR || DEBUG_MODE
using UnityEngine;

namespace Anvil.Legacy
{
    [RequireComponent(typeof(Canvas))]
    [ExecuteInEditMode]
    public class DebugCanvas : MonoBehaviour
    {
        [SerializeField] bool _drawRectTransforms = true;
        [SerializeField] bool _drawGizmosSelected = true;
        [SerializeField] bool _drawIncludeInactive = true;

        void OnDrawGizmos()
        {
            if (!_drawGizmosSelected)
            {
                DrawGizmos();
            }
        }

        void OnDrawGizmosSelected()
        {
            if (_drawGizmosSelected)
            {
                DrawGizmos();
            }
        }

        void DrawGizmos()
        {
            if (_drawRectTransforms)
            {
                if (_drawIncludeInactive || transform.gameObject.activeSelf)
                {
                    int childCount = transform.childCount;
                    if (childCount == 1)
                    {
                        var child = transform.GetChild(0);
                        if (_drawIncludeInactive || child.gameObject.activeSelf)
                        {
                            var safeArea = child.GetComponent<SafeArea>();
                            if (safeArea != null)
                            {
                                for (int i = 0; i < child.childCount; i++)
                                {
                                    GizmosHelper.DrawRectTransforms(child.GetChild(i), _drawIncludeInactive);
                                }
                            }
                            else
                            {
                                GizmosHelper.DrawRectTransforms(child, _drawIncludeInactive);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < childCount; i++)
                        {
                            GizmosHelper.DrawRectTransforms(transform.GetChild(i), _drawIncludeInactive);
                        }
                    }
                }
            }
        }
    }
}
#endif