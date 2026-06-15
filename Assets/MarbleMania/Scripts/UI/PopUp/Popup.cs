using System;
using Anvil;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Anvil
{
    [RequireComponent(typeof(RectTransform))]
    public class Popup : MonoBehaviour
    {
        [FormerlySerializedAs("_outBoundButton")] [SerializeField] private UIButton outBoundButton;
        [FormerlySerializedAs("_closeButton")] [SerializeField] protected UIButton closeButton;
        IAnimationController _animationController;

        [SerializeField] protected bool _canTouchOut = true;
        // [SerializeField] private bool _poolWhenHide = false;

        protected virtual void Awake()
        {
            _animationController = gameObject.CreateAnimationController();
            if (outBoundButton != null)
            {
                outBoundButton.AddListener(OnTouchOutBound);
            }
            if (closeButton != null)
            {
                closeButton.AddListener(OnClickClose);
            }
            gameObject.SetActive(false);
        }

        protected virtual void OnTouchOutBound()
        {
            if (_canTouchOut)
            {
                OnClickClose();
            }
        }
        protected virtual void OnClickClose()
        {
            Hide();
        }
        public void SetTouchOutBound(bool willClosePopUp)
        {
            _canTouchOut = willClosePopUp;
        }
        public virtual void Show(Action onShow = null)
        {
            // AudioManager.Instance.PlayEffect(AudioClipName.None);
            AudioClipName.ui_menu_button_confirm_05.PlaySound();
            _hideFunc = null;
            if (!gameObject.activeInHierarchy)
            {
                EnableSelf();
            }
            transform.SetAsLastSibling();
            PlayAnimation(AnimationNames.Show, onShow);
        }
        public virtual void Hide(Action callback = null)
        {
            if (!gameObject.activeInHierarchy)
            {
                callback?.Invoke();
                return;
            }
            PlayAnimation(AnimationNames.Hide, ()=>
            {
                OnHidden();
                callback?.Invoke();
            });
            //_confirmButton.RemoveAllListener();
            // AudioManager.Instance.PlayEffect(AudioClipName.None);
            AudioClipName.ui_menu_button_confirm_05.PlaySound();
        }

        private System.Func<bool> _hideFunc;

        public void OverrideHideFunc(Func<bool> hideFunc)
        {
            _hideFunc = hideFunc;
        }
        protected virtual void OnHidden()
        {
            if (_hideFunc != null && _hideFunc.Invoke())
                return;
            DisableSelf();
        }

        public virtual void PlayAnimation(string name)
        {
            _animationController?.PlayAnimation(name);
        }
        public virtual void PlayAnimation(string name, Action callback)
        {
            _animationController?.PlayAnimation(name, callback);
        }

        private void DisableSelf()
        {
            gameObject.SetActive(false);
        }
        private void EnableSelf()
        {
            gameObject.SetActive(true);
        }

#if UNITY_EDITOR
        [Button]
        protected void FindComponents()
        {
            var buttons = GetComponentsInChildren(typeof(UIButton), true);
            if (closeButton == null)
            {
                foreach (var button in buttons)
                {
                    if (button.name == "CloseButton")
                    {
                        closeButton = button as UIButton;
                        break;
                    }
                }
            }

            if (outBoundButton == null)
            {
                outBoundButton = transform.Find("ButtonCloseOutside")?.GetComponentInChildren<UIButton>();
            }
            
        }
#endif
    }
}
