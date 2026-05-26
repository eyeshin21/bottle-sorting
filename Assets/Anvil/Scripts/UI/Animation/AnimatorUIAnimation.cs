using System;
using Anvil.Legacy;
using UnityEngine;

namespace Anvil
{
    // public class AnimatorUIAnimation : MonoBehaviour, IUIAnimationController, IShowHideAnimation, IPlayAnimation
    public class AnimatorUIAnimation : MonoBehaviour, IUIAnimationController
    {
        [SerializeField] private Animator _animator;
        private AnimationEndEvent _animationEndEvent;
        private Action _callback;

        public string currentClipName
        {
            get
            {
                AnimatorClipInfo[] clipInfos = _animator.GetCurrentAnimatorClipInfo(0);
                if (clipInfos.Length > 0)
                {
                    return clipInfos[0].clip.name;
                }

                Debug.LogError("failed to get current animtor clip name");
                return "";
            }
        }

        private void Awake()
        {
            _animator = gameObject.TryGetComponent<Animator>();
        }

        public void PlayAnimation(string name)
        {
            //PlayAnimation(name, null);
            if (_animator != null)
            {
                _animator.Play(name, 0, 0);
            }
            else
            {
                Debug.Log($"[UIAnimator] cannot play animation {name}");
            }
        }

        public void PlayAnimation(string name, Action callback)
        {
            if (callback == null)
            {
                PlayAnimation(name);
                return;
            }
            // AppendCallback(callback);

            if (_animator != null && _animator.runtimeAnimatorController != null)
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
                    // Debug.Log($"animator inactive");
                    //LogInactive(_animator.gameObject);

                    callback?.Invoke();
                    // OnAnimationEnd();
                }
            }
            else
            {
                // Debug.Log($"[UIAnimator] cannot play animation {name}");
                callback?.Invoke();
                // OnAnimationEnd();
            }
        }

        private void OnAnimationEnd()
        {
            Debug.Log("animation end callback");
            _callback?.Invoke();
            _callback = null;
        }

        private void ReplaceCallback(Action callback)
        {
            _callback = callback;
        }

        public void AppendCallback(Action callback)
        {
            if (_callback == null)
            {
                _callback = callback;
                return;
            }

            _callback += callback;
        }
        public void PlayShowAnimation(Action callback = null)
        {
            PlayAnimation(AnimationNames.Show, callback);
        }

        public void PlayHideAnimation(Action callback = null)
        {
            PlayAnimation(AnimationNames.Hide, callback);
        }
        public virtual void OnShow(Action onComplete = null)
        {
            PlayShowAnimation(onComplete);
        }

        public virtual void OnHide(Action onComplete = null)
        {
            PlayHideAnimation(onComplete);
        }

        public void OnSetShow()
        {
            throw new NotImplementedException();
        }

        public void OnSetHide()
        {
            throw new NotImplementedException();
        }
    }
}
