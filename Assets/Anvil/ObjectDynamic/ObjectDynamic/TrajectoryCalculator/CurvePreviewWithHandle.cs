using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;

namespace Anvil
{
    [ExecuteInEditMode]
    public class CurvePreviewWithHandle : MonoBehaviour
    {
        [FormerlySerializedAs("quadBezierParameter")]
        [FormerlySerializedAs("_cubicBezierConfig")]
        [SerializeField] private Bezier4Parameter bezier4Parameter;

        [SerializeField] private bool enablePreview = true;
        [SerializeField] private bool previewWithoutControls = false;

        [SerializeField] private AnchorCalculationType anchorType =
            AnchorCalculationType.WorldSpaceRatio;

        [SerializeField] private Vector3 _startPos;
        [SerializeField] private Vector3 _endPos;

        [SerializeField, HideInInspector] private Vector3 _c1;
        [SerializeField, HideInInspector] private Vector3 _c2;

        private void OnDrawGizmos()
        {
            if (!enablePreview || bezier4Parameter == null)
                return;

            bezier4Parameter.DrawGizmos(_startPos, _endPos);
        }

        private void OnValidate()
        {
            SyncControlsFromParam();
        }

        private void SyncControlsFromParam()
        {
            if (bezier4Parameter == null)
                return;

            bezier4Parameter.GetControlsPosition(
                _startPos,
                _endPos,
                out _c1,
                out _c2
            );
        }

        private void ApplyControlsToParam()
        {
            Vector2 a1 = Vector2.zero;
            Vector2 a2 = Vector2.zero;

            switch (anchorType)
            {
                case AnchorCalculationType.WorldSpaceRatio:
                {
                    float dx = _endPos.x - _startPos.x;
                    float dy = _endPos.y - _startPos.y;

                    a1 = new Vector2(
                        dx != 0 ? (_c1.x - _startPos.x) / dx : 0,
                        dy != 0 ? (_c1.y - _startPos.y) / dy : 0
                    );

                    a2 = new Vector2(
                        dx != 0 ? (_c2.x - _startPos.x) / dx : 0,
                        dy != 0 ? (_c2.y - _startPos.y) / dy : 0
                    );
                    break;
                }

                case AnchorCalculationType.WorldSpaceOffset:
                {
                    a1 = _c1 - _startPos;
                    a2 = _c2 - _endPos;
                    break;
                }

                case AnchorCalculationType.LocalSpaceRatio:
                {
                    Vector3 dir = _endPos - _startPos;
                    float len = dir.magnitude;
                    if (len <= Mathf.Epsilon)
                        break;

                    Vector3 dirN = dir.normalized;
                    Vector3 normal = Vector3.Cross(Vector3.forward, dirN);
                    if (normal.y < 0) normal = -normal;

                    Vector3 v1 = _c1 - _startPos;
                    Vector3 v2 = _c2 - _startPos;

                    a1 = new Vector2(
                        Vector3.Dot(v1, dirN) / len,
                        Vector3.Dot(v1, normal) / len
                    );

                    a2 = new Vector2(
                        Vector3.Dot(v2, dirN) / len,
                        Vector3.Dot(v2, normal) / len
                    );
                    break;
                }

                case AnchorCalculationType.OffsetYRatioX:
                {
                    float dx = _endPos.x - _startPos.x;

                    a1 = new Vector2(
                        dx != 0 ? (_c1.x - _startPos.x) / dx : 0,
                        _c1.y - _startPos.y
                    );

                    a2 = new Vector2(
                        dx != 0 ? (_c2.x - _endPos.x) / dx : 0,
                        _c2.y - _endPos.y
                    );
                    break;
                }
            }

            bezier4Parameter.DebugSet(a1, a2, anchorType);
        }

        [CustomEditor(typeof(CurvePreviewWithHandle))]
        private class CurvePreviewEditor : Editor
        {
            private const float HANDLE_SIZE = 0.08f;

            private void OnSceneGUI()
            {
                var p = (CurvePreviewWithHandle)target;
                if (!p.enablePreview || p.bezier4Parameter == null)
                    return;

                if (!p.previewWithoutControls)
                    p.SyncControlsFromParam();

                float size(Vector3 pos) =>
                    HandleUtility.GetHandleSize(pos) * HANDLE_SIZE;

                EditorGUI.BeginChangeCheck();

                Handles.color = Color.green;
                Vector3 start = Handles.FreeMoveHandle(
                    p._startPos,
                    size(p._startPos),
                    Vector3.zero,
                    Handles.DotHandleCap
                );

                Handles.color = Color.red;
                Vector3 end = Handles.FreeMoveHandle(
                    p._endPos,
                    size(p._endPos),
                    Vector3.zero,
                    Handles.DotHandleCap
                );

                Vector3 c1 = p._c1;
                Vector3 c2 = p._c2;

                if (!p.previewWithoutControls)
                {
                    Handles.color = Color.yellow;
                    c1 = Handles.FreeMoveHandle(
                        p._c1,
                        size(p._c1),
                        Vector3.zero,
                        Handles.DotHandleCap
                    );

                    Handles.color = Color.cyan;
                    c2 = Handles.FreeMoveHandle(
                        p._c2,
                        size(p._c2),
                        Vector3.zero,
                        Handles.DotHandleCap
                    );
                }

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(p, "Edit Curve Preview");

                    p._startPos = start;
                    p._endPos = end;
                    p._c1 = c1;
                    p._c2 = c2;

                    if (!p.previewWithoutControls)
                        p.ApplyControlsToParam();

                    EditorUtility.SetDirty(p);
                }

                Handles.color = Color.gray;
                Handles.DrawLine(p._startPos, p._c1);
                Handles.DrawLine(p._endPos, p._c2);
            }
        }
    }
    
}
#endif

