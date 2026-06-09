using System;
using Anvil.Legacy;
using UnityEngine;

namespace Anvil
{
    public interface IAnimated
    {
        public void PlayAnimation(string AnimationNames);
        public void PlayAnimation(string AnimationNames, Action callback);
    }
    public class AnimatedObject : MonoBehaviour, IAnimated
    {
        private Legacy.IAnimationController _animationController;

        protected virtual void Awake()
        {
            _animationController = gameObject.CreateAnimationController();
        }
        public virtual void PlayAnimation(string AnimationNames)
        {
            if (_animationController == null)
            {
                return;
            }
            _animationController.PlayAnimation(AnimationNames);
        }

        public virtual void PlayAnimation(string AnimationNames, Action callback)
        {
            if (_animationController == null)
            {
                callback?.Invoke();
                return;
            }
            _animationController.PlayAnimation(AnimationNames, callback);
        }
    }
}