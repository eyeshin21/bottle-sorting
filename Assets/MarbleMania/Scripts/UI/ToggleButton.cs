using System;
using Anvil;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Anvil
{
    public class ToggleButton : FaceButton, IToggleUIButton, IAnimated
    {
        [FormerlySerializedAs("_allowInputSwitch")] [SerializeField] private bool allowStateSwitch = true;
        [SerializeField] private Sprite enableSprite;
        [SerializeField] private Sprite disableSprite;

        [SerializeField] private Color _enabledColor;
        [SerializeField] private Color _disabledColor;
        [SerializeField] private Image _coloredImage;
        
        [SerializeField] protected Anvil.IAnimationController _animationController;
        private Action<bool> _onToggleAction;
        public bool state = false;

        public bool AllowStateSwitch
        {
            get => allowStateSwitch;
            set => allowStateSwitch = value;
        }

        public void RegisterOnToggleAction(Action<bool> onToggleAction)
        {
            _onToggleAction += onToggleAction;
        }

        public void UnRegisterOnToggleAction(Action<bool> onToggleAction)
        {
            _onToggleAction -= onToggleAction;
        }
        protected override bool OnClick()   
        {
            if (!allowStateSwitch)
            {
                base.OnClick();
                return false;
            }
            
            if (_clickCoolDown > 0)
            {
                return false;
            }
            _clickCoolDown = _doubleClickBlockDuration;
            SetIsOn(!state);
            return true;
        }

        protected override void Awake()
        {
            base.Awake();
            UpdateStateRender();
            
            if (_animationController == null)
            {
                _animationController = AnimationController.Create(gameObject);
            }
        }

        public void SetEnableSprite(Sprite sprite)
        {
            if (sprite != null)
            {
                enableSprite = sprite;
            }
        }

        public void SetDisableSprite(Sprite sprite)
        {
            if (sprite != null)
            {
                disableSprite = sprite;
            }
        }

        public void SetColoredState(bool enabled)
        {
            if (_coloredImage == null) return;
            _coloredImage.color = enabled ? _enabledColor : _disabledColor;
        }

        public virtual void UpdateStateRender()
        {
            SetColoredState(state);
            if (state)
            {
                SetDisplayButton(enableSprite);
                return;
            }

            SetDisplayButton(disableSprite);
        }

        public void Switch()
        {
            OnClick();
        }

        public void SetIsOn(bool isOn)
        {
            state = isOn;
            AudioClipName.ui_menu_button_confirm_05.PlaySound();
            UpdateStateRender();
            PlayAnimation(AnimationNames.Press);
            _onClickAction?.Invoke();
            _onToggleAction?.Invoke(state);
        }

        public bool GetIsOn()
        {
            return state;
        }



        public void SwitchOff()
        {
            if (!state)
            {
                return;
            }
            else
            {
                Switch();
            }
        }

        public void ForceSwitchOff()
        {
            if (!state)
            {
                return;
            }

            state = false;
            UpdateStateRender();
        }

        public void ForceSwitchOn()
        {
            if (state)
            {
                return;
            }

            state = true;
            UpdateStateRender();
        }

        public void SwitchOn()
        {
            if (state)
            {
                return;
            }

            Switch();
        }

        public virtual void PlayAnimation(string animationName)
        {
            if (_animationController == null) return;
            
            _animationController.PlayAnimation(animationName);
        }
        public virtual void PlayAnimation(string animationName, System.Action callback)
        {
            if (_animationController == null)
            {
                callback?.Invoke();
                return;
            }
            
            _animationController.PlayAnimation(animationName, callback);
        }
    }
}