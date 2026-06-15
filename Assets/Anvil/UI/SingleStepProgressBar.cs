using System;
using System.Collections;
using System.Collections.Generic;
using Anvil;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Easer = Anvil.Easer;
using Easers = Anvil.Easers;
using EaseType = Anvil.EaseType;

namespace Anvil
{
    /// <summary>
    /// Base class for step-based UI progress bar
    /// </summary>
    public class SingleStepProgressBar : MonoBehaviour
    {
        // private float epsilon = 0.01f;
        [FormerlySerializedAs("_fullProgressRTF")] [SerializeField] protected RectTransform _innerSpaceRTF;
        [SerializeField] protected RectTransform _barRTF;
        // protected GameObjectReference _iref;
        protected Rect _fullRect;

        [SerializeField] protected AlligmentAxis _progressAxis = AlligmentAxis.Horizontal;
        [SerializeField] private EaseType _easeType = EaseType.SineInOut;
        [SerializeField] protected float _timeScale = 1;
        [FormerlySerializedAs("_duration")] [SerializeField] protected float _durationSec = 1;
        protected int _maxStep = 100;
        protected float _currentStep = 0;
        protected float _stepDiff => _desiredTarget - _currentStep;
        protected float _stepDiffAbs => Mathf.Abs(_stepDiff);
        protected float _startValue = 0;
        protected float _desiredTarget = 0;
        // private int _smoothStepMultiplier = 0;
        protected Easer _easer;

        private Action _callback;


        private Dictionary<float, List<Action>> _progressCallbacks = new Dictionary<float, List<Action>>();
        private Action onComplete;

        public Action<float> onProgressChanged;
        public float ProgressFraction => _currentStep / _maxStep;
        public float Progress => _currentStep;
        public int MaxStep => _maxStep;

        private Action Callback
        {
            get=>_callback;
            set=>_callback = value;
        }

        private bool inited = false;

        public void SetFullRect()
        {
            _fullRect = _innerSpaceRTF.rect;
        }

        protected virtual void Start()
        {
            if (inited)
            {
                return;
            }

            // Init();
            // _iref = GetComponent<GameObjectReference>();
            // if (_innerSpaceRTF == null)
            // {
            //     if (_iref == null)
            //     {
            //         Debug.LogError("progress bar reference has not been assigned or reference component not found");
            //         return;
            //     }
            //     _innerSpaceRTF = _iref.GetGameObjectReference(GameObjectReferenceID.ContentPopup).GetComponentSafe<RectTransform>();
            //     _barRTF = _iref.GetGameObjectReference(GameObjectReferenceID.Progress_bar).GetComponentSafe<RectTransform>();
            // }
            _fullRect = _innerSpaceRTF.rect;
            _currentStep = 0;
            inited = true;
            
            _easer = Easers.GetEaser(_easeType);
        }
        [Button]
        private void RefreshEaser()
        {
            _easer = Easers.GetEaser(_easeType);
        }

        private void OnDisable()
        {
            _isParalelUpdate = false;
            StopAllCoroutines();
        }

        public void Init(bool paralleUpdate = false)
        {
            _isParalelUpdate = paralleUpdate;
            // if (!inited)
            // {
            //     Awake();
            // }

            // CalculateSpeed();
            SetAnimParam(_durationSec, _timeScale);
            if (_isParalelUpdate)
            {
                StartCoroutine(UpdateCoroutine());
                onComplete += () =>
                {
                    _isParalelUpdate = false;
                };
            }
        }

        private IEnumerator UpdateCoroutine()
        {
            while (_isParalelUpdate)
            {
                OnUpdate(Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
        }

        protected float _speed = 0;

        protected void CalculateSpeed()
        {
            _speed = _maxStep / _durationSec;
        }

        protected virtual void OnProgressChanged(float fraction)
        {
            onProgressChanged?.Invoke(fraction);
            // Debug.Log($"on progress changed : {fraction}");
        }

        private bool _isParalelUpdate = false;

        protected void Update()
        {
            if (!_isParalelUpdate)
            {
                OnUpdate(Time.deltaTime);
            }
            // OnUpdate(Time.fixedDeltaTime);
        }

        private float _elapsedTime = 0;
        public bool logDebug = false;
        protected virtual void OnUpdate(float deltaTime)
        {
            if (Mathf.Approximately(_stepDiff, Mathf.Epsilon))
            {
                return;
            }

            _elapsedTime += deltaTime;
            // float increment = _speed * deltaTime * _timeScale * (_stepDiff > 0 ? 1 : -1);
            float increment = _easer.Invoke(_elapsedTime/_durationSec) * (_desiredTarget - _startValue);
            increment = increment + _startValue - _currentStep;
            if (logDebug)
            {
                Debug.Log($"increment {increment}, step diff {_stepDiff}, elapsed {_elapsedTime}, duration {_durationSec}, start {_startValue}, desired {_desiredTarget}, current {_currentStep}");
            }
            if (Mathf.Abs(increment) >= Mathf.Abs(_stepDiff))
            {
                // increment = _stepDiff ;
                //increment = _stepDiff + (_stepDiff > 0 ? epsilon : -epsilon); // epsion is added to ensure final step value cross the neccessary value for callback since some how calculation is alway short of 0.0000001 at the last increment step???

                //No need to prevent overshoot. value will be clamped anyway
                _currentStep = _desiredTarget;
            }
            else
            {
                _currentStep += increment;
            }

            if (_currentStep < 0)
            {
                _currentStep = 0;
            }
            else if (_currentStep > _maxStep)
            {
                _currentStep = _maxStep;
            }

            float fraction = _currentStep / _maxStep;
            Mathf.Clamp(fraction, 0, 1.0f);
            UpdateView(fraction, _barRTF);

            OnProgressChanged(fraction);

            if (Mathf.Approximately(_currentStep, _desiredTarget))
            {
                if (Callback != null)
                {
                    Action tmpCallback = Callback;
                    Callback = null;
                    tmpCallback?.Invoke();
                }
                return ;
            }
            var keyCollection = _progressCallbacks.GetKeys();
            float diffRange = _currentStep - increment;
            // Debug.Log($"{keyCollection.Count} keys found at diff range {diffRange}-{_currentStep}");
            for (int i = 0; i < keyCollection.Count; i++)
            {
            
                float key = keyCollection[i];
            
                if (diffRange >= _currentStep)
                {
                    if (key >= _currentStep && key <= diffRange)
                    {
                        // Debug.Log($"key range {key} {_currentStep} {diffRange} invoked");
                        InvokeCallbackAt(key, out _);
                        // _progressCallbacks[key]?.Invoke();
                        // _progressCallbacks.Remove(key);
                    }
                }
                else if (diffRange < _currentStep)
                {
                    if (key >= diffRange && key <= _currentStep)
                    {
                        // Debug.Log($"key range {key} {_currentStep} {diffRange} invoked");
                        InvokeCallbackAt(key, out _);
                        // _progressCallbacks[key]?.Invoke();
                        // _progressCallbacks.Remove(key);
                    }
                }
            }
        }

        public virtual void SetProgressImidiate(float progress)
        {
            _elapsedTime = 0;
            progress = Mathf.Clamp(progress, 0, _maxStep);
            float fraction = (float)progress / _maxStep;
            _currentStep = progress;
            _startValue = _currentStep;
            
            _desiredTarget = progress;
            UpdateView(fraction, _barRTF);
            OnProgressChanged(fraction);

            Action tmpCallback = Callback;
            Callback = null;
            tmpCallback?.Invoke();
        }

        protected virtual void UpdateView(float fraction, RectTransform barRTF)
        {
            float calculatedSize = _progressAxis == AlligmentAxis.Horizontal
                ? _fullRect.width * fraction
                : _fullRect.height * fraction;

            if (barRTF == null)
            {
                Debug.Log("bar null");
            }
            switch (_progressAxis)
            {
                case AlligmentAxis.Vertical:
                    barRTF.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, calculatedSize);
                    break;
                case AlligmentAxis.Horizontal:
                    barRTF.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, calculatedSize);
                    break;
            }
        }

        public virtual void SetAnimParam(float duration, float timeScale)
        {
            _durationSec = duration;
            _timeScale = timeScale;
            CalculateSpeed();
        }
        public virtual void GetAnimParam(out float duration, out float timeScale)
        {
            duration = _durationSec;
            timeScale = _timeScale;
        }

        public virtual void SetProgressParam(int maxStep, int currentStep = 0, bool updateView = true)
        {
            if (_maxStep != maxStep)
            {
                _maxStep = maxStep;
                CalculateSpeed();
            }
            else
            {
                _maxStep = maxStep;
            }

            if (updateView)
            {
                SetProgressImidiate(currentStep);
            }
            else
            {
                SetProgress(currentStep);
            }
            // _smoothStepMultiplier = smoothMultiplier > 1 ? smoothMultiplier : 1;
        }

        public virtual void SetProgress(float progress, Action callback = null)
        {
            _elapsedTime = 0;
            progress = Mathf.Clamp(progress, 0, _maxStep);
            _desiredTarget = progress;

            if (_stepDiff == 0)
            {
                callback?.Invoke();
            }
            else
            {
                _startValue = _currentStep;
                // InsertCallback(progress, callback);
                Action tmpCallback = Callback;
                Callback = callback;

                tmpCallback?.Invoke();
            }
        }

        public virtual void SetProgressTowardFinish(Action callback = null)
        {
            SetProgressFraction(1f, callback);
        }

        public virtual void SetProgressFraction(float fraction, Action callback = null)
        {
            SetProgress(fraction * _maxStep, callback);
        }
        public virtual void SetProgressFractionImidiate(float fraction)
        {
            SetProgressImidiate(fraction * _maxStep);
        }


        /// <summary>
        /// DEPRECATED Only support single callback from this base class
        /// </summary>
        /// <param name="increment"></param>
        /// <param name="callback"></param>
        public void IncrementProgress(float increment = 1, Action callback = null)
        {
            if ( _stepDiffAbs > 0)
            {
                //InsertCallback(_desiredTarget + increment, callback);

                _desiredTarget += increment;
            }
            else
            {
                SetProgress(_currentStep + increment, callback);
            }
        }
        private void InsertCallback(float step, Action callback)
        {
            if (callback == null)
            {
                return;
            }
            float stepFraction = step / _maxStep;
            // step = step - epsilon;
            if (!_progressCallbacks.ContainsKey(step))
            {
                _progressCallbacks.Add(step, new List<Action>());
            }
            _progressCallbacks[step].Add(callback);
            Debug.Log($"callback inserted {step} dif {_stepDiff}");
        
        }
        
        private bool InvokeCallbackAt(float step, out Action callback)
        {
            callback = null;
            if (_progressCallbacks.ContainsKey(step))
            {
                List<Action> callbacks = _progressCallbacks[step];
                if (callbacks.IsNullOrEmpty())
                {
                    return false;
                }
                callback = callbacks[0];
                callbacks.RemoveAt(0);
                callback?.Invoke();
                return true;
            }
            return false;
        }
    }
}
