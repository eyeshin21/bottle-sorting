using Anvil;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Anvil
{
    public class UILabledIconButton : FaceButton, ILabeledUIButton, IIconedUIButton
    {
        
        Anvil.IAnimationController _animationController;
        private Animator _animator;
        TextAdapter _textAdapter = null;
        [SerializeField] private GameObject _textObject;

        [FormerlySerializedAs("_displayICon")] [FormerlySerializedAs("_displayImage")] [SerializeField]
        private Image _displayIcon;
        

        [SerializeField] private bool _allowInput = true;

        public bool AllowInput
        {
            get { return _allowInput; }
            set { _allowInput = value; }
        }

        protected string pressAnimation = AnimationNames.Press;

        List<Action> callbacks = new List<Action>();

        public string PressAnim
        {
            set { pressAnimation = value; }
        }

        public Animator animator => _animator;

        protected TextAdapter TextAdapter
        {
            get
            {
                if (_textAdapter == null)
                {
                    _textAdapter = TextAdapter.Create(_textObject != null ? _textObject : gameObject);
                }

                return _textAdapter;
            }
        }

        public void SetNativeSize()
        {
            foreach (Image image in GetComponentsInChildren<Image>())
            {
                image.SetNativeSize();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _animationController = gameObject.CreateAnimationController();
            _animator = GetComponent<Animator>();

        }

        protected override bool OnClick()
        {
            if (!AllowInput)
            {
                return false;
            }

            if (pressAnimation != AnimationNames.Default)
            {
                PlayAnimation(pressAnimation);
            }

            // AudioManager.Instance.PlayEffect(AudioClipName.ElementClick);
            AudioClipName.ui_menu_button_confirm_05.PlaySound();
            if (callbacks.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < callbacks.Count; i++)
            {
                Action callback = callbacks[i];
                if (callback != null)
                {
                    callback();
                }
            }
            _onClickAction?.Invoke();
            //foreach (var callback in callbacks)
            //{
            //    callback();
            //}
            //callbacks.Clear();
            return true;
        }

        public override void AddListener(Action callback)
        {
            callbacks.Add(callback);
        }

        public override void RemoveListener(Action callback)
        {
            callbacks.Remove(callback);
            _onClickAction -= callback;
        }

        public void PlayAnimation(string animName)
        {
            _animationController.PlayAnimation(animName);
        }

        public void PlayAnimation(string animName, Action callback)
        {
            _animationController.PlayAnimation(animName, callback);
        }

        public void SetLabel(string text)
        {
            TextAdapter.Text = text;
            return;
        }

        public void SetLabel(object obj)
        {
            SetLabel(obj.ToString());
        }

        public void SetPrice(string text)
        {
            TextAdapter.Text = "$" + text;
            return;
        }

        public void SetIcon(Sprite sprite)
        {
            if (_displayIcon != null)
            {
                _displayIcon.enabled = true;
                _displayIcon.sprite = sprite;
                // _displayIcon.SetNativeSize();
                return;
            }

            _displayIcon.enabled = false;
        }


        public void RemoveAllListener()
        {
            //_button.onClick.RemoveAllListeners();
            //_button.onClick.AddListener(OnClick);
            callbacks.Clear();
        }

        protected void DisableSelf()
        {
            gameObject.SetActive(false);
        }

        protected void EnableSelf()
        {
            gameObject.SetActive(true);
        }
    }
}