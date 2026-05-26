using UnityEngine;

namespace Anvil.Legacy
{
    public class TouchReceiverRect : ITouchReceiver
    {
        float _width;
        float _height;

        public void Construct(float width, float height)
        {
            _width = width;
            _height = height;
        }

        public void SetScale(float scale)
        {
            _width *= scale;
            _height *= scale;
        }

        public void GetSize(out float width, out float height)
        {
            width = _width;
            height = _height;
        }

        public bool Contains(Vector3 pos)
        {
            float left = -_width * 0.5f;
            if (pos.x < left || pos.x > left + _width) return false;
            float bottom = -_height * 0.5f;
            return pos.y >= bottom && pos.y <= bottom + _height;
        }

#if DEBUG_MODE
        public void DrawGizmos(Vector3 pos, Color color)
        {
            GizmosHelper.DrawRect(pos, _width, _height, color);
        }
#endif

        #region Pool
        public void ReturnToPool()
        {
            _pool.Return(this);
        }

        static Pool<TouchReceiverRect> _pool = new();
        public static TouchReceiverRect Create(float width, float height)
        {
            var rect = _pool.Get();
            rect.Construct(width, height);
            return rect;
        }
        #endregion
    }
}