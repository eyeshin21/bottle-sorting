using System;
using UnityEngine;
using UnityEngine.UI;

namespace Anvil
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