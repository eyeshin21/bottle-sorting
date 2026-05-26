using UnityEngine;
using UnityEngine.EventSystems;

namespace Anvil.Legacy
{
    public class DebugPointHandler : MonoBehaviour
    {
        Vector3 _offset;

        void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            var mousePos = Input.mousePosition;
            mousePos.z = 0;
            _offset = transform.position - Camera.main.ScreenToWorldPoint(mousePos);
        }

        void OnMouseDrag()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            var mousePos = Input.mousePosition;
            mousePos.z = 0;
            transform.position = Camera.main.ScreenToWorldPoint(mousePos) + _offset;
        }

        void OnMouseUp()
        {
            //if (EventSystem.current.IsPointerOverGameObject())
            //{
            //    return;
            //}
        }
    }
}