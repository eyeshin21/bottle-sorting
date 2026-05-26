using UnityEngine;

namespace Anvil.Legacy
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        void Awake()
        {
#if UNITY_EDITOR
            if (name == "GameObject")
            {
                name = "SafeArea";
                var rectTransform = GetComponent<RectTransform>();
                rectTransform.SetStretch();
            }
#endif
            UpdateArea();
        }

        public void UpdateArea()
        {
            var rectTransform = GetComponent<RectTransform>();
            var safeArea = Screen.safeArea;
            var minAnchor = safeArea.position;
            var maxAnchor = minAnchor + safeArea.size;
            //Debug.Log($"safeArea={safeArea}, minAnchor={minAnchor}, maxAnchor={maxAnchor}");

            float xFactor = 1f / Screen.width;
            float yFactor = 1f / Screen.height;
            minAnchor.x *= xFactor;
            minAnchor.y *= yFactor;
            maxAnchor.x *= xFactor;
            maxAnchor.y *= yFactor;

            rectTransform.anchorMin = minAnchor;
            rectTransform.anchorMax = maxAnchor;
        }
    }
}