using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Anvil
{
    public class ColorInterpolate : MonoBehaviour , IScriptedEvent
    {
        public bool IsCompleted { get; }
        
        [SerializeField] List<GameObject> coloredObjects;
        [SerializeField] bool playOnStart = true; 
        [SerializeField] public Color startColor = Color.cyan;
        [SerializeField] public Color endColor = Color.magenta;  // error color
        [SerializeField] private float duration = 1f;

        private List<IColorChangable> _colorControllers;
        private bool _isActive = false;
        private float _elapsedTime = 0f;
        private Action _callback = null;
        [SerializeField,ReadOnly] Color _currentColor;

        private void Awake()
        {
            _colorControllers = new List<IColorChangable>();
            if (coloredObjects.IsNullOrEmpty())
            {
                return;
            }
            foreach (GameObject colorObj in coloredObjects)
            {
                var colorController = colorObj.GetComponent<IColorChangable>();
                if (colorController != null)
                {
                    _colorControllers.Add(colorController);
                }
            }
        }

        private void Update()
        {
            if (!_isActive)
            {
                return;
            }
            _elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(_elapsedTime / duration);
            _currentColor = Color.Lerp(startColor, endColor, t);
            foreach (IColorChangable controller in _colorControllers)
            {
                controller.ChangeColor(_currentColor);
            }

            if (_elapsedTime >= duration)
            {
                _isActive = false;
                _callback?.Invoke();
                _callback = null;
            }
        }

        private void Start()
        {
            if (playOnStart)
            {
                Execute();
            }
        }

        public void Execute(Action callback = null)
        {
            _callback = callback;
            _isActive = true;
            _elapsedTime = 0f;
        }
    }
}