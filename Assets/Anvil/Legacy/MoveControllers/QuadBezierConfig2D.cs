using UnityEngine;

namespace Anvil.Legacy
{
    public enum ControlAnchorType
    {
        Constant,
        Random,
    }

    public enum ControlOffsetType
    {
        Constant,
        Random,
    }

    [System.Serializable]
    public class QuadBezierConfig2D
    {
        [SerializeField] ControlAnchorType _controlAnchorType = ControlAnchorType.Constant;
        [SerializeField] Vector2 _controlAnchor;
        [SerializeField, ConditionalShow("_controlAnchorType", (int)ControlAnchorType.Random)]
        Vector2 _controlAnchor2;
        [SerializeField] ControlOffsetType _controlOffsetType = ControlOffsetType.Constant;
        [SerializeField] Vector2 _controlOffset;
        [SerializeField, ConditionalShow("_controlOffsetType", (int)ControlOffsetType.Random)]
        Vector2 _controlOffset2;
        [SerializeField] bool _random;

        
        public Vector3 GetControlPosition(Vector3 startPos, Vector3 endPos)
        {
            var controlAnchor = _controlAnchorType == ControlAnchorType.Constant ? _controlAnchor : Helper.GetRandom(_controlAnchor, _controlAnchor2);
            var controlOffset = _controlOffsetType == ControlOffsetType.Constant ? _controlOffset : Helper.GetRandom(_controlOffset, _controlOffset2);
            return Helper.GetQuadBezierControl(startPos, endPos, controlAnchor, controlOffset, _random);
        }

#if UNITY_EDITOR
        public void DrawGizmos(Vector3 startPos, Vector3 endPos, Color? color = null)
        {
            float deltaX = endPos.x - startPos.x;
            float deltaY = endPos.y - startPos.y;
            var lineColor = Color.white;
            var anchorColor = Color.green;
            var controlColor = Color.yellow;
            var anchorPos = new Vector3(startPos.x + deltaX * _controlAnchor.x, startPos.y + deltaY * _controlAnchor.y);
            var controlPos = anchorPos.Add(_controlOffset);
            GizmosHelper.DrawQuadBezier(startPos, controlPos, endPos, color, lineColor);
            if (_controlOffsetType == ControlOffsetType.Random)
            {
                var controlPos2 = anchorPos.Add(_controlOffset2);
                GizmosHelper.DrawQuadBezier(startPos, controlPos2, endPos, color, lineColor);
                GizmosHelper.DrawLine(controlPos, controlPos2, controlColor);
            }
            GizmosHelper.DrawCross(anchorPos, anchorColor);

            if (_controlAnchorType == ControlAnchorType.Random)
            {
                var anchorPos2 = new Vector3(startPos.x + deltaX * _controlAnchor2.x, startPos.y + deltaY * _controlAnchor2.y);
                controlPos = anchorPos2.Add(_controlOffset);
                GizmosHelper.DrawQuadBezier(startPos, controlPos, endPos, color, lineColor);
                if (_controlOffsetType == ControlOffsetType.Random)
                {
                    var controlPos2 = anchorPos2.Add(_controlOffset2);
                    GizmosHelper.DrawQuadBezier(startPos, controlPos2, endPos, color, lineColor);
                    GizmosHelper.DrawLine(controlPos, controlPos2, controlColor);
                }
                GizmosHelper.DrawCross(anchorPos2, anchorColor);
                GizmosHelper.DrawLine(anchorPos, anchorPos2, anchorColor);
            }
        }
#endif
        public QuadBezierConfig2D Clone()
        {
            QuadBezierConfig2D clone = new QuadBezierConfig2D();
            clone._controlAnchorType = _controlAnchorType;
            clone._controlAnchor = _controlAnchor;
            clone._controlAnchor2 = _controlAnchor2;
            clone._controlOffsetType = _controlOffsetType;
            clone._controlOffset = _controlOffset;
            clone._controlOffset2 = _controlOffset2;
            clone._random = _random;
            return clone;
        }
    }
}
