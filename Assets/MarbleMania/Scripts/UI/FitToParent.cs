using System;
using UnityEngine;

namespace Anvil
{
    [RequireComponent(typeof(RectTransform))]
    public class FitToParent : MonoBehaviour
    {
        private void OnEnable()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}