using System;
using Anvil.Legacy;
using UnityEngine;
using UnityEngine.Rendering;

namespace Anvil
{
    public enum EffectControllType
    {
        Particle,
        Animation,
        Movement,
        AnimationMovementComposite,
    }

    /// <summary>
    /// Legacy effect controller
    /// </summary>
    public class EffectController_v0 : MonoBehaviour
    {
        [SerializeField] private bool isAutoScale;
        [SerializeField] private Vector3 defaultAutoScale = Vector3.one;
        private IColorChangable _colorController;
        private GameObject _originPrefab;

        private SortingGroup _sortingGroup;
        private string _defaultSortingLayerName;
        private int _defaultSortingOrder;

        public float removeDelay = 3f;

        public effectCallbackAction endType;
        public effectAwakeAction awakeType;

        public bool AutoScale => isAutoScale;
        public Vector3 DefaultScale => defaultAutoScale;

        public EffectControllType controllType = EffectControllType.Particle;
        private EffectHandler _handler;
        public EffectHandler Handler => _handler;

        private Action _finalCallback = null;
        private Action _callback = null;


        protected virtual void Awake()
        {
            Init();
        }

        private void FixedUpdate()
        {
            _handler.OnUpdate(Time.fixedDeltaTime);
        }

        public void Init()
        {
            if (controllType == EffectControllType.Particle)
            {
                _handler = new ParticleEffectHandler();
            }
            else if (controllType == EffectControllType.Animation)
            {
                _handler = new AnimationEffectHandler();
            }
            else if (controllType == EffectControllType.Movement)
            {
                _handler = new MovementEffectHandler();
            }

            _handler.Init(this);
            _handler.onEndCallback = OnFxEnd;
        }

        private void OnParticleSystemStopped()
        {
            if (_handler is ParticleEffectHandler)
            {
                OnFxEnd();
            }
        }

        public ScriptedEvent ConvertToScriptedEvent()
        {
            return new ScriptedEvent((callback) =>
            {
                Play(callback);
            });
        }
        public virtual void Play(Action callback = null , Action fxCallback = null)
        {
            InsertCallback(callback, fxCallback);
            _handler.Play();
        }

        private void OnEnable()
        {
            if (awakeType == effectAwakeAction.Play)
            {
                Play();
            }
        }

        public void InsertCallback(Action finalCallback, Action fxCallback = null)
        {
            _callback = fxCallback;
            // _handler.onEndCallback += _callback;

            if (_finalCallback == null)
            {
                _finalCallback = finalCallback;
                return;
            }
            _finalCallback += finalCallback;
        }

        protected virtual void OnFxEnd()
        {
            _callback?.Invoke();
            _callback = null;

            var callback = _finalCallback;
            _finalCallback = null;

            switch (endType)
            {
                case effectCallbackAction.ReturnToPool:
                    // Debug.Log("returning effect to pool");
                    if (removeDelay > 0)
                    {
                        DelayReturnToPool(callback: InvokeCallback);
                    }
                    else
                    {
                        ReturnToPool();
                        InvokeCallback();
                    }
                    break;
                case effectCallbackAction.DisableAwakeLoop:
                    SetPlayOnAwake(false);
                    InvokeCallback();
                    break;
                case effectCallbackAction.Nothing:
                    InvokeCallback();
                    break;
                default:
                    break;
            }

            void InvokeCallback()
            {
                callback?.Invoke();
            }
        }

        protected virtual void SetPlayOnAwake(bool state = true)
        {
            if (state)
            {
                awakeType = effectAwakeAction.Play;
                return;
            }

            awakeType = effectAwakeAction.Nothing;
        }

        public void Init(GameObject originPrefab)
        {
            _originPrefab = originPrefab;
        }

        public void SetSortingOrder(int sortingOrder = -1, string layerName = "")
        {
            if (_sortingGroup == null)
            {
                _sortingGroup = gameObject.GetOrAddComponent<SortingGroup>();
                _defaultSortingLayerName = _sortingGroup.sortingLayerName;
                _defaultSortingOrder = _sortingGroup.sortingOrder;
            }

            if (layerName != "")
            {
                _sortingGroup.sortingLayerName = layerName;
            }

            if (sortingOrder > 0)
            {
                _sortingGroup.sortingOrder = sortingOrder;
            }
        }

        public void ReturnToPool()
        {
            TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
            if (trail != null)
            {
                trail.Clear();
            }

            if (_finalCallback != null)
            {
                _finalCallback?.Invoke();
                _finalCallback = null;
            }

            EffectManager.RemoveEffect(this);

            if (_sortingGroup != null)
            {
                SetSortingOrder(_defaultSortingOrder, _defaultSortingLayerName);
            }
            //Debug.Log("Deleted Effect");
        }

        public void DelayReturnToPool(float delay = -1, Action callback = null)
        {
            transform.SetParent(null);
            if (delay < 0)
            {
                delay = removeDelay;
            }else if (delay == 0)
            {
                EffectManager.RemoveEffect(this);
                return;
            }

            Manager.DelayCall(delay, () =>
            {
                EffectManager.RemoveEffect(this);
                callback?.Invoke();
            });
            //Debug.Log("Deleted Effect");
        }

        public void ChangeColor(Color color)
        {
            if (_colorController == null)
            {
                Debug.Log("Colorcontroller havent been inited");
                return;
            }

            _colorController.ChangeColor(color);
        }

        public void ChangeColor(Gradient gradient)
        {
            if (_colorController == null)
            {
                Debug.Log("Colorcontroller havent been inited");
                return;
            }

            _colorController.ChangeColor(gradient);
        }

        public void ChangeColor(Color minColor, Color maxColor)
        {
            if (_colorController == null)
            {
                Debug.Log("Colorcontroller havent been inited");
                return;
            }

            _colorController.ChangeColor(minColor, maxColor);
        }
    }
}
