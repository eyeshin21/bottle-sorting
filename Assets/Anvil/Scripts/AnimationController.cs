using System;
using UnityEngine;

namespace Anvil
{
   
    public interface IAnimationController
    {
        public void PlayAnimation(int animationHash);
        public void PlayAnimation(string animationName);
        public void PlayAnimation(string animationName, Action callback);
        
        void OnAnimationEnd(string animationName);
    }
    public abstract class AnimationControllerComponent : MonoBehaviour, IAnimationController
    {
        public abstract void PlayAnimation(int animationHash);
        public abstract void PlayAnimation(string animationName);

        public abstract void PlayAnimation(string animationName, Action callback);
        public abstract void OnAnimationEnd(string animationName);
    }
    public class AnimationEndHandler : StateMachineBehaviour
    {
        public string animationName;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var controller = animator.GetComponent<IAnimationController>();
            controller?.OnAnimationEnd(animationName);
        }
    }
    public class AnimatorAnimationController : AnimationControllerComponent
    {
        private Action _endCallback;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public override void PlayAnimation(int animationHash)
        {
            _animator.Play(animationHash);
        }

        public override void PlayAnimation(string animationName)
        {
            _animator.Play(animationName);
        }

        public override void PlayAnimation(string animationName, Action callback)
        {
            _endCallback = callback;
            _animator.Play(animationName);
        }

        public override void OnAnimationEnd(string animationName)
        {
            _endCallback?.Invoke();
            _endCallback = null;
        }
    }
}