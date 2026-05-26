using UnityEngine;

namespace Anvil.Legacy
{
    public static class Context
    {
        static Camera _mainCamera;
        //public static Camera MainCamera => _mainCamera ??= Camera.main;
        public static Camera MainCamera
        {
            get
            {
                if (_mainCamera == null)
                {
                    _mainCamera = Camera.main;
                }
                return _mainCamera;
            }
        }

        static Canvas _mainCanvas;
        public static Canvas MainCanvas
        {
            get
            {
                if (_mainCanvas == null)
                {
                    var gameObjects = Helper.RootGameObjects;
                    for (int i = gameObjects.Length - 1; i >= 0; i--)
                    {
                        var gameObject = gameObjects[i];
                        if (gameObject.activeSelf)
                        {
                            var canvas = gameObject.GetComponent<Canvas>();
                            if (canvas != null)
                            {
                                //#if UNITY_EDITOR || DEBUG_MODE
                                //                                if (canvas.name.Contains("Debug")) continue;
                                //#endif
                                if (_mainCanvas == null || canvas.sortingOrder > _mainCanvas.sortingOrder)
                                {
                                    _mainCanvas = canvas;
                                }
                            }
                        }
                    }

                    if (_mainCanvas == null)
                    {
                        _mainCanvas = GameObject.FindObjectOfType<Canvas>();
                    }
                }

                return _mainCanvas;
            }
            set
            {
                _mainCanvas = value;
            }
        }

        public static Camera TutorialCamera => MainCamera;
        public static Canvas TutorialCanvas => MainCanvas;

        public static void Clear()
        {
            _mainCamera = null;
        }
    }
}