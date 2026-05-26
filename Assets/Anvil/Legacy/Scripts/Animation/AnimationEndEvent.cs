using UnityEngine;
using System;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    // [RequireComponent(typeof(Animator))]
    public class AnimationEndEvent : MonoBehaviour
    {
        static string EventName => "OnEndAnimation";

        class EventData
        {
            public Action Callback { get; set; }
        }

        private Animator _animator;
        private Animation _animation;
        private Dictionary<string, EventData> _eventDatas = new Dictionary<string, EventData>();

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _animation = GetComponent<Animation>();
        }

        protected virtual void PlayAnimation(string animationName)
        {
            if (_animator != null)
            {
                _animator.Play(animationName, 0, 0);
            }
            else if (_animation != null)
            {
                _animation.Play(animationName);
            }
            else
            {
                Debug.LogError("Animation end event: PlayAnimation failed, no animator or animation component found");
            }
        }
        protected virtual AnimationClip[] GetAllAnimationClips()
        {
            if (_animation != null)
            {
                AnimationClip[] ret = new AnimationClip[_animation.GetClipCount()];
                int index = 0;
                foreach (AnimationState animationState in _animation)
                {
                    AnimationClip clip = animationState.clip;
                    ret[index] = clip;
                    index++;
                }
                return ret;
            }
            else if (_animator != null)
            {
                return _animator.runtimeAnimatorController.animationClips;
            }

            Debug.LogError("Animation end event: GetAllAnimationClips failed, no animator or animation component found");
            return Array.Empty<AnimationClip>();
        }

        public void PlayAnimation(string name, Action callback)
        {
            //Log.Debug($"[{gameObject.name}] Play animation \"{name}\"");
            if (!_eventDatas.TryGetValue(name, out EventData eventData))
            {
                
                eventData = new EventData();

                // var clips = _animator.runtimeAnimatorController.animationClips;
                AnimationClip[] clips = GetAllAnimationClips();
                bool found = false;
                foreach (var clip in clips)
                {
                    if (clip.name == name)
                    {
                        found = true;
                        int eventCount = clip.events.Length;
                        if (eventCount == 0)
                        {
                            AddEndEvent(clip, name);
                        }
                        else
                        {
                            //Log.Warning($"[{gameObject.name}] End event added: animName=\"{name}\"");
                            var evt = clip.events[eventCount - 1];
                            if (evt.time == clip.length)
                            {
                                //Log.Debug($"Update end event \"{name}\"");
                                evt.functionName = EventName;
                                evt.stringParameter = name;
                            }
                            else
                            {
                                AddEndEvent(clip, name);
                            }
                        }
                        break;
                    }
                }

                if (!found)
                {
                    callback?.Invoke();
                    return;
                }

                _eventDatas.Add(name, eventData);
            }

            eventData.Callback = callback;

            // _animator.Play(name, 0, 0);
            PlayAnimation(name);
        }

        void AddEndEvent(AnimationClip clip, string name)
        {
            //Log.Debug($"[{gameObject.name}] Add event: animName=\"{name}\", length={clip.length}");
            var evt = new AnimationEvent();
            evt.time = clip.length;
            evt.functionName = EventName;
            evt.stringParameter = name;
            clip.AddEvent(evt);
        }

        public void OnEndAnimation(string name)
        {
            if (_eventDatas.TryGetValue(name, out EventData eventData))
            {
                eventData.Callback?.Invoke();
                eventData.Callback = null;
            }
        }
    }
}
