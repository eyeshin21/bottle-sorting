#if UNITY_EDITOR || UNITY_STANDALONE
#define MOUSE_ENABLED
#endif

using UnityEngine;
using UnityEngine.EventSystems;

namespace Anvil.Legacy
{
    public class TouchController
    {
        ITouchHandler _handler;
        bool _isInteractable = true;
        bool _isCheckPointerOverGameObject = true;

        bool _isControlTouch;
#if MOUSE_ENABLED
        Vector3 _mousePosition;
#endif

        public ITouchHandler Handler
        {
            get => _handler;
            set => _handler = value;
        }

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

        public void OnTouchPressed(Vector3 pos)
        {
            if (_isInteractable && !_isControlTouch && _handler != null)
            {
                _isControlTouch = _handler.OnTouchPressed(pos);
            }
        }

        public void OnApplicationPaused()
        {
            //if (_isInteractable && _isControlTouch && _handler != null)
            //{
            //    _handler.OnTouchReleased(_lastPos);
            //    _isControlTouch = false;
            //}
        }

        public void Update()
        {
            if (!_isInteractable || _handler == null) return;

            // Touch input
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (!_isCheckPointerOverGameObject || !IsPointerOverGameObject(touch.fingerId))
                    {
                        _isControlTouch = _handler.OnTouchPressed(ScreenToWorldPoint(touch.position));
                    }
                }
                else
                {
                    if (_isControlTouch)
                    {
                        if (touch.phase == TouchPhase.Moved)
                        {
                            if (!_handler.OnTouchMoved(ScreenToWorldPoint(touch.position)))
                            {
                                _isControlTouch = false;
                            }
                        }
                        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        {
                            _handler.OnTouchReleased(ScreenToWorldPoint(touch.position));
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
                        _isControlTouch = _handler.OnTouchPressed(ScreenToWorldPoint(_mousePosition));
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
                                if (!_handler.OnTouchMoved(ScreenToWorldPoint(mousePosition)))
                                {
                                    _isControlTouch = false;
                                }
                            }
                        }
                        else if (Input.GetMouseButtonUp(0))
                        {
                            _handler.OnTouchReleased(ScreenToWorldPoint(Input.mousePosition));
                            _isControlTouch = false;
                        }
                    }
                }
            }
#endif
        }

        bool IsPointerOverGameObject(int pointerId)
        {
            var eventSystem = EventSystem.current;
            return eventSystem != null ? eventSystem.IsPointerOverGameObject(pointerId) : false;
        }

        Vector3 ScreenToWorldPoint(Vector3 screenPosition)
        {
            var position = Context.MainCamera.ScreenToWorldPoint(screenPosition);
            position.z = 0;
            return position;
        }
    }
}