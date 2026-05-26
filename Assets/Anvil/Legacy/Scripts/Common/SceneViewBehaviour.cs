#if UNITY_EDITOR || UNITY_STANDALONE
#define MOUSE_ENABLED
#endif
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    [ExecuteInEditMode]
    public class SceneViewBehaviour : MonoBehaviour
    {
        protected bool _isHoldControl;

        protected virtual bool AutoRepaint => true;

        protected virtual void Awake()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorHelper.FocusSceneViewAndSetActiveGameObject(gameObject, Set2DMode);
            }
#endif
        }

#if UNITY_EDITOR
        protected void SetToolView()
        {
            Tools.current = Tool.View;
        }

        protected void Set2DMode()
        {
            var sceneView = SceneView.currentDrawingSceneView;
            if (sceneView == null)
            {
                sceneView = SceneView.sceneViews[0] as SceneView;
            }

            if (sceneView != null)
            {
                sceneView.in2DMode = true;
            }
        }
#endif

        protected virtual void OnEnable()
        {
#if UNITY_EDITOR
            Selection.selectionChanged += OnSelectionChanged;
#endif
        }

        protected virtual void OnDisable()
        {
#if UNITY_EDITOR
            Selection.selectionChanged -= OnSelectionChanged;
#endif
        }

#if UNITY_EDITOR
        protected virtual void OnFocus()
        {
            EditorHelper.FocusSceneView();
            Set2DMode();
        }

        protected virtual void OnSelectionChanged()
        {
            if (Application.isPlaying) return;

            if (Selection.activeGameObject == gameObject)
            {
                OnFocus();
            }
        }
#endif

        protected virtual void Update()
        {
            UpdateTouch();
        }

        #region Touch
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

        protected virtual void UpdateTouch()
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
                            _mousePosition = Input.mousePosition;
                            OnTouchReleased(ScreenToWorldPoint(_mousePosition));
                            _isControlTouch = false;
                        }
                    }
                }
            }
#endif
        }

        protected bool IsPointerOverGameObject(int pointerId)
        {
            var eventSystem = EventSystem.current;
            return eventSystem != null ? eventSystem.IsPointerOverGameObject(pointerId) : false;
        }

        protected Vector3 ScreenToWorldPoint(Vector3 screenPosition)
        {
            var position = Context.MainCamera.ScreenToWorldPoint(screenPosition);
            position.z = 0;
            return position;
        }

        public virtual bool OnTouchPressed(Vector3 pos)
        {
            return false;
        }

        public virtual bool OnTouchMoved(Vector3 pos)
        {
            return false;
        }

        public virtual void OnTouchReleased(Vector3 pos)
        {

        }
        #endregion

#if UNITY_EDITOR
        #region Scene GUI
        public virtual void OnSceneGUI()
        {
            OnGUICallback();
        }

        public virtual void OnSceneGUI(Event evt)
        {
            var eventType = evt.type;
            bool repaint = false;
            bool use = false;

            // Key
            if (eventType == EventType.KeyDown)
            {
                var keyCode = evt.keyCode;
                if (keyCode != KeyCode.None)
                {
                    _isHoldControl = evt.control;
                    if (OnKeyPressed(keyCode))
                    {
                        repaint = true;
                        use = true;
                    }
                }
            }
            // Mouse
            else if (eventType == EventType.MouseDown)
            {
                // Left mouse
                if (evt.button == 0 || evt.button == 1)
                {
                    if (Tools.current == Tool.View)
                    {
                        _isControlTouch = OnTouchPressed(MouseToWorldPoint(evt));
                        repaint = true;
                        use = true;
                    }
                }
            }
            else
            {
                if (_isControlTouch && (evt.button == 0 || evt.button == 1))
                {
                    if (eventType == EventType.MouseDrag)
                    {
                        _isControlTouch = OnTouchMoved(MouseToWorldPoint(evt));
                        repaint = true;
                        use = true;
                    }
                    else if (eventType == EventType.MouseUp)
                    {
                        _isControlTouch = false;
                        OnTouchReleased(MouseToWorldPoint(evt));
                        repaint = true;
                        use = true;
                    }
                }
            }

            if (repaint)
            {
                Repaint();
            }

            if (use)
            {
                evt.Use();
            }
        }

        public virtual bool OnKeyPressed(KeyCode keyCode)
        {
            return false;
        }

        protected Vector3 MouseToWorldPoint(Event evt)
        {
            return HandleUtility.GUIPointToWorldRay(evt.mousePosition).origin;
        }

        protected void Repaint()
        {
            SceneView.RepaintAll();
        }
        #endregion
#endif

        protected virtual void OnGUI()
        {
            if (!Application.isPlaying) return;

            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            {
                OnGUICallback();
            }
            GUILayout.EndArea();
        }

        protected virtual void OnGUICallback()
        {

        }

#if UNITY_EDITOR
        public virtual void OnInspectorGUI(SerializedObject serializedObject)
        {
            serializedObject.OnInspectorGUI(property =>
            {
                EditorGUILayout.PropertyField(property);
            });

            OnInspectorGUICallback();
        }

        protected virtual void OnInspectorGUICallback()
        {

        }
#endif

        protected virtual void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (AutoRepaint)
                {
                    // Ensure continuous Update calls
                    EditorApplication.QueuePlayerLoopUpdate();
                    SceneView.RepaintAll();
                }
            }
#endif
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SceneViewBehaviour), true)]
    public class SceneViewBehaviourEditor : Editor
    {
        protected void OnSceneGUI()
        {
            if (Application.isPlaying) return;

            //Tools.current = Tool.View;

            var targetBehaviour = target as SceneViewBehaviour;
            Handles.BeginGUI();
            {
                targetBehaviour.OnSceneGUI();
            }
            Handles.EndGUI();

            targetBehaviour.OnSceneGUI(Event.current);
        }

        public override void OnInspectorGUI()
        {
            //serializedObject.Update();
            //serializedObject.ScriptField();

            (target as SceneViewBehaviour).OnInspectorGUI(serializedObject);

            //if (GUI.changed)
            //{
            //    serializedObject.ApplyModifiedProperties();
            //}
        }
    }
#endif
}