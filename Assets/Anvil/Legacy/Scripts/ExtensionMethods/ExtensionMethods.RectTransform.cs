using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        static readonly Vector2 AnchorTopLeft = new Vector2(0, 1);

        /// <summary>
        /// Returns width in pixels.
        /// </summary>
        public static float GetWidth(this RectTransform rectTransform)
        {
            return rectTransform != null ? rectTransform.rect.width : 0;
        }

        /// <summary>
        /// Returns height in pixels.
        /// </summary>
        public static float GetHeight(this RectTransform rectTransform)
        {
            return rectTransform != null ? rectTransform.rect.height : 0;
        }

        /// <summary>
        /// Returns size in pixels.
        /// </summary>
        public static void GetSize(this RectTransform rectTransform, out float width, out float height)
        {
            if (rectTransform != null)
            {
                //var size = rectTransform.sizeDelta;
                var size = rectTransform.rect.size;
                var scale = rectTransform.localScale;
                width = size.x * scale.x;
                height = size.y * scale.y;
            }
            else
            {
                width = height = 0;
            }
        }

        /// <summary>
        /// Sets size in pixels. (sizeDelta)
        /// </summary>
        public static void SetSize(this RectTransform rectTransform, float width, float height)
        {
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(width, height);
            }
        }

        /// <summary>
        /// Sets size in pixels. (sizeDelta)
        /// </summary>
        public static void SetSize(this RectTransform rectTransform, Vector2 size)
        {
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = size;
            }
        }

        public static void SetAnchorTopLeft(this RectTransform rectTransform)
        {
            if (rectTransform != null)
            {
                rectTransform.anchorMin = rectTransform.anchorMax = AnchorTopLeft;
            }
        }

        public static T GetOrAddComponent<T>(this RectTransform rectTransform) where T : Component
        {
            if (rectTransform != null)
            {
                var component = rectTransform.GetComponent<T>();
                if (component == null)
                {
                    component = rectTransform.gameObject.AddComponent<T>();
                }
                return component;
            }
            return null;
        }

        public static RectTransform CreateChild(this RectTransform rectTransform, string name)
        {
            if (rectTransform != null)
            {
                var child = new GameObject(name).AddComponent<RectTransform>();
                child.SetParent(rectTransform);
                child.localPosition = Vector3.zero;
                child.localScale = Vector3.one;
                return child;
            }
            return default;
        }

        public static GameObject CreateChild(this RectTransform rectTransform, GameObject prefab, bool worldPositionStays = false)
        {
            if (rectTransform != null && prefab != null)
            {
                var child = GameObject.Instantiate(prefab, rectTransform, worldPositionStays);
                FixName(child);
                return child;
            }
            return default;
        }

        public static RectTransform SetStretch(this RectTransform rectTransform)
        {
            if (rectTransform != null)
            {
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
            }
            return rectTransform;
        }

        public static RectTransform SetTop(this RectTransform rectTransform)
        {
            if (rectTransform != null)
            {
                rectTransform.pivot = new Vector2(0.5f, 1);
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = Vector2.one;
                rectTransform.offsetMin = new Vector2(0, -rectTransform.sizeDelta.y);
                rectTransform.offsetMax = Vector2.zero;
            }
            return rectTransform;
        }

        public static RectTransform SetBottom(this RectTransform rectTransform)
        {
            if (rectTransform != null)
            {
                rectTransform.pivot = new Vector2(0.5f, 0);
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = new Vector2(1, 0);
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = new Vector2(0, rectTransform.sizeDelta.y);
            }
            return rectTransform;
        }

        public static void SetPositionTopLeft(this RectTransform rectTransform, float paddingLeft, float paddingTop)
        {
            if (rectTransform != null)
            {
                var size = rectTransform.sizeDelta;
                rectTransform.anchorMin = rectTransform.anchorMax = new Vector2(0, 1);
                rectTransform.offsetMin = new Vector2(paddingLeft, -paddingTop - size.y);
                rectTransform.offsetMax = new Vector2(paddingLeft + size.x, -paddingTop);
            }
        }

        public static RectTransform SetPlayTopLeft(this RectTransform rectTransform, float paddingLeft = 100, float paddingTop = 100, float width = 100, float height = 100)
        {
            if (rectTransform != null)
            {
                rectTransform.pivot = new Vector2(0, 1);
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.zero;
                rectTransform.offsetMin = new Vector2(paddingLeft, -paddingTop - height);
                rectTransform.offsetMax = new Vector2(paddingLeft + width, -paddingTop);
            }
            return rectTransform;
        }

        public static RectTransform SetPlayBottomRight(this RectTransform rectTransform, float paddingRight = 100, float paddingBottom = 100, float width = 100, float height = 100)
        {
            if (rectTransform != null)
            {
                rectTransform.pivot = new Vector2(1, 0);
                rectTransform.anchorMin = Vector2.one;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.offsetMin = new Vector2(-paddingRight - width, paddingBottom);
                rectTransform.offsetMax = new Vector2(-paddingRight, paddingBottom + height);
            }
            return rectTransform;
        }

        public static void SetHeight(this RectTransform rectTransform, float height)
        {
            var sizeDelta = rectTransform.sizeDelta;
            sizeDelta.y = height;
            rectTransform.sizeDelta = sizeDelta;
        }

        public static void ForceUpdate(this RectTransform rectTransform)
        {
            if (rectTransform != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            }
        }

        static Vector3[] _corners = new Vector3[4];

        public static AABB GetAABB(this RectTransform rectTransform)
        {
            if (rectTransform != null)
            {
                GetAABB(rectTransform, out float left, out float top, out float right, out float bottom);
                return new AABB(left, top, right, bottom);
            }
            return AABB.Default;
        }

        public static void GetAABB(this RectTransform rectTransform, out float left, out float top, out float right, out float bottom)
        {
            rectTransform.GetWorldCorners(_corners);
            left = _corners[0].x;
            bottom = _corners[0].y;
            right = _corners[2].x;
            top = _corners[2].y;
        }

        public static void GetOverlayParams(this RectTransform rectTransform, out Vector3 pos, out float width, out float height)
        {
            GetAABB(rectTransform, out float left, out float top, out float right, out float bottom);
            pos = new Vector3((left + right) * 0.5f, (bottom + top) * 0.5f);
            width = right - left;
            height = top - bottom;
        }

        public static void ForceLayout(this RectTransform rectTransform)
        {
            if (rectTransform != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            }
        }

        public static bool IsPointInside(this RectTransform rt,Vector3 point)
        {
            Rect rect = rt.rect;

            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);


            return point.x >= corners[0].x && point.x <= corners[2].x &&
                   point.y >= corners[0].y && point.y <= corners[1].y;
        }

#if UNITY_EDITOR || DEBUG_MODE
        public static void LogAABB(this RectTransform rectTransform)
        {
            if (rectTransform != null)
            {
                GetAABB(rectTransform, out float left, out float top, out float right, out float bottom);
                DebugHelper.LogAABB(left, top, right, bottom);
            }
        }
#endif
    }
}