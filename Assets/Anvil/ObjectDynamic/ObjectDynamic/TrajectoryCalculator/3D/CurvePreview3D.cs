using UnityEngine;
using UnityEngine.Serialization;

namespace Anvil
{
#if UNITY_EDITOR

    [ExecuteInEditMode]
    public class CurvePreview3D : MonoBehaviour
    {
        [SerializeField] private Bezier4Parameter3D parameter = new Bezier4Parameter3D();
        [SerializeField] private Vector3 _startPos;
        [SerializeField] private Vector3 _endPos;
        [SerializeField] private Transform _startTransform;
        [SerializeField] private Transform _endTransform;
        [SerializeField] private Transform _control1;
        [SerializeField] private Transform _control2;
        [SerializeField] private AnchorCalculationMode anchorMode = AnchorCalculationMode.WorldspaceOffset;
        [SerializeField] private bool enablePreview = true;
        [SerializeField] private bool enablePreviewHandle = true;

        private void Update()
        {
            if (!enablePreview)
            {
                return;
            }

            if (_startTransform)
            {
                _startPos = _startTransform.position;
            }

            if (_endTransform)
            {
                _endPos = _endTransform.position;
            }

            if (enablePreviewHandle)
            {
                if (anchorMode == AnchorCalculationMode.WorldspaceOffset)
                {
                    Vector3 controlPos1 = Vector3.zero;
                    Vector3 controlPos2 = Vector3.zero;
                    Vector3 dir = _endPos - _startPos;
                    Vector3 dirNorm = dir.normalized;
                    float length = dir.magnitude;
                    Vector3 up = Vector3.up;
                    Vector3 right = Vector3.Cross(dir, up);

                    if (_control1)
                    {
                        Vector3 controlDir = _control1.position - _startPos;
                        float controlAngle = Vector3.Angle(dir, controlDir);


                        Vector3 posC1 = _control1.position;
                        Vector3 projectedZ = Vector3.Project(_control1.position - _startPos, dirNorm);
                        float anglez = Vector3.Angle(dir, projectedZ);
                        float z = (anglez < 90 ? 1 : -1) * projectedZ.magnitude / length;
                        Vector3 projectedX = Vector3.Project(controlDir, right);
                        float x = projectedX.magnitude * (Vector3.Dot(projectedX, right) > 0 ? 1 : -1); ;
                        Vector3 projectedY = Vector3.Project(controlDir, up);
                        float y = projectedY.magnitude * (Vector3.Dot(projectedY, up) > 0 ? 1 : -1); ;
                        controlPos1 = new Vector3(x, y, z);
                    }
                    if (_control2)
                    {
                        Vector3 controlDir = _control2.position - _startPos;
                        float controlAngle = Vector3.Angle(dir, controlDir);
                        
                        Vector3 posC2 = _control2.position;
                        Vector3 projectedZ = Vector3.Project(_control2.position - _startPos, dirNorm);
                        float anglez = Vector3.Angle(dir, projectedZ);
                        float z = (anglez < 90 ? 1 : -1) * projectedZ.magnitude / length;
                        Vector3 projectedX = Vector3.Project(controlDir, right);
                        float x = projectedX.magnitude * (Vector3.Dot(projectedX, right) > 0 ? 1 : -1); ;
                        Vector3 projectedY = Vector3.Project(controlDir, up);
                        float y = projectedY.magnitude * (Vector3.Dot(projectedY, up) > 0 ? 1 : -1); ;

                        controlPos2 = new Vector3(x, y, z);
                    }
                    parameter.DebugSet(controlPos1,controlPos2,anchorMode);
                }
                else
                {
                    Debug.LogError("Not supported");
                }
            }
        }

        private void OnDrawGizmos()
        {
            parameter.DrawGizmos(_startPos,_endPos);
        }
    }
#endif

    //
    // [CustomEditor(typeof(CurvePreview))]
    // public class DrawCurveHandle : Editor
    // {
    //     void OnSceneGUI()
    //     {
    //         Handles.color = Color.red;
    //     }
    // }
}
