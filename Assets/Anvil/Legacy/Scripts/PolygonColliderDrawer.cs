using UnityEngine;

namespace Anvil.Legacy
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class PolygonColliderDrawer : MonoBehaviour
    {
        [SerializeField] Color _drawColor = Color.white;

        PolygonCollider2D _polygonCollider;

        void OnDrawGizmos()
        {
            if (_polygonCollider == null)
            {
                _polygonCollider = GetComponent<PolygonCollider2D>();
                if (_polygonCollider == null)
                {
                    return;
                }
            }

            GizmosHelper.DrawPolyline(transform.position, _polygonCollider.points, true, _drawColor);
        }
    }
}