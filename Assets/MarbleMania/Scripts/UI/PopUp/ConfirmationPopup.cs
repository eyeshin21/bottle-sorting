using System;
using Anvil;
using TMPro;
using UnityEngine;

namespace Anvil
{
    public class ConfirmationPopup : Popup
    {
        [SerializeField] protected TMP_Text messageText;
        [SerializeField] protected TMP_Text confirmButtonText;
        [SerializeField] protected TMP_Text cancelButtonText;
        [SerializeField] protected UIButton confirmButton;
        [SerializeField] protected UIButton cancelButton;
        
        protected override void Awake()
        {
            base.Awake();
            if (confirmButton != null)
            {
                confirmButton.AddListener(OnConfirm);
            }
            if (cancelButton != null)
            {
                cancelButton.AddListener(OnCancel);
            }
        }

        public void SetMessage(string message)
        {
            if (message.IsNullOrEmpty()) return;
            messageText?.SetText(message);
        }
        public void SetConfirmButtonText(string text)
        {
            if (text.IsNullOrEmpty()) return;
            confirmButtonText?.SetText(text);
        }
        public void SetCancelButtonText(string text)
        {
            if (text.IsNullOrEmpty()) return;
            cancelButtonText?.SetText(text);
        }
        protected override void OnTouchOutBound()
        {
            OnCancel();
        }

        protected override void OnClickClose()
        {
            OnCancel();
        }

        Action<bool> _confirmationCallback;
        public virtual void Show(Action<bool> callback)
        {
            _confirmationCallback = callback;
            base.Show();
        }

        protected override void OnHidden()
        {
            base.OnHidden();
            GameObjectPool.RemoveObject(gameObject);
        }

        private void OnCancel()
        {
            Hide(() =>
            {
                InvokeCallback(false);
            });
        }

        protected virtual void OnConfirm()
        {
            Hide(() =>
            {
                InvokeCallback(true);
            });
        }
        protected void InvokeCallback(bool result)
        {
            var callback = _confirmationCallback;
            _confirmationCallback = null;
            callback?.Invoke(result);
        }
    }
}