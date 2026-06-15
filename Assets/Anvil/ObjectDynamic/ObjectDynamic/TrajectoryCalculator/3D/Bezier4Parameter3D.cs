using Anvil;
using UnityEngine;

namespace Anvil
{
    public enum AnchorCalculationMode
    {
        WorldspaceOffset,
    }
    [System.Serializable]
    public class Bezier4Parameter3D : ITrajectoryParameter<Bezier4TrajectoryCalculator3D>
    {

        public Bezier4TrajectoryCalculator3D CreateTrajectoryCalculator()
        {
            return new Bezier4TrajectoryCalculator3D(this);
        }

        [SerializeField] AnchorCalculationMode anchorType = AnchorCalculationMode.WorldspaceOffset;

        [SerializeField] ControlAnchorType _controlAnchorType = ControlAnchorType.Constant;
        [SerializeField] Vector3 _controlAnchor;
        [SerializeField, ConditionalShow("_controlAnchorType", (int)ControlAnchorType.Random)]
        Vector3 _controlAnchor2;
        [SerializeField] ControlAnchorType _control2AnchorType = ControlAnchorType.Constant;
        [SerializeField] Vector3 _control2Anchor;
        [SerializeField, ConditionalShow("_control2AnchorType", (int)ControlAnchorType.Random)]
        Vector3 _control2Anchor2;
        [SerializeField] ControlOffsetType _controlOffsetType = ControlOffsetType.Constant;
        [SerializeField] Vector3 _controlOffset;
        [SerializeField, ConditionalShow("_controlOffsetType", (int)ControlOffsetType.Random)]
        Vector3 _controlOffset2;
        //[SerializeField] bool _random;

        public void GetControlsPosition(Vector3 startPos, Vector3 endPos, out Vector3 controlPos1, out Vector3 controlPos2)
        {
            switch (anchorType)
            {
                case AnchorCalculationMode.WorldspaceOffset:
                    GetMode1ControlsPosition(startPos, endPos, out controlPos1, out controlPos2);
                    return;
                default:
                    controlPos1 = startPos;
                    controlPos2 = endPos;
                    Debug.LogError($"[bezier4 parameter] anchor of type {anchorType} is not supported");
                    break;
            }
        }

        private void GetMode1ControlsPosition(Vector3 startPos,Vector3 endPos,out Vector3 controlPos1,out Vector3 controlPos2)
        {
            Vector3 dir = endPos - startPos;
            float length = dir.magnitude;
            if (length <= Mathf.Epsilon)
            {
                controlPos1 = startPos;
                controlPos2 = startPos;
                return;
            }

            Vector3 dirNormalized = dir / length;
            Vector3 up = Vector3.up; // keep the normal up so the curve adapts to height (y) differences
            Vector3 right = Vector3.Cross(dirNormalized, up);
            if (right.sqrMagnitude <= 1e-6f)
            {
                right = Vector3.Cross(dirNormalized, Vector3.forward);
            }
            right.Normalize();

            Vector3 controlAnchor1 = _controlAnchorType == ControlAnchorType.Constant
                ? _controlAnchor
                : Utility.GetRandom(_controlAnchor, _controlAnchor2);
            Vector3 controlAnchor2 = _control2AnchorType == ControlAnchorType.Constant
                ? _control2Anchor
                : Utility.GetRandom(_control2Anchor, _control2Anchor2);

            Vector3 controlOffset1 = _controlOffsetType == ControlOffsetType.Constant
                ? _controlOffset
                : Utility.GetRandom(_controlOffset, _controlOffset2);
            Vector3 controlOffset2 = controlOffset1;
            if (_controlOffsetType == ControlOffsetType.Random)
            {
                controlOffset2 = Utility.GetRandom(_controlOffset, _controlOffset2);
            }

            Vector3 offset1 = right * controlOffset1.x + up * controlOffset1.y + dirNormalized * controlOffset1.z;
            Vector3 offset2 = right * controlOffset2.x + up * controlOffset2.y + dirNormalized * controlOffset2.z;

            controlPos1 = startPos
                          + dirNormalized * (controlAnchor1.z * length) // forward (ratio)
                          + up * controlAnchor1.y                      // up (world units)
                          + right * controlAnchor1.x                   // right (world units)
                          + offset1;
            controlPos2 = startPos
                          + dirNormalized * (controlAnchor2.z * length)
                          + up * controlAnchor2.y
                          + right * controlAnchor2.x
                          + offset2;

        }
#if UNITY_EDITOR
        public void DebugSet(Vector3 controlAnchor1, Vector3 controlAnchor2, AnchorCalculationMode anchorCalculationMode)
        {
            anchorType = anchorCalculationMode;
            _controlAnchor = controlAnchor1;
            _control2Anchor = controlAnchor2;
        }

        [SerializeField] bool _debugEnabled = false;
        public void DrawGizmos(Vector3 startPos, Vector3 endPos, Color? color = null)
        {
            if (!_debugEnabled)
            {
                return;
            }
            
            var lineColor = Color.white;
            var anchorColor = Color.green;
            var controlColor = Color.yellow;

            if (anchorType == AnchorCalculationMode.WorldspaceOffset)
            {
                GetMode1ControlsPosition(startPos, endPos, out Vector3 c1, out Vector3 c2);
                GizmosHelper.DrawCubicBezier(startPos, c1, c2, endPos, color, lineColor);
                GizmosHelper.DrawCross(c1, anchorColor);
                GizmosHelper.DrawCross(c2, anchorColor);
                return;
            }
        }
#endif
        public Bezier4Parameter3D Clone()
        {
            Bezier4Parameter3D clone = new Bezier4Parameter3D
            {
                _controlAnchorType = _controlAnchorType,
                _controlAnchor = _controlAnchor,
                _controlAnchor2 = _controlAnchor2,
                _control2AnchorType = _control2AnchorType,
                _control2Anchor = _control2Anchor,
                _control2Anchor2 = _control2Anchor2,
                _controlOffsetType = _controlOffsetType,
                _controlOffset = _controlOffset,
                _controlOffset2 = _controlOffset2
            };
            return clone;
        }

        private static class Utility
        {
            public static Vector3 GetRandom(Vector3 a, Vector3 b)
            {
                return new Vector3(
                    Random.Range(a.x, b.x),
                    Random.Range(a.y, b.y),
                    Random.Range(a.z, b.z)
                );
            }
        }
    }
}
