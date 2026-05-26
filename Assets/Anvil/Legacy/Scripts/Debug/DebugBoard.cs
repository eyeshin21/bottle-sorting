#if DEBUG_MODE
using UnityEngine;

namespace Anvil.Legacy
{
    public class DebugBoard : MonoBehaviour
    {
        [SerializeField] int _rowCount = 8;
        [SerializeField] int _columnCount = 8;
        [SerializeField] float _cellSize = 1f;
        [SerializeField] Color _color = Color.green;

        void OnDrawGizmos()
        {
            GizmosHelper.DrawGrid(transform, _rowCount, _columnCount, _cellSize, _color);
        }
    }
}
#endif