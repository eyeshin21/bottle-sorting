using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static bool IsShow(this Component component)
        {
            return component != null ? component.gameObject.activeInHierarchy : false;
        }

        public static void SetShow(this Component component, bool show)
        {
            if (component != null)
            {
                component.gameObject.SetActive(show);
            }
        }

        public static float GetPositionX(this Component component)
        {
            return component != null ? component.transform.position.x : 0;
        }

        public static void SetPosition(this Component component, Vector3 pos)
        {
            if (component != null)
            {
                component.transform.position = pos;
            }
        }

        public static GameObject GetChild(this Component component, string name)
        {
            if (component != null)
            {
                return component.transform.GetChild(name)?.gameObject;
            }
            return null;
        }

        public static void SetParent(this Component component, GameObject parent)
        {
            if (component != null)
            {
                component.transform.SetParent(parent?.transform);
            }
        }

        public static void SetParent(this Component component, Component parent)
        {
            if (component != null)
            {
                component.transform.SetParent(parent?.transform);
            }
        }

        public static void SetNoParent(this Component component)
        {
            if (component != null)
            {
                component.transform.SetParent(null);
            }
        }

        /// <summary>
        /// Returns angle in degrees.
        /// </summary>
        public static float GetAngle(this Component component)
        {
            if (component != null)
            {
                return component.transform.localRotation.eulerAngles.z;
            }
            return 0;
        }

        /// <summary>
        /// Angle in degrees.
        /// </summary>
        public static void SetAngle(this Component component, float angle)
        {
            if (component != null)
            {
                component.transform.localRotation = Quaternion.Euler(0, 0, angle);
            }
        }

        public static void ResetRotation(this Component component)
        {
            if (component != null)
            {
                component.transform.localRotation = Quaternion.identity;
            }
        }

        public static void SetLocalScale(this Component component, float scale)
        {
            if (component != null)
            {
                component.transform.localScale = new Vector3(scale, scale, scale);
            }
        }

        public static void ResetLocalScale(this Component component)
        {
            if (component != null)
            {
                component.transform.localScale = Vector3.one;
            }
        }

        public static void CopyTransform(this Component component, Component other)
        {
            if (component != null && other != null)
            {
                component.transform.Copy(other.transform);
            }
        }

        public static void SetSortingOrder(this Component component, int sortingOrder)
        {
            if (component != null)
            {
                SetSortingOrder(component.gameObject, sortingOrder);
            }
        }

        public static void SetText(this Component component, string text)
        {
            SetText(component?.gameObject, text);
        }

        /// <summary>
        /// Sets size delta.
        /// </summary>
        public static void SetUISize(this Component component, float width, float height)
        {
            if (component != null)
            {
                var rectTransform = component.transform as RectTransform;
                if (rectTransform != null)
                {
                    rectTransform.sizeDelta = new Vector2(width, height);
                }
                else
                {
                    LegacyLog.Warning($"Can't set UI size for {component.GetHierarchyPath()}!");
                }
            }
        }

        public static void SetAnchoredPosition(this Component component, float x, float y)
        {
            if (component != null)
            {
                var rectTransform = component.transform as RectTransform;
                if (rectTransform != null)
                {
                    rectTransform.anchoredPosition = new Vector2(x, y);
                }
                else
                {
                    LegacyLog.Warning($"Can't set anchored position for {component.GetHierarchyPath()}!");
                }
            }
        }

        public static void SetTransformParent(this Component component, Transform parent)
        {
            if (component != null)
            {
                component.transform.SetParent(parent);
            }
        }

        public static T SetDefaultName<T>(this T component, string defaultName) where T : Component
        {
            if (component != null)
            {
                if (component.name.StartsWith("GameObject"))
                {
                    component.name = defaultName;
                }
            }
            return component;
        }

        public static void SetLayer<T>(this T component, int layer) where T : Component
        {
            if (component != null)
            {
                component.transform.ForEachTransform(child => child.gameObject.layer = layer);
            }
        }

        public static RectTransform GetRectTransform(this Component component)
        {
            if (component != null)
            {
                var rectTransform = component.transform as RectTransform;
                if (rectTransform == null)
                {
                    LegacyLog.Warning($"Can't get RectTransform from {component.GetHierarchyPath()}!");
                }
                return rectTransform;
            }
            return default;
        }

        public static bool HasComponent<T>(this Component component) where T : Component
        {
            if (component != null)
            {
                return component.GetComponent<T>() != null;
            }
            return false;
        }

        public static T AddComponent<T>(this Component component) where T : Component
        {
            if (component != null)
            {
                return component.gameObject.AddComponent<T>();
            }

            LogNullComponent();
            return default;
        }

        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            if (component != null)
            {
                T comp = component.GetComponent<T>();
                if (comp == null)
                {
                    comp = component.gameObject.AddComponent<T>();
                }
                return comp;
            }

            LogNullComponent();
            return default;
        }

        public static T CheckAddComponent<T>(this Component component) where T : Component
        {
            if (component != null)
            {
                var comp = component.GetComponent<T>();
                if (comp == null)
                {
                    comp = component.gameObject.AddComponent<T>();
                }
                return comp;
            }

            LogNullComponent();
            return default;
        }

        public static bool CheckGetComponent<T>(this Component component, ref T comp) where T : Component
        {
            if (comp == null)
            {
                comp = component.GetComponent<T>();
                if (comp == null)
                {
                    return false;
                }
            }
            return true;
        }

        public static T GetChildComponent<T>(this Component component) where T : Component
        {
            if (component != null)
            {
                return GetChildComponent<T>(component.transform);
            }
            return default;
        }

        public static T GetChildComponent<T>(this Component component, string name, bool includeInactive = true) where T : Component
        {
            if (component != null)
            {
                return GetChildComponent<T>(component.transform, name, includeInactive);
            }
            return default;
        }

        public static T GetComponentInRootChildren<T>(this Component component) where T : Component
        {
            if (component != null)
            {
                var transform = component.transform;
                for (int i = 0; i < transform.childCount; i++)
                {
                    T comp = transform.GetChild(i).GetComponent<T>();
                    if (comp != null)
                    {
                        return comp;
                    }
                }
            }
            else
            {
                LogNullComponent();
            }

            return null;
        }

        public static void DisableComponent<T>(this Component component) where T : Behaviour
        {
            if (component != null)
            {
                T comp = component.GetComponent<T>();
                if (comp != null)
                {
                    comp.enabled = false;
                }
            }
        }

        public static void SetStretchRectTransform(this Component component)
        {
            if (component != null)
            {
                var rectTransform = component.GetComponent<RectTransform>();
                rectTransform.SetStretch();
            }
            else
            {
                LogNullComponent();
            }
        }

        public static void ShowAndPlayAnimation(this Component component, string animationName)
        {
            if (component != null)
            {
                ShowAndPlayAnimation(component.gameObject, animationName);
            }
        }

        public static void PlayAnimation(this Component component, string animationName)
        {
            if (component != null)
            {
                PlayAnimation(component.gameObject, animationName);
            }
        }

        public static void PlayAnimation(this Component component, string animationName, Callback doneCallback)
        {
            if (component != null)
            {
                PlayAnimation(component.gameObject, animationName, doneCallback);
            }
        }

        public static void SetAnimationSpeed(this Component component, float speed)
        {
            if (component != null)
            {
                SetAnimationSpeed(component.gameObject, speed);
            }
        }

        public static string GetHierarchyPath(this Component component)
        {
            if (component != null)
            {
                return GetHierarchyPath(component.gameObject);
            }
            return "";
        }

        public static void DestroyGameObject(this Component component)
        {
            if (component != null)
            {
                Destroy(component.gameObject);
            }
        }

        public static void DelayDestroyGameObject(this Component component, float delay)
        {
            if (component != null)
            {
                DelayDestroy(component.gameObject, delay);
            }
        }

#if UNITY_EDITOR
        public static T SetIcon<T>(this T component, LabelIcon icon) where T : Component
        {
            if (component != null)
            {
                IconManager.SetIcon(component, icon);
            }
            return component;
        }
#endif
    }
}