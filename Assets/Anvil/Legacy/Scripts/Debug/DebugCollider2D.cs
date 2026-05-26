#if DEBUG_MODE
using UnityEngine;

namespace Anvil.Legacy
{
    public class DebugCollider2D : MonoBehaviour
    {
        [SerializeField] Color _lineColor = Color.green;
        [SerializeField] bool _drawPoints;

        Collider2D[] _colliders;

        void Reset()
        {
            _colliders = null;
        }

        void OnDrawGizmos()
        {
            if (_colliders == null)
            {
                _colliders = GetComponentsInChildren<Collider2D>();
            }

            int count = _colliders.GetLength();
            for (int i = 0; i < count; i++)
            {
                GizmosHelper.DrawCollider(_colliders[i], _lineColor, _drawPoints);
            }
        }
    }
}
#endif