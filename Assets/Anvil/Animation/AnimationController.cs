#if DEBUG_MODE
#undef DEBUG_ANIMATION_CONTROLLER
#endif
using UnityEngine;
using System;
using System.Diagnostics;
using Anvil;
using Animation = UnityEngine.Animation;
using Debug = UnityEngine.Debug;

#if SPINE_ENABLED
using Spine;
using Spine.Unity;
#endif

namespace Anvil
{
    public interface IAnimationController
    {
        public string CurrentClipName { get; }
        public bool IsPlaying(string name);
        GameObject gameObject { get; }
        void PlayAnimation(string name);
        void PlayAnimation(string name, bool loop);
        void PlayAnimation(string name, Action callback);
        void PlayAnimations(string name, string name2, bool loop = false);
        void PlayAnimations(string name, string name2, string name3, bool loop = false);
        void RequestAnimation(string name, bool loop);
        void AddEvent(string eventName, Action callback);
        void AddEvent(string eventName, Action<int> callback);

        /// <summary>
        /// Play to end of animation.
        /// </summary>
        void ForcePlayAnimation(string name);

        void SetSpeed(float speed);
        void SetShow(bool show);
        public float StateProgress { get; }
    }

    public abstract class BaseAnimationController : IAnimationController
    {
        protected GameObject _gameObject;

        public virtual string CurrentClipName => "Unknown";
        public virtual GameObject gameObject => _gameObject;
        public abstract void PlayAnimation(string name);

        public virtual bool IsPlaying(string name)
        {
            return CurrentClipName == name;
        }
        public virtual void PlayAnimation(string name, bool loop)
        {
            if (loop)
            {
                LogCannotPlayLoopAnimation(name);
            }

            PlayAnimation(name);
        }

        public virtual void PlayAnimation(string name, Action callback)
        {
            LogWarning($"Can't play animation \"{name}\" with callback!");
            PlayAnimation(name);
            callback?.Invoke();
        }

        public virtual void PlayAnimations(string name, string name2, bool loop = false)
        {
            LogCannotPlayAnimations(name, name2);
            PlayAnimation(name2, loop);
        }

        public virtual void PlayAnimations(string name, string name2, string name3, bool loop = false)
        {
            LogCannotPlayAnimations(name, name2, name3);
            PlayAnimation(name3, loop);
        }

        public virtual void RequestAnimation(string name, bool loop)
        {
            PlayAnimation(name, loop);
        }

        public virtual void AddEvent(string eventName, Action callback)
        {
            LogWarning($"Can't add event \"{eventName}\"!");
        }

        public virtual void AddEvent(string eventName, Action<int> callback)
        {
            LogWarning($"Can't add event \"{eventName}\"!");
        }

        public virtual void ForcePlayAnimation(string name)
        {
            LogCannotForcePlayAnimation(name);
            PlayAnimation(name);
        }

        public virtual void SetSpeed(float speed)
        {
            LogCannotSetSpeed(speed);
        }

        public virtual void SetShow(bool show)
        {
            gameObject.SetShow(show);
        }

        public virtual float StateProgress => 1;

        [Conditional("DEBUG_ANIMATION_CONTROLLER")]
        protected static void LogWarning(string message)
        {
           Debug.LogWarning(message);
        }

        [Conditional("DEBUG_ANIMATION_CONTROLLER")]
        protected static void LogCannotPlayAnimation(string name)
        {
           Debug.LogWarning($"Can't play animation \"{name}\"!");
        }

        [Conditional("DEBUG_ANIMATION_CONTROLLER")]
        protected static void LogCannotPlayLoopAnimation(string name)
        {
           Debug.LogWarning($"Can't play loop animation \"{name}\"!");
        }

        [Conditional("DEBUG_ANIMATION_CONTROLLER")]
        protected static void LogCannotPlayAnimations(string name, string name2)
        {
           Debug.LogWarning($"Can't play animation \"{name}\" and \"{name2}\"!");
        }

        [Conditional("DEBUG_ANIMATION_CONTROLLER")]
        protected static void LogCannotPlayAnimations(string name, string name2, string name3)
        {
           Debug.LogWarning($"Can't play animation \"{name}\", \"{name2}\" and \"{name3}\"!");
        }

        [Conditional("DEBUG_ANIMATION_CONTROLLER")]
        protected static void LogCannotForcePlayAnimation(string name)
        {
           Debug.LogWarning($"Can't force play animation \"{name}\"!");
        }

        [Conditional("DEBUG_ANIMATION_CONTROLLER")]
        protected static void LogCannotSetSpeed(float speed)
        {
           Debug.LogWarning($"Can't set speed {speed}!");
        }

        [Conditional("DEBUG_ANIMATION_CONTROLLER")]
        protected static void LogInactive(GameObject gameObject)
        {
           Debug.LogWarning($"GameObject {gameObject.name} is inactive!");
        }
    }

    public class DefaultAnimationController : BaseAnimationController
    {
        public DefaultAnimationController(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public override void PlayAnimation(string name)
        {
            LogCannotPlayAnimation(name);
        }
    }
    public class LegacyAnimationController : BaseAnimationController
    {
        private Animation _animation;

        public LegacyAnimationController(Animation animation)
        {
            _animation = animation;
        }
        public override void PlayAnimation(string name)
        {
            if (_animation == null)
            {
                return;
            }

            _animation.Play(name);
        }
    }

    public class AnimatorAnimationController : BaseAnimationController
    {
        private Animator _animator;
        private AnimationEndEvent _animationEndEvent;

        public override bool IsPlaying(string name)
        {
            if (_animator != null)
            {
                var info = _animator.GetCurrentAnimatorStateInfo(0);
                return info.IsName(name);
            }
            return false;
        }

        public override float StateProgress
        {
            get
            {
                if (_animator != null)
                {
                    var info = _animator.GetCurrentAnimatorStateInfo(0);
                    return info.normalizedTime;
                }
                return 1;
            }
        }

        public AnimatorAnimationController(GameObject gameObject, Animator animator)
        {
            _gameObject = gameObject;
            _animator = animator;
        }

        public override void PlayAnimation(string name)
        {
            if (_animator != null)
            {
                if (!_animator.gameObject.activeInHierarchy)
                {
                    LogInactive(_animator.gameObject);
                }

                _animator.Play(name, 0, 0);
            }
            else
            {
                LogCannotPlayAnimation(name);
            }
        }

        public void SetParameter(string name, bool value, Action callback = null)
        {
            if (_animator != null)
            {
                _animator.SetBool(name, value);
                _animationEndEvent.CheckRegisterEndEvent(name, callback);
            }
            else
            {
                LogWarning($"[animator animation controller] Can't set parameter \"{name}\" to {value}!");
            }
        }

        public override void PlayAnimation(string name, bool loop)
        {
            //TODO: Check loop
            PlayAnimation(name);
        }

        public override void PlayAnimation(string name, Action callback)
        {
            if (_animator != null)
            {
                if (_animator.gameObject.activeInHierarchy)
                {
                    if (_animationEndEvent == null)
                    {
                        _animationEndEvent = _animator.gameObject.GetOrAddComponent<AnimationEndEvent>();
                    }

                    _animationEndEvent.PlayAnimation(name, callback);
                }
                else
                {
                    LogInactive(_animator.gameObject);
                    callback?.Invoke();
                }
            }
            else
            {
                LogCannotPlayAnimation(name);
                callback?.Invoke();
            }
        }

        public override void ForcePlayAnimation(string name)
        {
            if (_animator != null)
            {
                _animator.Play(name, 0, 1);
                _animator.Update(0.02f);
            }
            else
            {
                LogCannotForcePlayAnimation(name);
            }
        }

        public override void SetSpeed(float speed)
        {
            if (_animator != null)
            {
                _animator.speed = speed;
            }
            else
            {
                LogCannotSetSpeed(speed);
            }
        }
    }
#if SPINE_ENABLED

    public abstract class SpineAnimationController : BaseAnimationController
    {
        protected ISkeletonAnimation _skeletonAnimation;
        protected Spine.AnimationState _animationState;

        public override string CurrentClipName
        {
            get
            {
                if (_animationState != null)
                {
                    var trackEntry = _animationState.GetCurrent(0);
                    if (trackEntry != null)
                    {
                        return trackEntry.Animation.Name;
                    }
                }

                return "Unknown";
            }
        }

        public override float StateProgress
        {
            get
            {
                if (_animationState != null)
                {
                    var trackEntry = _animationState.GetCurrent(0);
                    if (trackEntry != null)
                    {
                        return trackEntry.TrackTime / trackEntry.Animation.Duration;
                    }
                }

                return 1;
            }
        }

        protected abstract SkeletonData SkeletonData { get; }

        protected void Construct(ISkeletonAnimation skeletonAnimation, Spine.AnimationState animationState)
        {
            _skeletonAnimation = skeletonAnimation;
            _animationState = animationState;
        }

        public override void PlayAnimation(string name)
        {
            if (_animationState != null)
            {
                _animationState.SetAnimation(0, name, false);
            }
            else
            {
                LogCannotPlayAnimation(name);
            }
        }

        public override void PlayAnimation(string name, bool loop)
        {
            if (_animationState != null)
            {
                _animationState.SetAnimation(0, name, loop);
            }
            else
            {
                LogCannotPlayLoopAnimation(name);
            }
        }
        public override void RequestAnimation(string name, bool loop)
        {
            if (_animationState != null)
            {
                _animationState.AddAnimation(0, name, loop, 0);
            }
            else
            {
                LogCannotPlayLoopAnimation(name);
            }
        }

        public override void PlayAnimation(string name, Action callback)
        {
            if (_animationState == null || _skeletonAnimation == null)
            {
                LogCannotPlayAnimation(name);
                callback?.Invoke();
                return;
            }

            if (string.IsNullOrEmpty(name))
            {
                LogWarning("Can't play animation with empty name!");
                callback?.Invoke();
                return;
            }
            var trackEntry = _animationState.GetCurrent(0);
            var isSame = trackEntry != null && trackEntry.Animation != null && trackEntry.Animation.Name == name;

            if (isSame)
            {
                // Force restart if the same animation is requested again.
                trackEntry = _animationState.SetAnimation(0, name, false);
                trackEntry.TrackTime = 0f;
            }
            else
            {
                trackEntry = _animationState.SetAnimation(0, name, false);
            }
            trackEntry.Complete += (trackEntry2) => callback?.Invoke();
        }

        public override void PlayAnimations(string name, string name2, bool loop = false)
        {
            if (_animationState != null)
            {
                var animationState = _animationState;
                animationState.SetAnimation(0, name, false);
                animationState.AddAnimation(0, name2, loop, 0);
            }
            else
            {
                LogCannotPlayAnimations(name, name2);
            }
        }

        public override void PlayAnimations(string name, string name2, string name3, bool loop = false)
        {
            if (_animationState != null)
            {
                var animationState = _animationState;
                animationState.SetAnimation(0, name, false);
                animationState.AddAnimation(0, name2, false, 0);
                animationState.AddAnimation(0, name3, loop, 0);
            }
            else
            {
                LogCannotPlayAnimations(name, name2, name3);
            }
        }

        public override void AddEvent(string eventName, Action callback)
        {
            if (_animationState != null)
            {
                if (string.IsNullOrEmpty(eventName))
                {
                    _animationState.Event += (trackEntry, evt) => { callback?.Invoke(); };
                }
                else
                {
                    var eventData = _skeletonAnimation.Skeleton.Data.FindEvent(eventName);
                    if (eventData != null)
                    {
                        _animationState.Event += (trackEntry, evt) =>
                        {
                            if (evt.Data == eventData)
                            {
                                callback?.Invoke();
                            }
                        };
                    }
                    else
                    {
                        LogWarning($"Event \"{eventName}\" not found!");
                    }
                }
            }
            else
            {
                LogWarning($"Can't add event \"{eventName}\"!");
            }
        }

        public override void AddEvent(string eventName, Action<int> callback)
        {
            if (_animationState != null)
            {
                if (string.IsNullOrEmpty(eventName))
                {
                    _animationState.Event += (trackEntry, evt) => { callback?.Invoke(evt.Int); };
                }
                else
                {
                    var eventData = _skeletonAnimation.Skeleton.Data.FindEvent(eventName);
                    if (eventData != null)
                    {
                        _animationState.Event += (trackEntry, evt) =>
                        {
                            if (evt.Data == eventData)
                            {
                                callback?.Invoke(evt.Int);
                            }
                        };
                    }
                    else
                    {
                        LogWarning($"Event \"{eventName}\" not found!");
                    }
                }
            }
            else
            {
                LogWarning($"Can't add event \"{eventName}\"!");
            }
        }

        public override void ForcePlayAnimation(string name)
        {
            if (_skeletonAnimation != null)
            {
                var animation = SkeletonData.FindAnimation(name);
                if (animation != null)
                {
                    var trackEntry = _animationState.SetAnimation(0, animation, false);
                    trackEntry.TrackTime = animation.Duration;
                }
            }
            else
            {
                LogCannotForcePlayAnimation(name);
            }
        }
    }

    public class SkeletonAnimationController : SpineAnimationController
    {
        private SkeletonAnimation _skeletonAnimation2;

        protected override SkeletonData SkeletonData => _skeletonAnimation2.SkeletonDataAsset.GetSkeletonData(false);

        public SkeletonAnimationController(GameObject gameObject, SkeletonAnimation skeletonAnimation)
        {
            _gameObject = gameObject;
            _skeletonAnimation2 = skeletonAnimation;
            Construct(skeletonAnimation, skeletonAnimation.AnimationState);
        }

        public override void SetSpeed(float speed)
        {
            if (_skeletonAnimation2 != null)
            {
                _skeletonAnimation2.timeScale = speed;
            }
            else
            {
                LogCannotSetSpeed(speed);
            }
        }
    }


    public class SkeletonGraphicController : SpineAnimationController
    {
        private SkeletonGraphic _skeletonGraphic;

        protected override SkeletonData SkeletonData => _skeletonGraphic.SkeletonData;

        public SkeletonGraphicController(GameObject gameObject, SkeletonGraphic skeletonGraphic)
        {
            _gameObject = gameObject;
            _skeletonGraphic = skeletonGraphic;
            Construct(skeletonGraphic, skeletonGraphic.AnimationState);
        }

        public override void SetSpeed(float speed)
        {
            if (_skeletonGraphic != null)
            {
                _skeletonGraphic.timeScale = speed;
            }
            else
            {
                LogCannotSetSpeed(speed);
            }
        }
    }
#endif

    public static class AnimationController
    {
        public static IAnimationController Create(GameObject gameObject, bool skipNull = false)
        {
            IAnimationController controller = null;
            gameObject.BrowseChildren(go =>
            {
                controller = go.GetComponent<IAnimationController>();
                if (controller != null)
                {
                    return false;
                }

                var animator = go.GetComponent<Animator>();
                if (animator != null)
                {
                    controller = new AnimatorAnimationController(gameObject, animator);
                    return false;
                }
#if SPINE_ENABLED
                var skeletonAnimation = go.GetComponent<SkeletonAnimation>();
                if (skeletonAnimation != null)
                {
                    controller = new SkeletonAnimationController(gameObject, skeletonAnimation);
                    return false;
                }

                var skeletonGraphic = go.GetComponent<SkeletonGraphic>();
                if (skeletonGraphic != null)
                {
                    controller = new SkeletonGraphicController(gameObject, skeletonGraphic);
                    return false;
                }
#endif

                return true;
            });

            if (controller == null && !skipNull)
            {
                controller = new DefaultAnimationController(gameObject);
            }

            return controller;
        }
    }

    public static partial class ExtensionMethods
    {
        public static IAnimationController CreateAnimationController(this GameObject gameObject)
        {
            return AnimationController.Create(gameObject);
        }
    }
}