using System;
using Anvil;
using UnityEngine;
using EaseType = Anvil.EaseType;

namespace Anvil
{
    public class TutorialMask : MonoBehaviour
    {
        [SerializeField] private DiscreteObjectDynamicComponent _objectDynamic;
        
        private IAnimationController _animationController;

        private void Awake()
        {
            if (_objectDynamic == null)
            {
                _objectDynamic = GetComponent<DiscreteObjectDynamicComponent>();
            }
            if (_objectDynamic == null)
            {
                Debug.LogWarning("DiscreteObjectDynamicComponent is not assigned.");
            }
            _animationController = gameObject.CreateAnimationController();
            if (_animationController == null)
            {
                Debug.LogWarning("Failed to create AnimationController.");
            }
        }

        public EaseType _EaseType = EaseType.SineOut;
        public float duration = 4f;
        public float hideDuration = 1.2f;
        
        public void ShowMask(Vector3 position, float width, float height, Action callback = null, bool keepCurrentSize = false)
        {
            Vector3 originalPosition = keepCurrentSize? transform.position : transform.parent.position;
            transform.position = originalPosition;
            RectTransform rtf = transform as RectTransform;
            RectTransform parentRect = transform.parent as RectTransform;
            Vector2 initialSize = keepCurrentSize? rtf.rect.size : parentRect.rect.size;
            Vector2 finalSize = new Vector3(width, height, 0f);
            Vector3 scaleFactor = transform.lossyScale;
            // initialSize /= scaleFactor;
            finalSize /= scaleFactor;

            FloatController sineOutController = FloatController.CreateLerp(0f,1f,duration, _EaseType);
            // var sizeAction = new CallFuncFloat();
            // sizeAction.Construct(sineOutController, t =>
            // {
            //     Vector2 size = Vector2.Lerp(initialSize, finalSize, t);
            //     rtf.sizeDelta = size;
            //     transform.position = Vector3.Lerp(originalPosition, position, t);
            // });
            // var sequence = Sequence.Create(sizeAction, ()=>
            // {
            //     callback?.Invoke();
            // });
            //
            // gameObject.PlayAction(sequence);
            // // Debug.Log("playing sequene");
        }
        public void HideMask(Action callback = null)
        {
            Vector3 originalPosition = transform.position;
            Vector3 finalPosition = transform.parent.position;
            RectTransform rtf = transform as RectTransform;
            RectTransform parentRect = transform.parent as RectTransform;
            Vector2 finalSize = parentRect.rect.size;
            Vector2 initialSize = rtf.rect.size;
            Vector3 scaleFactor = transform.lossyScale;

            FloatController sineOutController = FloatController.CreateLerp(0f,1f,hideDuration, _EaseType);
            // var sizeAction = new CallFuncFloat();
            // sizeAction.Construct(sineOutController, t =>
            // {
            //     Vector2 size = Vector2.Lerp(initialSize, finalSize, t);
            //     rtf.sizeDelta = size;
            //     transform.position = Vector3.Lerp(originalPosition, finalPosition, t);
            // });
            // var sequence = Sequence.Create(sizeAction, () => {callback?.Invoke();});
            //
            // gameObject.PlayAction(sequence);
        }
    }
}