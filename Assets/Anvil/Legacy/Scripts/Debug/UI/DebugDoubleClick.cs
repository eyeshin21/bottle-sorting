#if DEBUG_MODE
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Anvil.Legacy
{
    public class DebugDoubleClick : MonoBehaviour, IPointerClickHandler
    {
        float _lastClickTime = -1;

        public Callback OnDoubleClick { get; set; }

        void Awake()
        {
            var canvas = gameObject.GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                var graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
                if (graphicRaycaster == null)
                {
                    canvas.AddComponent<GraphicRaycaster>();
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //if (eventData.clickCount == 2)
            //{
            //    Manager.HideDebug();
            //    OnDoubleClick?.Invoke();
            //}

            if (Helper.CheckDoubleTouch(ref _lastClickTime))
            {
                //Manager.HideDebug();
                OnDoubleClick?.Invoke();
            }
        }
    }
}
#endif