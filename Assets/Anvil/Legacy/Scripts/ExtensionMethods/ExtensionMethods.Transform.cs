using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static void Copy(this Transform transform, Transform other)
        {
            transform.position = other.position;
            transform.localRotation = other.localRotation;
            transform.localScale = other.localScale;
        }

        public static void CopyLocal(this Transform transform, Transform other)
        {
            if (transform != null && other != null)
            {
                transform.localPosition = other.localPosition;
                transform.localRotation = other.localRotation;
                transform.localScale = other.localScale;
            }
        }

        public static void Reset(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Returns world position.
        /// </summary>
        public static Vector3 GetPosition(this Transform transform)
        {
            return transform != null ? transform.position : Vector3.zero;
        }

        public static float GetPositionX(this Transform transform)
        {
            return transform != null ? transform.position.x : 0;
        }

        public static float GetPositionY(this Transform transform)
        {
            return transform != null ? transform.position.y : 0;
        }

        public static Vector3 GetPosition(this Transform transform, bool local)
        {
            if (transform != null)
            {
                return local ? transform.localPosition : transform.position;
            }
            return Vector3.zero;
        }

        public static Vector3 GetPositionByX(this Transform transform, float x)
        {
            if (transform != null)
            {
                var pos = transform.position;
                pos.x = x;
                return pos;
            }
            return new Vector3(x, 0);
        }

        public static Vector3 GetPositionByY(this Transform transform, float y)
        {
            if (transform != null)
            {
                var pos = transform.position;
                pos.y = y;
                return pos;
            }
            return new Vector3(0, y);
        }

        public static Vector3 GetPositionByDeltaX(this Transform transform, float deltaX)
        {
            if (transform != null)
            {
                var pos = transform.position;
                pos.x += deltaX;
                return pos;
            }
            return new Vector3(deltaX, 0);
        }

        public static Vector3 GetPositionByDeltaY(this Transform transform, float deltaY)
        {
            if (transform != null)
            {
                var pos = transform.position;
                pos.y += deltaY;
                return pos;
            }
            return new Vector3(0, deltaY);
        }

        public static void SetPositionX(this Transform transform, float x)
        {
            if (transform != null)
            {
                var pos = transform.position;
                pos.x = x;
                transform.position = pos;
            }
        }

        public static void SetPositionY(this Transform transform, float y)
        {
            if (transform != null)
            {
                var pos = transform.position;
                pos.y = y;
                transform.position = pos;
            }
        }

        public static void SetPosition(this Transform transform, Vector3 pos, bool local)
        {
            if (transform != null)
            {
                if (local)
                {
                    transform.localPosition = pos;
                }
                else
                {
                    transform.position = pos;
                }
            }
        }

        public static float GetScale(this Transform transform)
        {
            if (transform != null)
            {
                var scale = transform.localScale;
                return (scale.x + scale.y) * 0.5f;
            }
            return 1f;
        }

        public static void SetScale(this Transform transform, float scale)
        {
            if (transform != null)
            {
                transform.localScale = new Vector3(scale, scale, scale);
            }
        }

        /// <summary>
        /// Returns angle in degrees.
        /// </summary>
        public static float GetAngle(this Transform transform)
        {
            if (transform != null)
            {
                return transform.localRotation.eulerAngles.z;
            }
            return default;
        }

        /// <summary>
        /// Angle is in degrees.
        /// </summary>
        public static void SetAngle(this Transform transform, float angle)
        {
            if (transform != null)
            {
                transform.localRotation = Quaternion.Euler(0, 0, angle);
            }
        }

        //public static void SetText(this Transform transform, string text)
        //{
        //    if (transform != null)
        //    {
        //        var tmpText = transform.GetComponent<TMPro.TMP_Text>();
        //        if (tmpText != null)
        //        {
        //            tmpText.text = text;
        //        }
        //    }
        //}

        //public static void SetTextColor(this Transform transform, Color color)
        //{
        //    if (transform != null)
        //    {
        //        var tmpText = transform.GetComponent<TMPro.TMP_Text>();
        //        if (tmpText != null)
        //        {
        //            tmpText.color = color;
        //        }
        //    }
        //}

        public static Transform GetChild(this Transform transform, string name)//, bool includeInactive = true)
        {
            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                var child = transform.GetChild(i);
                //if (includeInactive || child.gameObject.activeSelf)
                {
                    if (child.name == name)
                    {
                        return child;
                    }
                }
            }
            LegacyLog.Warning($"{transform.GetHierarchyPath()}: Can't get child \"{name}\"!");
            return null;
        }

        public static Transform GetChildByTag(this Transform transform, string tag, bool includeInactive = true)
        {
            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                var child = transform.GetChild(i);
                if (includeInactive || child.gameObject.activeSelf)
                {
                    if (child.tag == tag)
                    {
                        return child;
                    }
                }
            }
            return null;
        }

        public static Transform GetOrCreateChild(this Transform transform, string name)
        {
            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                var child = transform.GetChild(i);
                if (child.name == name)
                {
                    return child;
                }
            }

            var child2 = new GameObject(name).transform;
            child2.SetParent(transform);
            return child2;
        }

        public static T GetChildComponent<T>(this Transform transform) where T : Component
        {
            if (transform != null)
            {
                int childCount = transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    T component = transform.GetChild(i).GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                }
            }
            return default;
        }

        public static T GetChildComponent<T>(this Transform transform, string name, bool includeInactive = true) where T : Component
        {
            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                var child = transform.GetChild(i);
                if (includeInactive || child.gameObject.activeSelf)
                {
                    if (child.name == name)
                    {
                        return child.GetComponent<T>();
                    }
                }
            }
            return null;
        }

        public static Transform CreateChild(this Transform transform, string name)
        {
            var child = new GameObject(name).transform;
            child.SetParent(transform);
            child.localPosition = Vector3.zero;
            child.localRotation = Quaternion.identity;
            child.localScale = Vector3.one;
            return child;
        }

        public static T CreateChild<T>(this Transform transform, string name) where T : Component
        {
            var child = new GameObject(name).transform;
            child.SetParent(transform);
            child.localPosition = Vector3.zero;
            child.localRotation = Quaternion.identity;
            child.localScale = Vector3.one;
            return child.AddComponent<T>();
        }

        public static void DoIdentity(this Transform transform, Callback callback)
        {
            var pos = transform.localPosition;
            var rotation = transform.localRotation;
            var scale = transform.localScale;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            callback();
            transform.localPosition = pos;
            transform.localRotation = rotation;
            transform.localScale = scale;
        }

        public static void ForceLayout(this Transform transform)
        {
            if (transform != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
            }
        }

        //public static void BrowseTransforms(this Transform transform, Action<Transform> callback)
        //{
        //    if (transform != null)
        //    {
        //        callback(transform);

        //        int childCount = transform.childCount;
        //        for (int i = 0; i < childCount; i++)
        //        {
        //            BrowseTransforms(transform.GetChild(i), callback);
        //        }
        //    }
        //}

        //public static void BrowseComponents<T>(this Transform transform, Action<T> callback) //where T : Component
        //{
        //    if (transform != null)
        //    {
        //        var component = transform.GetComponent<T>();
        //        if (component != null)
        //        {
        //            callback(component);
        //        }

        //        int childCount = transform.childCount;
        //        for (int i = 0; i < childCount; i++)
        //        {
        //            BrowseComponents(transform.GetChild(i), callback);
        //        }
        //    }
        //}

        /// <summary>
        /// Only children's transform.
        /// </summary>
        public static void ForEachChildTransform(this Transform transform, Callback<Transform> callback)
        {
            if (transform == null || callback == null) return;

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                callback(transform.GetChild(i));
            }
        }

        public static void ForEachChildTransform(this Transform transform, ContinueFunc<Transform> continueFunc)
        {
            if (transform == null || continueFunc == null) return;

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                if (!continueFunc(transform.GetChild(i)))
                {
                    break;
                }
            }
        }

        public static void ForEachChildComponent<T>(this Transform transform, Callback<T> callback) where T : Component
        {
            if (transform == null || callback == null) return;

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                T comp = transform.GetChild(i).GetComponent<T>();
                if (comp != null)
                {
                    callback(comp);
                }
            }
        }

        public static void ForEachChildComponent<T>(this Transform transform, ContinueFunc<T> continueFunc) where T : Component
        {
            if (transform == null || continueFunc == null) return;

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                T comp = transform.GetChild(i).GetComponent<T>();
                if (comp != null)
                {
                    if (!continueFunc(comp))
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// All transforms (include this transform).
        /// </summary>
        public static void ForEachTransform(this Transform transform, Callback<Transform> callback)
        {
            if (transform == null || callback == null) return;

            callback(transform);

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                ForEachTransform(transform.GetChild(i), callback);
            }
        }

        public static bool ForEachTransform(this Transform transform, ContinueFunc<Transform> continueFunc)
        {
            if (transform == null || continueFunc == null) return true;

            if (!continueFunc(transform))
            {
                return false;
            }

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                if (!ForEachTransform(transform.GetChild(i), continueFunc))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ForEachGameObject(this Transform transform, ContinueFunc<GameObject> continueFunc)
        {
            if (transform == null || continueFunc == null) return true;

            if (!continueFunc(transform.gameObject))
            {
                return false;
            }

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                if (!ForEachGameObject(transform.GetChild(i), continueFunc))
                {
                    return false;
                }
            }

            return true;
        }

        public static void ForEachComponent<T>(this Transform transform, Callback<T> callback) where T : Component
        {
            if (transform == null || callback == null) return;

            T comp = transform.GetComponent<T>();
            if (comp != null)
            {
                callback(comp);
            }

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                ForEachComponent(transform.GetChild(i), callback);
            }
        }

        public static bool ForEachComponent<T>(this Transform transform, ContinueFunc<T> continueFunc) where T : Component
        {
            if (transform == null || continueFunc == null) return true;

            T comp = transform.GetComponent<T>();
            if (comp != null)
            {
                if (!continueFunc(comp))
                {
                    return false;
                }
            }

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                if (!ForEachComponent(transform.GetChild(i), continueFunc))
                {
                    return false;
                }
            }

            return true;
        }

        //public static void ForEachComponentInRootChildren<T>(this Transform transform, Action<T> callback) where T : Component
        //{
        //    if (transform != null)
        //    {
        //        int childCount = transform.childCount;
        //        for (int i = 0; i < childCount; i++)
        //        {
        //            var component = transform.GetChild(i).GetComponent<T>();
        //            if (component != null)
        //            {
        //                callback(component);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// func(component, continue)
        /// </summary>
        //public static bool BrowseComponentsInChildren<T>(this Transform transform, Func<T, bool> func) where T : Component
        //{
        //    T comp = transform.GetComponent<T>();
        //    if (comp != null)
        //    {
        //        if (!func(comp))
        //        {
        //            return false;
        //        }
        //    }

        //    int childCount = transform.childCount;
        //    for (int i = 0; i < childCount; i++)
        //    {
        //        if (!BrowseComponentsInChildren(transform.GetChild(i), func))
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}


        public static void DestroyChildren(this Transform transform)
        {
            if (transform == null) return;

            if (Application.isPlaying)
            {
                for (int i = transform.childCount - 1; i >= 0; i--)
                {
                    GameObject.Destroy(transform.GetChild(i).gameObject);
                }
            }
            else
            {
                for (int i = transform.childCount - 1; i >= 0; i--)
                {
                    GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
                }
            }
        }

        public static void DestroyChildren(this Transform transform, Func<Transform, bool> acceptFunc)
        {
            if (transform == null) return;

            if (Application.isPlaying)
            {
                for (int i = transform.childCount - 1; i >= 0; i--)
                {
                    var child = transform.GetChild(i);
                    if (acceptFunc(child))
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                }
            }
            else
            {
                for (int i = transform.childCount - 1; i >= 0; i--)
                {
                    var child = transform.GetChild(i);
                    if (acceptFunc(child))
                    {
                        GameObject.DestroyImmediate(child.gameObject);
                    }
                }
            }
        }

        public static void DestroyChildrenNotIgnoreLayout(this Transform transform)
        {
            DestroyChildren(transform, child =>
            {
                var layoutElement = child.GetComponent<LayoutElement>();
                return layoutElement == null || !layoutElement.ignoreLayout;
            });
        }

        public static string GetHierarchyPath(this Transform transform)
        {
            if (transform != null)
            {
                var parent = transform.parent;
                return parent != null ? $"{GetHierarchyPath(parent)}/{transform.name}" : transform.name;
            }
            return "";
        }
    }
}