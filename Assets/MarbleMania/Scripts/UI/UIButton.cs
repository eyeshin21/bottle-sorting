using System;
using Anvil;
using UnityEngine;
using UnityEngine.UI;

namespace Anvil
{
    public interface IUIButton
    {
        public GameObject gameObject { get; }
        void SetInteractable(bool interactable);
        
        void AddListener(Action callback);
        void RemoveListener(Action callback);
    }
    public interface ILabeledUIButton : IUIButton
    {
        void SetLabel(string label);
    }
    public interface IIconedUIButton : IUIButton
    {
        void SetIcon(Sprite icon);
    }
    public interface IAnimatedUIButton : IUIButton
    {
        void PlayAnimation(string AnimationNames);
    }
    public interface IToggleUIButton : IUIButton
    {
        void Switch();
        void SetIsOn(bool isOn);
        bool GetIsOn();
    }
    
    public class UIButton : MonoBehaviour, IUIButton
    {
        [SerializeField] protected Button _button;
        [SerializeField] protected float _doubleClickBlockDuration = 0.2f;

        protected float _clickCoolDown = 0;
        protected Action _onClickAction;
        protected virtual void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(()=>
            {
                OnClick();
            });
        }

        protected virtual bool OnClick()
        {
            //  PlayAnimation(AnimationNames.ButtonPress);
            if (_clickCoolDown > 0)
            {
                return false;
            }
            _clickCoolDown = _doubleClickBlockDuration;
            
            AudioClipName.ui_menu_button_confirm_05.PlaySound();
            _onClickAction?.Invoke();
            return true;
        }

        public virtual void AddListener(Action callback)
        {
            _onClickAction += callback;
        }
        public virtual void AddListenerOnce(Action callback)
        {
            _onClickAction += Wrapper;
            return;

            void Wrapper()
            {
                callback?.Invoke();
                RemoveListener(Wrapper);
            }
        }
        public virtual void ClearListeners()
        {
            _onClickAction = null;
        }
        public virtual void RemoveListener(Action callback)
        {
            _onClickAction -= callback;
        }
        public void SetInteractable(bool interactable)
        {
            if (_button != null)
            {
                _button.interactable = interactable;
            }
        }

        public void SimulateClick()
        {
            OnClick();
        }
        protected virtual void Update()
        {
            if (_clickCoolDown > 0)
            {
                _clickCoolDown -= Time.deltaTime;
            }
        }
    }

    public static class UIButtonExtensions
    {
        public static void AddListenerSafe(this GameObject gameObject, Action callback)
        {
            if (gameObject == null || callback == null)
            {
                return;
            }
            var button = gameObject.GetComponent<IUIButton>();
            if (button == null)
            {
                return;
            }
            button.AddListener(callback);
        }
        public static void AddListenerSafe(this IUIButton button, Action callback)
        {
            if (button == null || callback == null)
            {
                return;
            }
            button.AddListener(callback);
        }
        public static void SetLabelSafe(this IUIButton button, string label)
        {
            if (button == null)
            {
                return;
            }

            if (button is ILabeledUIButton labeledbutton)
            {
                labeledbutton.SetLabel(label);
            }
        }
        public static void PlayClickAnimation(this IUIButton button)
        {
            if (button == null)
            {
                return;
            }

            if (button is IAnimatedUIButton animatedButton)
            {
                animatedButton.PlayAnimation(AnimationNames.Press);
            }
        }
    }
}