// using UnityEngine;
// using Gametamin;
// using System;
//
// namespace MergeGame
// {
//     public class CubicBezierMoveControllerExtended : LerpMoveController
//     {
//         CubicBezierConfig _config;
//         Vector3 _controlPos;
//         Vector3 _control2Pos;
//
//         public CubicBezierConfig Config
//         {
//             get => _config;
//             set => _config = value;
//         }
//
//         public Vector3 ControlPos
//         {
//             get => _controlPos;
//             set => _controlPos = value;
//         }
//
//         public Vector3 Control2Pos
//         {
//             get => _control2Pos;
//             set => _control2Pos = value;
//         }
//
//         public override void MoveTo(Vector3 endPos, Action callback = null)
//         {
//             Vector3 controlPos;
//             Vector3 control2Pos;
//             if (_config != null)
//             {
//                 _config.GetControlsPosition(Current, endPos, out controlPos, out control2Pos);
//             }
//             else
//             {
//                 controlPos = _controlPos;
//                 control2Pos = _control2Pos;
//             }
//             float duration = _duration;
//             if (_speed > 0)
//             {
//                 //TODO
//                 duration = (endPos - Current).magnitude / Mathf.Max(_speed, Mathf.Epsilon);
//             }
//             MoveTo(endPos, controlPos, control2Pos, duration, callback);
//         }
//
//         public void MoveTo(Vector3 endPos, Vector3 controlPos, Vector3 control2Pos, float duration, Action callback = null)
//         {
//             base.MoveTo(endPos, callback);
//             _controlPos = controlPos;
//             _control2Pos = control2Pos;
//             _duration = duration;
//             _startPos = Current;
//             _time = 0;
//         }
//
//         public override void Update(float deltaTime)
//         {
//             if (!_isFinished)
//             {
//                 _time += deltaTime;
//                 if (_time < _duration)
//                 {
//                     float t = _time / _duration;
//                     float u = 1 - t;
//                     float a = u * u * u;
//                     float b = 3 * u * u * t;
//                     float c = 3 * u * t * t;
//                     float d = t * t * t;
//
//                     float x = a * _startPos.x + b * _controlPos.x + c * _control2Pos.x + d * _endPos.x;
//                     float y = a * _startPos.y + b * _controlPos.y + c * _control2Pos.y + d * _endPos.y;
//                     Current = new Vector3(x, y, 0);
//                 }
//                 else
//                 {
//                     Current = _endPos;
//                     OnFinish();
//                 }
//             }
//         }
//
//         public override void UpdateConfig(MoveConfig moveConfig)
//         {
//             base.UpdateConfig(moveConfig);
//             Config = moveConfig.CubicBezierConfig;
//         }
//
//         static Pool<CubicBezierMoveController> _pool;
//         static Pool<CubicBezierMoveController> Pool
//         {
//             get
//             {
//                 if (_pool == null)
//                 {
//                     _pool = new Pool<CubicBezierMoveController>();
//                 }
//                 return _pool;
//             }
//         }
//
//         public static CubicBezierMoveController Create()
//         {
//             return Pool.Get();
//         }
//
//         public override void ReturnPool()
//         {
//             base.ReturnPool();
//             Pool.Return(this);
//         }
//     }
// }
