using UnityEngine;

namespace Anvil
{
    public static class Context
    {
        private static Canvas _canvas;
        public static Camera MainCamera => CameraController.Camera;

        public static Canvas MainCanvas
        {
            get
            {
                if (_canvas == null)
                {
                    _canvas = CanvasUIUtility.CreateUICanvas();
                }

                return _canvas;
            }
        }
    }
}