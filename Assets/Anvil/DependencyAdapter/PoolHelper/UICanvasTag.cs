using UnityEngine;

namespace Anvil
{
    public class UICanvasTag : MonoBehaviour
    {
        private static Canvas _canvasInstance;

        public static Canvas CanvasInstance
        {
            get
            {
                if (_canvasInstance == null)
                {
                    UICanvasTag taged = null;
                    taged = Object.FindFirstObjectByType<UICanvasTag>();
                    _canvasInstance = taged != null ? taged.GetComponent<Canvas>() : null;
                }
                return _canvasInstance;
            }
        } 
        
        private void Awake()
        {
           
        }
    }
}