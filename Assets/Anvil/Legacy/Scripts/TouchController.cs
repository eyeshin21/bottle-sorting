#if UNITY_EDITOR || UNITY_STANDALONE
#define MOUSE_ENABLED
#endif

using UnityEngine;
using UnityEngine.EventSystems;

namespace Anvil
{
    public class TouchController
    {
        private enum TouchState
        {
            None,
            Pressed,
            Moved,
            Released
        }

        private ITouchHandler _handler;
        private bool _isInteractable = true;
        private bool _isCheckPointerOverGameObject = true;
        private TouchState _currentTouchState = TouchState.None;
        private Vector2 _touchLastPoint;

        private bool _isControlTouch;
#if MOUSE_ENABLED
        private Vector3 _mousePosition;
#endif

        public ITouchHandler Handler
        {
            get => _handler;
            set
            {
                _handler = value;
            }
        }

        public bool Interactable
        {
            get => _isInteractable;
            set
            {
                _isInteractable = value;
            }
        }

        // public void OnTouchPressed(Vector3 pos)
        // {
        //     if (_handler == null || !_isInteractable) return;
        //     _isControlTouch = _handler.OnTouchPressed(pos);
        // }

        public void Update()
        {
            if (_handler == null || !_isInteractable) return;
            
            // Touch input
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                _touchLastPoint = touch.position;
                if (touch.phase == TouchPhase.Began)
                {
                    _currentTouchState = TouchState.Pressed;
                    if (!_isCheckPointerOverGameObject || !IsPointerOverGameObject(touch.fingerId))
                    {
                        _isControlTouch = _handler.OnTouchPressed(touch.position);
                    }
                }
                else
                {
                    if (_isControlTouch)
                    {
                        if (touch.phase == TouchPhase.Moved)
                        {
                            _currentTouchState = TouchState.Moved;
                            if (!_handler.OnTouchMoved(touch.position))
                            {
                                _isControlTouch = false;
                            }

                            return;
                        }
                        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        {
                            _currentTouchState = TouchState.None;
                            _handler.OnTouchReleased(touch.position);
                            _isControlTouch = false;
                            return;
                        }
                    }
                }

            }
            // Mouse input
            else
            {

#if MOUSE_ENABLED
                if (Input.GetMouseButtonDown(0))
                {
                    if (!_isCheckPointerOverGameObject || !IsPointerOverGameObject(-1))
                    {
                        _mousePosition = Input.mousePosition;
                        _currentTouchState = TouchState.Pressed;
                        _touchLastPoint = _mousePosition;
                        _isControlTouch = _handler.OnTouchPressed(_mousePosition);
                    }
                }
                else
                {
                    if (_isControlTouch)
                    {
                        if (Input.GetMouseButton(0))
                        {
                            var mousePosition = Input.mousePosition;
                            if (!Mathf.Approximately(mousePosition.x, _mousePosition.x) ||
                                !Mathf.Approximately(mousePosition.y, _mousePosition.y))
                            {
                                _mousePosition = mousePosition;
                                _touchLastPoint = _mousePosition;
                                _currentTouchState = TouchState.Moved;
                                if (!_handler.OnTouchMoved(mousePosition))
                                {
                                    _isControlTouch = false;
                                }
                            }
                            return;
                        }
                        else if (Input.GetMouseButtonUp(0))
                        {
                            _handler.OnTouchReleased(Input.mousePosition);
                            _isControlTouch = false;
                            _currentTouchState = TouchState.None;
                            return;
                        }
                    }
                }


// #else
                // if (_currentTouchState != TouchState.None)
                // {
                //     Debug.Log("force release");
                //     // release
                //     _currentTouchState = TouchState.None;
                //     _handler.OnTouchReleased(ScreenToWorldPoint(_touchLastPoint));
                //     _isControlTouch = false;
                // }
#endif          
                if (_currentTouchState != TouchState.None && _isControlTouch)
                {
                    if (!Input.GetMouseButton(0) && Input.touchCount == 0)
                    {
                        _handler.OnTouchReleased(Input.mousePosition);
                        _isControlTouch = false;
                        _currentTouchState = TouchState.None;
                        return; 
                    }
                }
            }
        }

        bool IsPointerOverGameObject(int pointerId)
        {
            var eventSystem = EventSystem.current;
            return eventSystem != null ? eventSystem.IsPointerOverGameObject(pointerId) : false;
        }

        // Vector3 ScreenToWorldPoint(Vector3 screenPosition)
        // {
        //     // screenPosition.z = 1;
        //     var position = Context.MainCamera.ScreenToWorldPoint(screenPosition);
        //     Debug.Log($"{screenPosition} {position}");
        //     return position;
        // }
    }
}