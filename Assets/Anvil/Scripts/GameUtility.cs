using Anvil.Legacy;
using UnityEngine;

namespace Anvil
{
    public static class GameUtility
    {
        static Vector3[] _screenCorner = null; // 0: bottom left, 1: top left, 2: top right, 3: bottom right (clockwise from bottom left)
        public static Vector3[] ScreenCorner
        {
            get
            {
                if (_screenCorner != null)
                {
                    return _screenCorner;
                }
                _screenCorner = new Vector3[4];
                Camera mainCamera = Camera.main;
                if (mainCamera == null)
                {
                    return _screenCorner;
                }
                _screenCorner[0] = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
                _screenCorner[1] = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
                _screenCorner[2] = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
                _screenCorner[3] = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
                return _screenCorner;
            }
        }
        public static Vector3 GetScreenCorner(int index)
        {
            return ScreenCorner[index];
        }
        public static Vector3 GetScreenCorner(Direction8 direction)
        {
            switch (direction)
            {
                case Direction8.DownLeft:
                    return GetScreenCorner(0);
                case Direction8.UpLeft:
                    return GetScreenCorner(1);
                case Direction8.UpRight:
                    return GetScreenCorner(2);
                case Direction8.DownRight:
                    return GetScreenCorner(3);
                case Direction8.Up:
                    return (GetScreenCorner(1) + GetScreenCorner(2)) / 2;
                case Direction8.Down:
                    return (GetScreenCorner(0) + GetScreenCorner(3)) / 2;
                case Direction8.Left:
                    return (GetScreenCorner(0) + GetScreenCorner(1)) / 2;
                case Direction8.Right:
                    return (GetScreenCorner(2) + GetScreenCorner(3)) / 2;
                default:
                    return Vector3.zero;
            }
        }
        public static Vector3 ScreenToWorldPoint(Vector3 screenPoint)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                return Vector3.zero;
            }
            return mainCamera.ScreenToWorldPoint(screenPoint);
        }
        public static Vector3 CurrentMousePosition2D
        {
            get
            {
                Vector3 mousePos = ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 10; // default canvas plane
                return mousePos;
            }
        }

    }
}