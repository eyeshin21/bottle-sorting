using UnityEngine;
using System;
using System.Collections.Generic;

namespace Anvil
{
    [RequireComponent(typeof(Animator))]
    public class AnimationEndEvent : MonoBehaviour
    {
        static readonly string EventName = "OnEndAnimation";

        class EventData
        {
            public Action Callback { get; set; }
        }

        private Animator _animator;
        private Dictionary<string, EventData> _eventDatas = new Dictionary<string, EventData>();

        void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayAnimation(string name, Action callback)
        {
            bool hasClip = true;
            //Log.Debug($"[{gameObject.name}] Play animation \"{name}\"");
            if (!_eventDatas.TryGetValue(name, out EventData eventData))
            {
                eventData = new EventData();
                hasClip = false;
                var clips = _animator.runtimeAnimatorController.animationClips;
                foreach (var clip in clips)
                {
                    if (clip.name == name)
                    {
                        hasClip = true;
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


                _eventDatas.Add(name, eventData);
            }
            if (!hasClip)
            {
               Debug.LogWarning($"[{gameObject.name}] Animation clip not found: animName=\"{name}\"");
                callback?.Invoke();
                return;
            }
            eventData.Callback = callback;
            _animator.Play(name, 0, 0);
        }
        public void CheckRegisterEndEvent(string name, Action callback)
        {
            if (!_eventDatas.TryGetValue(name, out EventData eventData))
            {
                eventData = new EventData();
                _eventDatas.Add(name, eventData);
            }
            eventData.Callback = callback;
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

        public void OnEndAnimation(string animName)
        {
            if (_eventDatas.TryGetValue(animName, out EventData eventData))
            {
                eventData.Callback?.Invoke();
            }
        }
    }
}