using Anvil;
using UnityEngine;

namespace Anvil
{
    public enum AnchorCalculationType
    {
        WorldSpaceRatio,
        LocalSpaceRatio,
        WorldSpaceOffset,
        OffsetYRatioX,
    }
    [System.Serializable]
    public class Bezier4Parameter : ITrajectoryParameter<Bezier4TrajectoryCalculator>
    {

        public Bezier4TrajectoryCalculator CreateTrajectoryCalculator()
        {
            return new Bezier4TrajectoryCalculator(this);
        }

        [SerializeField] AnchorCalculationType anchorType = AnchorCalculationType.WorldSpaceRatio;

        [SerializeField] ControlAnchorType _controlAnchorType = ControlAnchorType.Constant;
        [SerializeField] Vector2 _controlAnchor;
        [SerializeField, ConditionalShow("_controlAnchorType", (int)ControlAnchorType.Random)]
        Vector2 _controlAnchor2;
        [SerializeField] ControlAnchorType _control2AnchorType = ControlAnchorType.Constant;
        [SerializeField] Vector2 _control2Anchor;
        [SerializeField, ConditionalShow("_control2AnchorType", (int)ControlAnchorType.Random)]
        Vector2 _control2Anchor2;
        [SerializeField] ControlOffsetType _controlOffsetType = ControlOffsetType.Constant;
        [SerializeField] Vector2 _controlOffset;
        [SerializeField, ConditionalShow("_controlOffsetType", (int)ControlOffsetType.Random)]
        Vector2 _controlOffset2;
        //[SerializeField] bool _random;

        public void GetControlsPosition(Vector3 startPos, Vector3 endPos, out Vector3 controlPos1, out Vector3 controlPos2)
        {
            switch (anchorType)
            {
                case AnchorCalculationType.LocalSpaceRatio:
                    GetControlsPositionWithLocalNormal(startPos, endPos, out controlPos1, out controlPos2);
                    return;
                case AnchorCalculationType.WorldSpaceRatio:
                    GetControlsPositionFromWorldSpaceDistance(startPos, endPos, out controlPos1, out controlPos2);
                    return;
                case AnchorCalculationType.WorldSpaceOffset:
                    GetControlsPositionFromWorldSpaceOffsetUnit(startPos, endPos, out controlPos1, out controlPos2);
                    return;
                case AnchorCalculationType.OffsetYRatioX:
                    GetOffsetYRatioXControllPosition(startPos, endPos, out controlPos1, out controlPos2);
                    return;
                default:
                    controlPos1 = Vector3.zero;
                    controlPos2 = Vector3.zero;
                    Debug.LogError($"[bezier4 parameter] anchor of type {anchorType} is not supported");
                    break;
            }
        }

        private void GetOffsetYRatioXControllPosition(Vector3 startPos,Vector3 endPos,out Vector3 controlPos1,out Vector3 controlPos2)
        {
            controlPos1 = Vector3.zero;
            controlPos2 = Vector3.zero;
            
            float deltaX = endPos.x - startPos.x;
            Vector3 controlAnchor = _controlAnchorType == ControlAnchorType.Constant ? _controlAnchor : Utility.GetRandom(_controlAnchor, _controlAnchor2);
            Vector3 controlOffset = _controlOffsetType == ControlOffsetType.Constant ? _controlOffset : Utility.GetRandom(_controlOffset, _controlOffset2);
            float controlX = startPos.x + controlAnchor.x * deltaX + controlOffset.x;
            float controlY = startPos.y + controlAnchor.y + controlOffset.y;
            controlPos1 = new Vector3(controlX, controlY);

            controlAnchor = _control2AnchorType == ControlAnchorType.Constant ? _control2Anchor : Utility.GetRandom(_control2Anchor, _control2Anchor2);
            if (_controlOffsetType == ControlOffsetType.Random)
            {
                controlOffset = Utility.GetRandom(_controlOffset, _controlOffset2);
            }
            controlX = endPos.x + controlAnchor.x * deltaX + controlOffset.x;
            controlY = endPos.y + controlAnchor.y + controlOffset.y;
            controlPos2 = new Vector3(controlX, controlY);
        }

        private void GetControlsPositionFromWorldSpaceOffsetUnit(Vector3 startPos,Vector3 endPos,out Vector3 controlPos1,out Vector3 controlPos2)
        {
            
            Vector3 controlAnchor = _controlAnchorType == ControlAnchorType.Constant ? _controlAnchor : Utility.GetRandom(_controlAnchor, _controlAnchor2);
            Vector3 controlOffset = _controlOffsetType == ControlOffsetType.Constant ? _controlOffset : Utility.GetRandom(_controlOffset, _controlOffset2);
            
            controlPos1 = startPos + controlAnchor + controlOffset;

            controlAnchor = _control2AnchorType == ControlAnchorType.Constant ? _control2Anchor : Utility.GetRandom(_control2Anchor, _control2Anchor2);
            if (_controlOffsetType == ControlOffsetType.Random)
            {
                controlOffset = Utility.GetRandom(_controlOffset, _controlOffset2);
            }
            
            controlPos2 = endPos + controlAnchor + controlOffset;
        }

        private void GetControlsPositionFromWorldSpaceDistance(Vector3 startPos,Vector3 endPos,out Vector3 controlPos1,out Vector3 controlPos2)
        {
            float deltaX = endPos.x - startPos.x;
            float deltaY = endPos.y - startPos.y;
            var controlAnchor = _controlAnchorType == ControlAnchorType.Constant ? _controlAnchor : Utility.GetRandom(_controlAnchor, _controlAnchor2);
            var controlOffset = _controlOffsetType == ControlOffsetType.Constant ? _controlOffset : Utility.GetRandom(_controlOffset, _controlOffset2);
            float controlX = startPos.x + controlAnchor.x * deltaX + controlOffset.x;
            float controlY = startPos.y + controlAnchor.y * deltaY + controlOffset.y;
            controlPos1 = new Vector3(controlX, controlY);

            controlAnchor = _control2AnchorType == ControlAnchorType.Constant ? _control2Anchor : Utility.GetRandom(_control2Anchor, _control2Anchor2);
            if (_controlOffsetType == ControlOffsetType.Random)
            {
                controlOffset = Utility.GetRandom(_controlOffset, _controlOffset2);
            }
            controlX = startPos.x + controlAnchor.x * deltaX + controlOffset.x;
            controlY = startPos.y + controlAnchor.y * deltaY + controlOffset.y;
            controlPos2 = new Vector3(controlX, controlY);
        }

        private void GetControlsPositionWithLocalNormal(Vector3 startPos,Vector3 endPos,out Vector3 controlPos1,out Vector3 controlPos2)
        {
            Vector3 dir = endPos - startPos;
            float length = dir.magnitude;
            Vector3 dirNormalized = dir.normalized;
            Vector3 normal = Vector3.Cross(Vector3.forward, dir).normalized;
            if (normal.y < 0)
            {
                normal = -normal;
            }
            controlPos1 = startPos + dirNormalized * (_controlAnchor.x * length)  //X
                + normal * (_controlAnchor.y * length);                           //Y
            controlPos2 = startPos + dirNormalized * (_control2Anchor.x * length)  //X
                + normal * (_control2Anchor.y * length);                         //Y

        }

        private static class Utility
        {
            public static Vector2 GetRandom(Vector2 a,Vector2 b)
            {
                return new Vector2(
                    Random.Range(a.x, b.x),
                    Random.Range(a.y, b.y)
                );
            }
            public static Vector3 Add(Vector3 v,Vector2 offset)
            {
                return new Vector3(v.x + offset.x, v.y + offset.y, v.z);
            }
        }
        
#if UNITY_EDITOR
        public void DebugSet(Vector2 controlAnchor1, Vector2 controlAnchor2, AnchorCalculationType anchorCalculationType)
        {
            anchorType = anchorCalculationType;
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

            if (anchorType == AnchorCalculationType.LocalSpaceRatio)
            {
                GetControlsPositionWithLocalNormal(startPos, endPos, out Vector3 c1, out Vector3 c2);
                GizmosHelper.DrawCubicBezier(startPos, c1, c2, endPos, color, lineColor);
                GizmosHelper.DrawCross(c1, anchorColor);
                GizmosHelper.DrawCross(c2, anchorColor);
                return;
            }
            if (anchorType == AnchorCalculationType.OffsetYRatioX)
            {
                GetOffsetYRatioXControllPosition(startPos, endPos, out Vector3 c1, out Vector3 c2);
                GizmosHelper.DrawCubicBezier(startPos, c1, c2, endPos, color, lineColor);
                GizmosHelper.DrawCross(c1, anchorColor);
                GizmosHelper.DrawCross(c2, anchorColor);
                return;
            }

            float deltaX = endPos.x - startPos.x;
            float deltaY = endPos.y - startPos.y;


            var anchorPos = new Vector3(startPos.x + deltaX * _controlAnchor.x, startPos.y + deltaY * _controlAnchor.y);
            var anchor2Pos = new Vector3(startPos.x + deltaX * _control2Anchor.x, startPos.y + deltaY * _control2Anchor.y);
            if (anchorType == AnchorCalculationType.WorldSpaceOffset)
            {
                anchorPos = Utility.Add(startPos, _controlAnchor);
                anchor2Pos = Utility.Add(endPos, _control2Anchor);
            }
            
            var controlPos = Utility.Add(anchorPos,_controlOffset);
            var control2Pos = Utility.Add(anchor2Pos,_controlOffset);
            GizmosHelper.DrawCubicBezier(startPos, controlPos, control2Pos, endPos, color, lineColor);
            if (_controlOffsetType == ControlOffsetType.Random)
            {
                var controlPos2 = Utility.Add(anchorPos ,_controlOffset2);
                var control2Pos2 = Utility.Add(anchor2Pos,_controlOffset2);
                GizmosHelper.DrawCubicBezier(startPos, controlPos2, control2Pos2, endPos, color, lineColor);
                GizmosHelper.DrawLine(controlPos, controlPos2, controlColor);
                GizmosHelper.DrawLine(control2Pos, control2Pos2, controlColor);
            }
            GizmosHelper.DrawCross(anchorPos, anchorColor);
            GizmosHelper.DrawCross(anchor2Pos, anchorColor);

            if (_controlAnchorType == ControlAnchorType.Random)
            {
                var anchorPos2 = new Vector3(startPos.x + deltaX * _controlAnchor2.x, startPos.y + deltaY * _controlAnchor2.y);
                var anchor2Pos2 = new Vector3(startPos.x + deltaX * _control2Anchor2.x, startPos.y + deltaY * _control2Anchor2.y);
                controlPos = Utility.Add(anchorPos2, _controlOffset);
                control2Pos = Utility.Add(anchor2Pos2, _controlOffset);
                GizmosHelper.DrawCubicBezier(startPos, controlPos, control2Pos, endPos, color, lineColor);
                if (_controlOffsetType == ControlOffsetType.Random)
                {
                    var controlPos2 = Utility.Add(anchorPos2, _controlOffset2);
                    var control2Pos2 = Utility.Add(anchor2Pos, _controlOffset2);
                    GizmosHelper.DrawCubicBezier(startPos, controlPos2, control2Pos2, endPos, color, lineColor);
                    GizmosHelper.DrawLine(controlPos, controlPos2, controlColor);
                    GizmosHelper.DrawLine(control2Pos, control2Pos2, controlColor);
                }
                GizmosHelper.DrawCross(anchorPos2, anchorColor);
                GizmosHelper.DrawCross(anchor2Pos2, anchorColor);
                GizmosHelper.DrawLine(anchorPos, anchorPos2, anchorColor);
                GizmosHelper.DrawLine(anchor2Pos, anchor2Pos2, anchorColor);
            }
        }
#endif
        public Bezier4Parameter Clone()
        {
            Bezier4Parameter clone = new Bezier4Parameter();
            clone._controlAnchorType = _controlAnchorType;
            clone._controlAnchor = _controlAnchor;
            clone._controlAnchor2 = _controlAnchor2;
            clone._control2AnchorType = _control2AnchorType;
            clone._control2Anchor = _control2Anchor;
            clone._control2Anchor2 = _control2Anchor2;
            clone._controlOffsetType = _controlOffsetType;
            clone._controlOffset = _controlOffset;
            clone._controlOffset2 = _controlOffset2;
            return clone;
        }

    }
}
