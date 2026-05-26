using UnityEngine;

namespace Anvil.Legacy
{
    public class ParticleSystemFinishHandler : MonoBehaviour, IFinishHandler
    {
        [SerializeField] bool _withChildren;
        [SerializeField] bool _finishOnDisabled;

        ParticleSystem _particleSystem;
        event Listener _onFinish;
        bool _checkFinish;

        void Awake()
        {
            _particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        void OnEnable()
        {
            _checkFinish = true;
        }

        void OnDisable()
        {
            if (_finishOnDisabled)
            {
                if (_checkFinish)// && !_particleSystem.IsAlive(_withChildren))
                {
                    _checkFinish = false;
                    //_onFinish?.Invoke(); // Cannot set the parent of the GameObject 'Current' while activating or deactivating the parent GameObject 'Parent'.
                    Manager.CallOnUpdate(() => _onFinish?.Invoke());
                }
            }
        }

        public void AddOnFinish(Listener listener)
        {
            _onFinish += listener;
        }

        void Update()
        {
            if (_checkFinish)
            {
                if (!_particleSystem.IsAlive(_withChildren))
                {
                    _checkFinish = false;
                    //gameObject.SetActive(false);
                    _onFinish?.Invoke();
                }
            }
        }

#if UNITY_EDITOR
        public ParticleSystemFinishHandler SetWithChildren(bool withChildren)
        {
            _withChildren = withChildren;
            return this;
        }

        public ParticleSystemFinishHandler SetFinishOnDisabled(bool finishOnDisabled)
        {
            _finishOnDisabled = finishOnDisabled;
            return this;
        }
#endif
    }
}