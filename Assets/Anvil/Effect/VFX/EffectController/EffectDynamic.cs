using System;
using Anvil;
using MatchThree;
using UnityEngine;

namespace Anvil
{
    /// <summary>
    /// Controlling animation, movement, rotation of an effect
    /// </summary>
    public class EffectDynamic : EffectController, IAnimated
    {
        [SerializeField] private GameObject _animationObj;
        [SerializeField] private bool _autoPlay = true;
        private IAnimationController _animationController;

        [SerializeField]private DiscreteObjectDynamicComponent _objectDynamic;
        
        [SerializeField] protected string _sfxname = "";
        [SerializeField] protected float _delaySFX = 0;
        [SerializeField] protected float _volume = 0.3f;

        Action _onComplete;
        Action _onStartFinish;
        public void AddOnComplete(Action callback)
        {
            _onComplete += callback;
        }
        protected virtual void Awake()
        {
            GameObject animatedObj = gameObject;
            if (_animationObj != null)
            {
                animatedObj = _animationObj;
            }
            _animationController = animatedObj.GetComponent<IAnimationController>() ?? AnimationController.Create(animatedObj);

            if (_objectDynamic == null)
            {
                _objectDynamic = GetComponent<DiscreteObjectDynamicComponent>();
            }
        }

        public void MoveTo(Vector3 position, Action callback = null)
        {
            if (_objectDynamic == null)
            {
                callback?.Invoke();
                return;
            }
            _objectDynamic.MoveTo(position, callback);
            PlayAnimation(AnimationNames.Move);
        }
        public void MoveTo(ITargetDesignator target, Action callback = null)
        {
            if (_objectDynamic == null)
            {
                callback?.Invoke();
                return;
            }
            _objectDynamic.MoveTo(target, callback);
            PlayAnimation(AnimationNames.Move);
        }
        public override void PlayEffect()
        {
            _onStartFinish = OnStartFinish;
            PlayAnimation(AnimationNames.Start, _onStartFinish);
            
        }
        public override void StopEffect()
        {
            _onStartFinish = null;
            PlayAnimation(AnimationNames.Stop, OnComplete);
        }

        public override void PlayEffect(Action callback)
        {
            _onComplete = callback;
            PlayEffect();
        }
        public override void StopEffect(Action callback)
        {
            _onComplete += callback;
            StopEffect();
        }

        private void OnStartFinish()
        {
            if (_autoPlay)
            {
                StopEffect();
            }
            else
            {
                _onStartFinish = null;
            }
        }
        private void OnComplete()
        {
            _onComplete?.Invoke();
            _onComplete = null;
            //GameObjectPool.RemoveObject(gameObject);
        }

        public void PlayAnimation(string animationName)
        {
            if (_animationController == null)
            {
                return;
            }
            _animationController.PlayAnimation(animationName);
        }

        public void PlayAnimation(string animName,Action callback = null)
        {
            if (_animationController == null)
            {
                callback?.Invoke();
                return;
            }
            _animationController.PlayAnimation(animName, callback);
        }

        public void PlayDesignatedSFX()
        {
            if (string.IsNullOrEmpty(_sfxname))
            {
                return;
            }
            Manager.DelayCall(_delaySFX,()=>
            {
                // AudioManager.Instance.PlayEffect(_sfxname, normalizedVolume: _volume);
                
            });
        }
    }
}