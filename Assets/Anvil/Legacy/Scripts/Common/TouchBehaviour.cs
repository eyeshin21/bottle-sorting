#if UNITY_EDITOR || UNITY_STANDALONE
#define MOUSE_ENABLED
#endif
using UnityEngine;
using UnityEngine.EventSystems;

namespace Anvil.Legacy
{
    public class TouchBehaviour : MonoBehaviour
    {
        protected bool _isInteractable = true;
        protected bool _isCheckPointerOverGameObject = true;
        protected bool _isControlTouch;
#if MOUSE_ENABLED
        protected Vector3 _mousePosition;
#endif

        public bool Interactable
        {
            get => _isInteractable;
            set => _isInteractable = value;
        }

        public bool CheckPointerOverGameObject
        {
            get => _isCheckPointerOverGameObject;
            set => _isCheckPointerOverGameObject = value;
        }

        /// <summary>
        /// Returns true to control touch.
        /// </summary>
        protected virtual bool OnTouchPressed(Vector3 pos)
        {
            return false;
        }

        protected virtual bool OnTouchMoved(Vector3 pos)
        {
            return false;
        }

        protected virtual void OnTouchReleased(Vector3 pos)
        {

        }

        protected virtual void Update()
        {
            if (!_isInteractable) return;

            // Touch input
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (!_isCheckPointerOverGameObject || !IsPointerOverGameObject(touch.fingerId))
                    {
                        _isControlTouch = OnTouchPressed(ScreenToWorldPoint(touch.position));
                    }
                }
                else
                {
                    if (_isControlTouch)
                    {
                        if (touch.phase == TouchPhase.Moved)
                        {
                            if (!OnTouchMoved(ScreenToWorldPoint(touch.position)))
                            {
                                _isControlTouch = false;
                            }
                        }
                        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        {
                            OnTouchReleased(ScreenToWorldPoint(touch.position));
                            _isControlTouch = false;
                        }
                    }
                }
            }
#if MOUSE_ENABLED
            // Mouse input
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!_isCheckPointerOverGameObject || !IsPointerOverGameObject(-1))
                    {
                        _mousePosition = Input.mousePosition;
                        _isControlTouch = OnTouchPressed(ScreenToWorldPoint(_mousePosition));
                    }
                }
                else
                {
                    if (_isControlTouch)
                    {
                        if (Input.GetMouseButton(0))
                        {
                            var mousePosition = Input.mousePosition;
                            if (!Mathf.Approximately(mousePosition.x, _mousePosition.x) || !Mathf.Approximately(mousePosition.y, _mousePosition.y))
                            {
                                _mousePosition = mousePosition;
                                if (!OnTouchMoved(ScreenToWorldPoint(mousePosition)))
                                {
                                    _isControlTouch = false;
                                }
                            }
                        }
                        else if (Input.GetMouseButtonUp(0))
                        {
                            OnTouchReleased(ScreenToWorldPoint(Input.mousePosition));
                            _isControlTouch = false;
                        }
                    }
                }
            }
#endif
        }

        protected static bool IsPointerOverGameObject(int pointerId)
        {
            var eventSystem = EventSystem.current;
            return eventSystem != null ? eventSystem.IsPointerOverGameObject(pointerId) : false;
        }

        protected static Vector3 ScreenToWorldPoint(Vector3 screenPosition)
        {
            var position = Context.MainCamera.ScreenToWorldPoint(screenPosition);
            position.z = 0;
            return position;
        }
    }
}