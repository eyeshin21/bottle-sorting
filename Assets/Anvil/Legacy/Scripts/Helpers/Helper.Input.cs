#if UNITY_EDITOR || UNITY_STANDALONE
#define MOUSE_ENABLED
#endif
using UnityEngine;
using UnityEngine.EventSystems;

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        static bool IsEditorSimulator()
        {
#if UNITY_EDITOR
            //return !Application.isEditor;
            return UnityEngine.Device.SystemInfo.deviceType != DeviceType.Desktop;
#else
            return false;
#endif
        }

        public static bool HasInput()
        {
            return Input.anyKey || Input.touchCount > 0;
        }

        public static bool IsTouchPressed(bool checkPointerOverGameObject = false)
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (!checkPointerOverGameObject || !IsPointerOverGameObject(touch.fingerId))
                    {
                        return true;
                    }
                }
            }
#if MOUSE_ENABLED
            if (Input.GetMouseButtonDown(0))
            {
                if (!checkPointerOverGameObject || !IsPointerOverGameObject(-1))
                {
                    return true;
                }
            }
#endif
            return false;
        }

        public static bool IsHoldingTouch()
        {
            if (Input.touchCount > 0)
            {
                for (int i = Input.touchCount - 1; i >= 0; i--)
                {
                    var touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        return true;
                    }
                }
            }
#if MOUSE_ENABLED
            if (Input.GetMouseButton(0))
            {
                return true;
            }
#endif
            return false;
        }

        public static bool CheckDoubleTouch(ref float lastTime)
        {
            var time = Time.time;
            if (lastTime < 0)
            {
                lastTime = time;
            }
            else
            {
                float doubleTime = 0.5f;
                float resetTime = 5f;
                if (IsEditorSimulator())
                {
                    doubleTime += 2f;
                    resetTime += 2f;
                }
                float delta = time - lastTime;
                if (delta < doubleTime)
                {
                    lastTime = -1;
                    return true;
                }

                if (delta > resetTime)
                {
                    lastTime = time;
                }
                else
                {
                    lastTime = -1;
                }
            }

            return false;
        }

        static bool IsPointerOverGameObject(int pointerId)
        {
            var eventSystem = EventSystem.current;
            return eventSystem != null ? eventSystem.IsPointerOverGameObject(pointerId) : false;
        }
    }
}