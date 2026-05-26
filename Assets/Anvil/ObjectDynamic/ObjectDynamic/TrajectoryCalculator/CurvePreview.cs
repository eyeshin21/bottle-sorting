using UnityEngine;
using UnityEngine.Serialization;

namespace Anvil
{
#if UNITY_EDITOR

    [ExecuteInEditMode]
    public class CurvePreview : MonoBehaviour
    {
        [FormerlySerializedAs("_cubicBezierConfig")] [SerializeField] private Bezier4Parameter bezier4Parameter = new Bezier4Parameter();
        [SerializeField] private Vector3 _startPos;
        [SerializeField] private Vector3 _endPos;
        [SerializeField] private Transform _startTransform;
        [SerializeField] private Transform _endTransform;
        [SerializeField] private Transform _control1;
        [SerializeField] private Transform _control2;
        [SerializeField] private AnchorCalculationType anchorType = AnchorCalculationType.WorldSpaceRatio;
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
                if (anchorType == AnchorCalculationType.WorldSpaceRatio)
                {
                    Vector2 controlPos1 = Vector2.zero;
                    Vector2 controlPos2 = Vector2.zero;
                    if (_control1 != null)
                    {
                        Vector3 posC1 = _control1.position;
                        controlPos1 = new Vector2((posC1.x - _startPos.x) / (_endPos.x - _startPos.x),(posC1.y - _startPos.y) / (_endPos.y - _startPos.y));
                    }

                    if (_control2 != null)
                    {
                        Vector3 posC2 = _control2.position;
                        controlPos2 = new Vector2((posC2.x - _startPos.x) / (_endPos.x - _startPos.x),(posC2.y - _startPos.y) / (_endPos.y - _startPos.y));
                    }
                    bezier4Parameter.DebugSet(controlPos1,controlPos2,anchorType);

                }
                else if (anchorType == AnchorCalculationType.WorldSpaceOffset)
                {
                    Vector2 controlPos1 = Vector2.zero;
                    Vector2 controlPos2 = Vector2.zero;
                    if (_control1 != null)
                    {
                        Vector3 posC1 = _control1.position;
                        controlPos1 = posC1 - _startPos;
                    }

                    if (_control2 != null)
                    {
                        Vector3 posC2 = _control2.position;
                        controlPos2 = posC2 - _endPos;
                    }
                    bezier4Parameter.DebugSet(controlPos1,controlPos2,anchorType);
                }
                else if (anchorType == AnchorCalculationType.LocalSpaceRatio)
                {
                    Vector2 anchor1 = Vector2.zero;
                    Vector2 anchor2 = Vector2.zero;
                    Vector3 dir = _endPos - _startPos;
                    float len = dir.magnitude;
                    if (_control1 != null)
                    {
                        Vector3 posC1 = _control1.position;
                        Vector3 projectedX = Vector3.Project((posC1 - _startPos),dir);
                        Vector3 projectedPoint = _startPos + projectedX;
                        Vector3 anchorYVector = posC1 - projectedPoint;
                        float yVectorLen = anchorYVector.y >= 0 ? anchorYVector.magnitude : -anchorYVector.magnitude;
                        float xVectorLen = projectedX.x >= 0 ? projectedX.magnitude : -projectedX.magnitude;
                        anchor1 = new Vector2(xVectorLen / len,yVectorLen / len);
                    }
                    if (_control2 != null)
                    {
                        Vector3 posC2 = _control2.position;
                        Vector3 projectedX = Vector3.Project((posC2 - _startPos),dir);
                        Vector3 projectedPoint = _startPos + projectedX;
                        Vector3 anchorYVector = posC2 - projectedPoint;
                        float yVectorLen = anchorYVector.y >= 0 ? anchorYVector.magnitude : -anchorYVector.magnitude;
                        float xVectorLen = projectedX.x >= 0 ? projectedX.magnitude : -projectedX.magnitude;
                        anchor2 = new Vector2(xVectorLen / len,yVectorLen / len);
                    }
                    bezier4Parameter.DebugSet(anchor1,anchor2,anchorType);
                }
                else if (anchorType == AnchorCalculationType.OffsetYRatioX)
                {
                    Vector2 anchor1 = Vector2.zero;
                    Vector2 anchor2 = Vector2.zero;
                    float xVectorLen = _endPos.x - _startPos.x;
                    if (_control1 != null)
                    {
                        Vector3 posC1 = _control1.position;
                        float offSetX = posC1.x - _startPos.x;
                        anchor1 = new Vector2(offSetX / xVectorLen, posC1.y - _startPos.y);
                    }
                    if (_control2 != null)
                    {
                        Vector3 posC2 = _control2.position;
                        float offSetX = posC2.x - _endPos.x;
                        anchor2 = new Vector2(offSetX / xVectorLen, posC2.y - _endPos.y);
                    }
                    bezier4Parameter.DebugSet(anchor1,anchor2,anchorType);
                }
            }
        }

        private void OnDrawGizmos()
        {
            bezier4Parameter.DrawGizmos(_startPos,_endPos);
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
