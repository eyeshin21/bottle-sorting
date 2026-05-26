// using UnityEngine;
//
// namespace Gametamin
// {
//     [System.Serializable]
//     public class CubicBezierConfigExtended
//     {
//         [SerializeField] ControlAnchorType _controlAnchorType = ControlAnchorType.Constant;
//         [SerializeField] Vector2 _controlAnchor;
//         [SerializeField, ConditionalShow("_controlAnchorType", (int)ControlAnchorType.Random)]
//         Vector2 _controlAnchor2;
//         [SerializeField] ControlAnchorType _control2AnchorType = ControlAnchorType.Constant;
//         [SerializeField] Vector2 _control2Anchor;
//         [SerializeField, ConditionalShow("_control2AnchorType", (int)ControlAnchorType.Random)]
//         Vector2 _control2Anchor2;
//         [SerializeField] ControlOffsetType _controlOffsetType = ControlOffsetType.Constant;
//         [SerializeField] Vector2 _controlOffset;
//         [SerializeField, ConditionalShow("_controlOffsetType", (int)ControlOffsetType.Random)]
//         Vector2 _controlOffset2;
//         //[SerializeField] bool _random;
//
//         public void GetControlsPosition(Vector3 startPos, Vector3 endPos, out Vector3 controlPos1, out Vector3 controlPos2)
//         {
//             float deltaX = endPos.x - startPos.x;
//             float deltaY = endPos.y - startPos.y;
//             var controlAnchor = _controlAnchorType == ControlAnchorType.Constant ? _controlAnchor : Helper.GetRandom(_controlAnchor, _controlAnchor2);
//             var controlOffset = _controlOffsetType == ControlOffsetType.Constant ? _controlOffset : Helper.GetRandom(_controlOffset, _controlOffset2);
//             float controlX = startPos.x + controlAnchor.x * deltaX + controlOffset.x;
//             float controlY = startPos.y + controlAnchor.y * deltaY + controlOffset.y;
//             controlPos1 = new Vector3(controlX, controlY);
//
//             controlAnchor = _control2AnchorType == ControlAnchorType.Constant ? _control2Anchor : Helper.GetRandom(_control2Anchor, _control2Anchor2);
//             if (_controlOffsetType == ControlOffsetType.Random)
//             {
//                 controlOffset = Helper.GetRandom(_controlOffset, _controlOffset2);
//             }
//             controlX = startPos.x + controlAnchor.x * deltaX + controlOffset.x;
//             controlY = startPos.y + controlAnchor.y * deltaY + controlOffset.y;
//             controlPos2 = new Vector3(controlX, controlY);
//         }
//
// #if UNITY_EDITOR
//         public void DrawGizmos(Vector3 startPos, Vector3 endPos, Color? color = null)
//         {
//             float deltaX = endPos.x - startPos.x;
//             float deltaY = endPos.y - startPos.y;
//             var lineColor = Color.white;
//             var anchorColor = Color.green;
//             var controlColor = Color.yellow;
//             var anchorPos = new Vector3(startPos.x + deltaX * _controlAnchor.x, startPos.y + deltaY * _controlAnchor.y);
//             var anchor2Pos = new Vector3(startPos.x + deltaX * _control2Anchor.x, startPos.y + deltaY * _control2Anchor.y);
//             var controlPos = anchorPos.Add(_controlOffset);
//             var control2Pos = anchor2Pos.Add(_controlOffset);
//             GizmosHelper.DrawCubicBezier(startPos, controlPos, control2Pos, endPos, color, lineColor);
//             if (_controlOffsetType == ControlOffsetType.Random)
//             {
//                 var controlPos2 = anchorPos.Add(_controlOffset2);
//                 var control2Pos2 = anchor2Pos.Add(_controlOffset2);
//                 GizmosHelper.DrawCubicBezier(startPos, controlPos2, control2Pos2, endPos, color, lineColor);
//                 GizmosHelper.DrawLine(controlPos, controlPos2, controlColor);
//                 GizmosHelper.DrawLine(control2Pos, control2Pos2, controlColor);
//             }
//             GizmosHelper.DrawCross(anchorPos, anchorColor);
//             GizmosHelper.DrawCross(anchor2Pos, anchorColor);
//
//             if (_controlAnchorType == ControlAnchorType.Random)
//             {
//                 var anchorPos2 = new Vector3(startPos.x + deltaX * _controlAnchor2.x, startPos.y + deltaY * _controlAnchor2.y);
//                 var anchor2Pos2 = new Vector3(startPos.x + deltaX * _control2Anchor2.x, startPos.y + deltaY * _control2Anchor2.y);
//                 controlPos = anchorPos2.Add(_controlOffset);
//                 control2Pos = anchor2Pos2.Add(_controlOffset);
//                 GizmosHelper.DrawCubicBezier(startPos, controlPos, control2Pos, endPos, color, lineColor);
//                 if (_controlOffsetType == ControlOffsetType.Random)
//                 {
//                     var controlPos2 = anchorPos2.Add(_controlOffset2);
//                     var control2Pos2 = anchor2Pos.Add(_controlOffset2);
//                     GizmosHelper.DrawCubicBezier(startPos, controlPos2, control2Pos2, endPos, color, lineColor);
//                     GizmosHelper.DrawLine(controlPos, controlPos2, controlColor);
//                     GizmosHelper.DrawLine(control2Pos, control2Pos2, controlColor);
//                 }
//                 GizmosHelper.DrawCross(anchorPos2, anchorColor);
//                 GizmosHelper.DrawCross(anchor2Pos2, anchorColor);
//                 GizmosHelper.DrawLine(anchorPos, anchorPos2, anchorColor);
//                 GizmosHelper.DrawLine(anchor2Pos, anchor2Pos2, anchorColor);
//             }
//         }
// #endif
//     }
// }
