#region

using System;
using UnityEngine;

#endregion

namespace Anvil
{
    public static partial class RectTransformExtension
    {
        private static Vector3[] _worldCornersBuffer = new Vector3[4];
        private static RectTransform _cachedRectTransform;

        public static Rect GetWorldRect(this RectTransform rectTransform)
        {
            // This returns the world space positions of the corners in the order
            // [0] bottom left,
            // [1] top left
            // [2] top right
            // [3] bottom right
            if (!rectTransform)
            {
                return Rect.zero;
            }
            rectTransform.GetWorldCorners(_worldCornersBuffer);

            Vector2 min = _worldCornersBuffer[0];
            Vector2 max = _worldCornersBuffer[2];
            Vector2 size = max - min;

            return new Rect(min, size);
        }
        public static void ClearCache()
        {
            _cachedRectTransform = null;
        }
        public static Vector3 GetWorldCorner(this RectTransform rtf, Direction8 direction)
        {
            if (_cachedRectTransform == rtf)
            {
                return Return(direction);
            }

            _cachedRectTransform = rtf;
            rtf.GetWorldCorners(_worldCornersBuffer);
            return Return(direction);

            Vector3 Return(Direction8 dir)
            {
                switch (dir)
                {
                    case Direction8.Left:
                        return _worldCornersBuffer[0] + (_worldCornersBuffer[1] - _worldCornersBuffer[0]) * 0.5f;
                    case Direction8.UpLeft:
                        return _worldCornersBuffer[1];
                    case Direction8.Up:
                        return _worldCornersBuffer[1] + (_worldCornersBuffer[2] - _worldCornersBuffer[1]) * 0.5f;
                    case Direction8.UpRight:
                        return _worldCornersBuffer[2];
                    case Direction8.Right:
                        return _worldCornersBuffer[3] + (_worldCornersBuffer[2] - _worldCornersBuffer[3]) * 0.5f;
                    case Direction8.DownRight:
                        return _worldCornersBuffer[3];
                    case Direction8.Down:
                        return _worldCornersBuffer[0] + (_worldCornersBuffer[3] - _worldCornersBuffer[0]) * 0.5f;
                    case Direction8.DownLeft:
                        return _worldCornersBuffer[0];
                    default:
                        return Vector3.zero;
                }
            }
        }
    }
}