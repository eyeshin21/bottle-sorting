using Anvil.Legacy.Actions;
using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        #region Linear
        public static void MoveTo(this Component component, Vector3 endPos, float duration, Callback callback = null)
        {
            MoveTo(component?.gameObject, endPos, duration, EaseType.Linear, callback);
        }

        public static void MoveTo(this Component component, Vector3 endPos, float duration, EaseType easeType, Callback callback = null)
        {
            MoveTo(component?.gameObject, endPos, duration, easeType, callback);
        }

        public static void MoveTo(this Component component, GameObject target, float duration, EaseType easeType, Callback callback = null)
        {
            MoveTo(component?.gameObject, target?.transform, duration, easeType, callback);
        }

        public static void MoveTo(this Component component, Transform target, float duration, EaseType easeType, Callback callback = null)
        {
            MoveTo(component?.gameObject, target, duration, easeType, callback);
        }

        public static void MoveTo(this GameObject gameObject, Vector3 endPos, float duration, Callback callback = null)
        {
            MoveTo(gameObject, endPos, duration, EaseType.Linear, callback);
        }

        public static void MoveTo(this GameObject gameObject, Vector3 endPos, float duration, EaseType easeType, Callback callback = null)
        {
            if (gameObject != null)
            {
                var action = Sequence.Create(Move.CreateLerp(gameObject, endPos, duration, easeType), callback);
                gameObject.PlayAction(action);
            }
            else
            {
                LegacyLog.Warning($"Can't move to {endPos}: GameObject is null!");
                callback?.Invoke();
            }
        }

        public static void MoveTo(this GameObject gameObject, GameObject target, float duration, EaseType easeType, Callback callback = null)
        {
            MoveTo(gameObject, target?.transform, duration, easeType, callback);
        }

        public static void MoveTo(this GameObject gameObject, Transform target, float duration, EaseType easeType, Callback callback = null)
        {
            if (gameObject != null && target != null)
            {
                var move = Move.CreateLerp(gameObject, target.position, duration, easeType);
                var action = Sequence.Create(Spawn.Create(move, () => move.SetEnd(target.position)), () =>
                {
                    gameObject.transform.position = target.position;
                    callback?.Invoke();
                });
                gameObject.PlayAction(action);
            }
            else
            {
                LegacyLog.Warning($"Can't move to {target}: GameObject is null!");
                callback?.Invoke();
            }
        }

        public static void MoveToSpeed(this GameObject gameObject, Vector3 endPos, float speed, Callback callback = null)
        {
            MoveToSpeed(gameObject, endPos, speed, EaseType.Linear, callback);
        }

        public static void MoveToSpeed(this GameObject gameObject, Vector3 endPos, float speed, EaseType easeType, Callback callback = null)
        {
            if (gameObject != null)
            {
                float duration = Helper.GetDuration(gameObject.transform.position, endPos, speed);
                MoveTo(gameObject, endPos, duration, easeType, callback);
            }
            else
            {
                LegacyLog.Warning($"Can't move to {endPos}: GameObject is null!");
                callback?.Invoke();
            }
        }
        #endregion

        #region QuadBezier
        public static void MoveBezierTo(this GameObject gameObject, Vector3 controlPos, Vector3 endPos, float duration, Callback callback = null)
        {
            if (gameObject != null)
            {
                var action = Sequence.Create(Move.CreateQuadBezier(gameObject, controlPos, endPos, duration), callback);
                gameObject.PlayAction(action);
            }
            else
            {
                LegacyLog.Warning($"Can't move bezier to {endPos}: GameObject is null!");
                callback?.Invoke();
            }
        }

        public static void MoveBezierToSpeed(this GameObject gameObject, Vector3 controlPos, Vector3 endPos, float speed, Callback callback = null)
        {
            if (gameObject != null)
            {
                float duration = Helper.GetQuadBezierLength(gameObject.transform.position, controlPos, endPos, 0.05f) / speed;
                MoveBezierTo(gameObject, controlPos, endPos, duration, callback);
            }
            else
            {
                LegacyLog.Warning($"Can't move bezier to {endPos}: GameObject is null!");
                callback?.Invoke();
            }
        }

        /// <summary>
        /// Move + Rotate.
        /// </summary>
        public static void MoveBezierToSpeed(this GameObject gameObject, Vector3 controlPos, Vector3 endPos, float speed, float angleOffset, Callback callback = null)
        {
            if (gameObject != null)
            {
                float duration = Helper.GetQuadBezierLength(gameObject.transform.position, controlPos, endPos, 0.05f) / speed;
                var move = Move.CreateQuadBezier(gameObject, controlPos, endPos, duration);
                var moveRotate = SpawnRotate.Create(move, angleOffset);
                var action = Sequence.Create(moveRotate, callback);
                gameObject.PlayAction(action);
            }
            else
            {
                LegacyLog.Warning($"Can't move bezier to {endPos}: GameObject is null!");
                callback?.Invoke();
            }
        }
        #endregion
    }
}