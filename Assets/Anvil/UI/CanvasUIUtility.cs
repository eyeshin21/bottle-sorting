using MatchThree;
using UnityEngine;
using UnityEngine.UI;

namespace Anvil
{
    public static class CanvasUIUtility
    {
        public const string CanvasTag = "UICanvas";
        
        public const int DefaultScreenResolutionX = 1280;
        public const int DefaultScreenResolutionY = 2084;
        public const int DefaultCanvasDistance = 10;
        private static GameObject _inputBlocker;
        private static GameObject _canvasPoolParent;

        private static Canvas GetCanvas()
        {
            if (_canvasPoolParent == null)
            {
                _canvasPoolParent = new GameObject("CanvasPool");
                Object.DontDestroyOnLoad(_canvasPoolParent);
                return null;
            }

            foreach (Transform child in _canvasPoolParent.transform)
            {
                var canvas = child.GetComponent<Canvas>();
                if (canvas != null)
                {
                    return canvas;
                }
            }
            return null;
        }
        public static Canvas CreateUICanvas(string name = "Canvas", int sortingOrder = 0)
        {
            Canvas canvas = GetCanvas();
            if (canvas != null)
            {
                canvas.sortingOrder = sortingOrder;
                return  canvas;
            }
            
            GameObject canvasObject = new GameObject(name);
            canvas = canvasObject.AddComponent<Canvas>();
            canvasObject.AddComponent<GraphicRaycaster>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = CameraController.Camera;
            canvas.sortingLayerName = SortingLayerName.UI;
            canvas.sortingOrder = sortingOrder;
            canvas.planeDistance = DefaultCanvasDistance;
            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(DefaultScreenResolutionX, DefaultScreenResolutionY);
            scaler.scaleFactor = 0f;
            
            return canvas;
        }
        public static string ImagePoolAddress = "GeneratedImageObj";

        private static GameObject inputBlocker
        {
            get
            {
                if (_inputBlocker != null ) return _inputBlocker;
                var canvas = CreateUICanvas("InputBlockerCanvas", 30000);
                _inputBlocker = canvas.gameObject;
                var image = _inputBlocker.AddComponent<Image>();
                image.color = new Color(0, 0, 0, 0);
                return _inputBlocker;
            }
        }

        public static void BlockInput(bool block = true)
        {
            inputBlocker.SetActive(block);

            Debug.Log(block ? "blocked input" : "open input");
        }
    }
}