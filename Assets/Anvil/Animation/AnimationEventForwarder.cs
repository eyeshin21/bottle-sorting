using System;
using UnityEngine;

namespace Anvil
{
    public interface IAnimationEventListener
    {
        public void AnimationEndEvent();
        public void AnimationEvent1();
        public void AnimationEvent2();
    }
    public class AnimationEventForwarder : MonoBehaviour
    {
        private Action _animationEndEvent;
        private Action _animationEvent1;
        private Action _animationEvent2;
        private IAnimationEventListener _listener;
        
        public void RegisterListener(IAnimationEventListener listener)
        {
            _listener = listener;
        }

        private void AnimationEvent1()
        {
            _listener?.AnimationEvent1();
        }
        private void AnimationEvent2()
        {
            _listener?.AnimationEvent2();
        }
        public void AnimationEndEvent()
        {
            _listener?.AnimationEndEvent();
        }
    }
}