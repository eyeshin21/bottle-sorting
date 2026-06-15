using System;
using Anvil;
using UnityEngine;

namespace Anvil
{
    public class StateBasedAnimationController : MonoBehaviour, IAnimationEventListener
    {
        [SerializeField]protected GameObject _animationObject;
        protected Animator _animationController;
        private Action _animationEvent1;
        private Action _animationEvent2;
        private Action _animationEndEvent;
        private AnimationEventForwarder _animationEventForwarder;

        private AnimationEventForwarder EventForwarder
        {
            get
            {
                if (_animationEventForwarder == null)
                {
                    _animationEventForwarder = _animationObject.CheckAddComponent<AnimationEventForwarder>();
                    _animationEventForwarder.RegisterListener(this);
                }
                return _animationEventForwarder; 
            }
        }
        protected Animator AnimationController
        {
            get
            {
                if (_animationController == null && _animationObject != null)
                {
                    _animationController = _animationObject.GetComponent<Animator>();
                }
                return _animationController;
            }
        }

        private bool CheckAddEndEvent(string name)
        {
            var clips = AnimationController.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (clip.name == name)
                {
                    int eventCount = clip.events.Length;
                    if (eventCount == 0)
                    {
                        AddEndEvent(clip, name);
                    }
                    else
                    {
                        var evt = clip.events[eventCount - 1];
                        if (evt.time == clip.length)
                        {
                            evt.functionName = nameof(AnimationEndEvent);
                            // evt.stringParameter = name;
                        }
                        else
                        {
                            AddEndEvent(clip, name);
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        void AddEndEvent(AnimationClip clip, string name)
        {
            //Log.Debug($"[{gameObject.name}] Add event: animName=\"{name}\", length={clip.length}");
            var evt = new AnimationEvent();
            evt.time = clip.length;
            evt.functionName = nameof(EventForwarder.AnimationEndEvent);
            // evt.stringParameter = name;
            clip.AddEvent(evt);
        }
        // public void RegisterAnimationEvent1(Action callback)
        // {
        //     _animationEvent1 += callback;
        // }
        //
        // public void RegisterAnimationEvent2(Action callback)
        // {
        //     _animationEvent2 += callback;
        // }
        // public void RegisterAnimationEndEvent(Action callback)
        // {
        //     _animationEndEvent += callback;
        // }
        public void OverrideAnimationEndEvent(Action callback)
        {
            _animationEndEvent = callback;
        }

        public void AnimationEndEvent()
        {
            Action endEvent = _animationEndEvent;
            _animationEndEvent = null;
            endEvent?.Invoke();
        }

        public void OverrideAnimationEvent1(Action callback)
        {
            _animationEvent1 = callback;
        }
        public void OverrideAnimationEvent2(Action callback)
        {
            _animationEvent2 = callback;
        }
        
        public void AnimationEvent1()
        {
            Action action = _animationEvent1;
            _animationEvent1 = null;
            action?.Invoke();
        }
        public void AnimationEvent2()
        {
            Action action = _animationEvent2;
            _animationEvent2 = null;
            action?.Invoke();
        }

        private void ClearEvents()
        {
            _animationEvent1 = null;
            _animationEvent2 = null;
            _animationEndEvent = null;
        }
        private bool ForcePlayAnimation(string animName, Action callback = null)
        {
            if (AnimationController == null)
            {
                return false;
            }

            if (!CheckAddEndEvent(animName))
            {
                return false;
            }
            ClearEvents();
            OverrideAnimationEndEvent(callback);
            AnimationController.Play(animName, 0, 0);
            
            return true;
        }
        private bool TryPlayAnimation(string animName, Action callback = null)
        {
            if (AnimationController == null)
            {
                return false;
            }

            if (!CheckAddEndEvent(animName))
            {
                return false;
            }

            AnimationController.SetBool(animName, true);
            OverrideAnimationEndEvent(() =>
            {
                AnimationController.SetBool(animName, false);
            });
            return true;
        }
    }
}