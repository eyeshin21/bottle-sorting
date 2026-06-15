using System;
using Anvil;
using UnityEngine;

namespace Anvil
{
    [RequireComponent(typeof(RectTransform))]
    public class MessagePopup : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI messageText;
        private IAnimationController _animationController;

        private void Awake()
        {
            _animationController = gameObject.CreateAnimationController();
        }

        public void Show(Direction direction, Action callback = null)
        {
            string animName = $"{AnimationNames.Show}_{direction}";
            PlayAnimation(animName, callback);
        }
        public void Show(Action callback = null)
        {
            PlayAnimation(AnimationNames.Show, callback);
        }

        public void Hide(Action callback = null)
        {

            PlayAnimation(AnimationNames.Hide, () =>
            {
                callback?.Invoke();
                gameObject.SetActive(false);
            });
        }
        public void SetMessage(string message)
        {
            if (messageText != null)
            {
                messageText.text = message;
            }
        }

        public void SetPosition(Vector3 position, TextAnchor popupAnchor, float padding = 0)
        {
            RectTransform rectTransform = transform as RectTransform;
            Vector2 pivot = Vector2.zero;
            Vector3 offset = Vector3.zero;
            switch (popupAnchor)
            {
                case TextAnchor.UpperCenter:
                    pivot = new Vector2(0, 1);
                    offset = new Vector3(0, - padding, 0);
                    break;
                case TextAnchor.MiddleCenter:
                    pivot = new Vector2(0.5f, 0.5f);
                    break;
                case TextAnchor.LowerCenter:
                    offset = new Vector3(0, padding, 0);
                    pivot = new Vector2(0.5f, 0);
                    break;
                case TextAnchor.UpperLeft:
                    pivot = new Vector2(0, 1);
                    break;
                case TextAnchor.MiddleLeft:
                    pivot = new Vector2(0, 0.5f);
                    break;
                case TextAnchor.LowerLeft:
                    pivot = new Vector2(0, 0);
                    break;
                case TextAnchor.UpperRight:
                    pivot = new Vector2(1, 1);
                    break;
                case TextAnchor.MiddleRight:
                    pivot = new Vector2(1, 0.5f);
                    break;
                case TextAnchor.LowerRight:
                    pivot = new Vector2(1, 0);
                    break;
            }
            rectTransform.pivot = pivot;
            rectTransform.position = position + offset;
        }
        private void PlayAnimation(string animationName, Action callback = null)
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