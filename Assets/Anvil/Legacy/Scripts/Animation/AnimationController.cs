#if DEBUG_MODE
#define DEBUG_ANIMATION_CONTROLLER
#endif

// #define SPINE_ENABLED
#if SPINE_ENABLED
using Spine;
using Spine.Unity;
#endif

using UnityEngine;
using System;
using System.Diagnostics;
using Animation = UnityEngine.Animation;
using AnimationState = UnityEngine.AnimationState;
using Debug = UnityEngine.Debug;


namespace Anvil.Legacy
{
    public interface IAnimationController
    {
        GameObject gameObject { get; }
        void PlayAnimation(string name);
        void PlayAnimation(string name, bool loop);
        void PlayAnimation(string name, Action callback);
        void PlayAnimations(string name, string name2, bool loop = false);
        void PlayAnimations(string name, string name2, string name3, bool loop = false);
        void AddEvent(string eventName, Action callback);
        void AddEvent(string eventName, Action<int> callback);
        /// <summary>
        /// Play to end of animation.
        /// </summary>
        void ForcePlayAnimation(string name);
        void ForceEndAnimation();
        void SetSpeed(float speed);
        void SetShow(bool show);
        void SetPosition(Vector3 pos);
        bool GetAnimationLength(string animationName, out float length);
    }

    public abstract class BaseAnimationController : IAnimationController
    {
        protected GameObject _gameObject;

        public virtual GameObject gameObject => _gameObject;
        public abstract void PlayAnimation(string name);
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
        public virtual void ForceEndAnimation()
        {
            LogCannotForceEndAnimation();
        }
        public virtual void SetSpeed(float speed)
        {
            LogCannotSetSpeed(speed);
        }
        public virtual void SetShow(bool show)
        {
            gameObject.SetShow(show);
        }
        public virtual void SetPosition(Vector3 pos)
        {
            if (gameObject != null)
            {
                gameObject.transform.position = pos;
            }
        }

        public virtual bool GetAnimationLength(string animName, out float length)
        {
            UnityEngine.Debug.LogError("Get current animation length not implemented");
            length = 0;
            return false;
        }

        [Conditional("DEBUG_ANIMATION_CONTROLLER")]
        protected static void LogWarning(string message)
        {
            LegacyLog.Warning(message);
        }

        [Conditional("DEBUG_ANIMATION_CONTROLLER")]
        protected static void LogCannotPlayAnimation(string name)
        {
            LegacyLog.Warning($"Can't play animation \"{name}\"!");
        }

        [Conditional("DEBUG_ANIMATION_CONTROLLER")]
        protected static void LogCannotPlayLoopAnimation(string name)
        {
            LegacyLog.Warning($"Can't play loop animation \"{name}\"!");
        }

        [Conditional("DEBUG_ANIMATION_CONTROLLER")]
        protected static void LogCannotPlayAnimations(string name, string name2)
        {
            LegacyLog.Warning($"Can't play animation \"{name}\" and \"{name2}\"!");
        }

        [Conditional("DEBUG_ANIMATION_CONTROLLER")]
        protected static void LogCannotPlayAnimations(string name, string name2, string name3)
        {
            LegacyLog.Warning($"Can't play animation \"{name}\", \"{name2}\" and \"{name3}\"!");
        }

        [Conditional("DEBUG_ANIMATION_CONTROLLER")]
        protected static void LogCannotForcePlayAnimation(string name)
        {
            LegacyLog.Warning($"Can't force play animation \"{name}\"!");
        }

        [Conditional("DEBUG_ANIMATION_CONTROLLER")]
        protected static void LogCannotForceEndAnimation()
        {
            LegacyLog.Warning($"Can't force end animation!");
        }

        [Conditional("DEBUG_ANIMATION_CONTROLLER")]
        protected static void LogCannotSetSpeed(float speed)
        {
            LegacyLog.Warning($"Can't set speed {speed}!");
        }

        [Conditional("DEBUG_ANIMATION_CONTROLLER")]
        protected static void LogInactive(GameObject gameObject)
        {
            LegacyLog.Warning($"GameObject {gameObject.name} is inactive!");
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

    public class AnimatorAnimationController : BaseAnimationController
    {
        private Animator _animator;
        private AnimationEndEvent _animationEndEvent;

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

        public override void PlayAnimation(string name, bool loop)
        {
            //TODO: Check loop
            PlayAnimation(name);
        }

        public override void PlayAnimation(string name, Action callback)
        {
            if (callback == null)
            {
                PlayAnimation(name);
                return;
            }

            if (_animator != null)
            {
                if (_animator.gameObject.activeInHierarchy)
                {
                    if (_animationEndEvent == null)
                    {
                        _animationEndEvent = _animator.gameObject.AddComponent<AnimationEndEvent>();
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

        public override void ForceEndAnimation()
        {
            if (_animator != null)
            {
                //_animator.ForceStateNormalizedTime(1);
                _animator.Update(1000000);
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

        public override bool GetAnimationLength(string animName, out float length)
        {
            length = 0;
            AnimationClip[] allClip = _animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in allClip)
            {
                if (clip.name == animName)
                {
                    length = clip.length;
                    return true;
                }
            }

            return false;
            // stateInfo.
        }
    }
    public class LegacyAnimationController : BaseAnimationController
    {
        private Animation _animationComponent;
        private AnimationEndEvent _animationEndEvent;

        public LegacyAnimationController(GameObject gameObject, Animation animationComponent)
        {
            _gameObject = gameObject;
            _animationComponent = animationComponent;
        }

        public override void PlayAnimation(string name)
        {
            if (_animationComponent == null)
            {
                LogCannotPlayAnimation(name);
            }
            if (!_animationComponent.gameObject.activeInHierarchy)
            {
                LogInactive(_animationComponent.gameObject);
            }

            AnimationClip clip = _animationComponent.GetClip(name);
            if (clip == null)
            {
                LogCannotPlayAnimation(name);
                return;
            }
            _animationComponent.Play(name);
        }

        public override void PlayAnimation(string name, bool loop)
        {
            //TODO: Check loop
            Debug.LogWarning("Loop not supported for legacy animation. Specify loopable in animation clip instead");
            PlayAnimation(name);
        }

        public override void PlayAnimation(string name, Action callback)
        {
            if (callback == null)
            {
                PlayAnimation(name);
                return;
            }

            if (_animationComponent != null)
            {
                if (_animationComponent.gameObject.activeInHierarchy)
                {
                    if (_animationEndEvent == null)
                    {
                        _animationEndEvent = _animationComponent.gameObject.AddComponent<AnimationEndEvent>();
                    }
                    _animationEndEvent.PlayAnimation(name, callback);
                }
                else
                {
                    LogInactive(_animationComponent.gameObject);
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
            if (_animationComponent != null)
            {
                _animationComponent.Play(name);
                // _animationComponent.Update(0.02f);
            }
            else
            {
                LogCannotForcePlayAnimation(name);
            }
        }

        public override void ForceEndAnimation()
        {
            if (_animationComponent != null)
            {
                //_animator.ForceStateNormalizedTime(1);
                _animationComponent.Stop();
            }
        }

        public override void SetSpeed(float speed)
        {
            if (_animationComponent != null)
            {
                foreach (AnimationState state in _animationComponent)
                {
                    bool playing = _animationComponent.IsPlaying(state.clip.name);
                    if (playing)
                    {
                        state.speed = speed;
                    }
                }
                // _animationComponent.get = speed;
            }
            else
            {
                LogCannotSetSpeed(speed);
            }
        }

        public override bool GetAnimationLength(string animName, out float length)
        {
            length = 0;
            AnimationClip clip = _animationComponent.GetClip(animName);
            if (clip == null)
            {
                return false;
            }

            length = clip.length;
            return true;
        }
    }

#if SPINE_ENABLED

    public abstract class SpineAnimationController : BaseAnimationController
    {
        protected ISkeletonAnimation _skeletonAnimation;
        protected Spine.AnimationState _animationState;

        protected abstract SkeletonData SkeletonData { get; }

        protected void Construct(ISkeletonAnimation skeletonAnimation, Spine.AnimationState animationState)
        {
            _skeletonAnimation = skeletonAnimation;
            _animationState = animationState;
        }
        protected virtual void ForceUpdateAnimation(float deltaTime = 0)
        {

        }

        public override void PlayAnimation(string name)
        {
            if (_animationState != null)
            {
                _animationState.SetAnimation(0, name, false);
                ForceUpdateAnimation();
                //_animationState.Update(0);
                //_animationState.LateUpdate();
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
                ForceUpdateAnimation();
            }
            else
            {
                LogCannotPlayLoopAnimation(name);
            }
        }

        public override void PlayAnimation(string name, Action callback)
        {
            if (_animationState != null)
            {
                var trackEntry = _animationState.SetAnimation(0, name, false);
                trackEntry.Complete += (trackEntry2) => callback?.Invoke();
                ForceUpdateAnimation();
            }
            else
            {
                LogCannotPlayAnimation(name);
                callback?.Invoke();
            }
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
                    _animationState.Event += (trackEntry, evt) =>
                    {
                        callback?.Invoke();
                    };
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
                    _animationState.Event += (trackEntry, evt) =>
                    {
                        callback?.Invoke(evt.Int);
                    };
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

        public override bool GetAnimationLength(string animName, out float length)
        {
            length = 0;
            var animation = _skeletonAnimation.Skeleton.Data.FindAnimation(animName);
            if (animation == null)
            {
                return false;
            }

            length = animation.Duration;
            return true;
        }
    }

    public class SkeletonAnimationController : SpineAnimationController
    {
        private SkeletonAnimation _skeletonAnimation2;

        protected override SkeletonData SkeletonData => _skeletonAnimation2.SkeletonDataAsset.GetSkeletonData(false);
        protected override void ForceUpdateAnimation(float deltaTime = 0)
        {
            _skeletonAnimation2.Update(deltaTime);
        }
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
        protected override void ForceUpdateAnimation(float deltaTime = 0)
        {
            _skeletonGraphic.Update(deltaTime);
        }
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
        public static void PlayAnimation(GameObject gameObject, string animationName, Action callback= null)
        {
            gameObject.BrowseChildren(go =>
            {
                var controller = go.GetComponent<IAnimationController>();
                if (controller != null)
                {
                    controller.PlayAnimation(animationName);
                    return false;
                }

                var animator = go.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.Play(animationName, 0, 0);
                    return false;
                }

                var animation = go.GetComponent<Animation>();
                if (animation != null)
                {
                    animation.Play(animationName);
                    return false;
                }

#if SPINE_ENABLED
                //TODO
                //var skeletonAnimation = go.GetComponent<SkeletonAnimation>();
                //if (skeletonAnimation != null)
                //{
                //    controller = new SkeletonAnimationController(gameObject, skeletonAnimation);
                //    return false;
                //}

                //var skeletonGraphic = go.GetComponent<SkeletonGraphic>();
                //if (skeletonGraphic != null)
                //{
                //    controller = new SkeletonGraphicController(gameObject, skeletonGraphic);
                //    return false;
                //}
#endif
                return true;
            });
        }

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

                var animation = go.GetComponent<Animation>();
                if (animation != null)
                {
                    controller = new LegacyAnimationController(gameObject, animation);
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

        internal static void SetAnimationSpeed(GameObject gameObject, float speed)
        {
            Debug.LogWarning("Set animation speed Not Supported on legacy Animation Controller");
        }
    }

    public static partial class ExtensionMethods
    {
        public static IAnimationController CreateAnimationController(this GameObject gameObject)
        {
            return AnimationController.Create(gameObject);
        }

        public static bool IsActive(this IAnimationController animationController)
        {
            if (animationController != null)
            {
                var gameObject = animationController.gameObject;
                return gameObject != null && gameObject.activeSelf;
            }
            return false;
        }

        public static void SetActive(this IAnimationController animationController, bool active)
        {
            if (animationController != null)
            {
                var gameObject = animationController.gameObject;
                if (gameObject != null)
                {
                    gameObject.SetActive(active);
                }
            }
        }
    }
}
