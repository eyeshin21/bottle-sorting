using UnityEngine;
using System;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static AABB GetAABB(this Transform transform)
        {
            if (transform != null)
            {
                var rectTransform = transform.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    return rectTransform.GetAABB();
                }
            }
            return AABB.Default;
        }

        public static Transform CreateChildGroup(this Transform transform, string name)
        {
            Transform child;
            int childCount = transform.childCount;
            if (childCount == 0)
            {
                child = CreateChild(transform, name);
            }
            else if (childCount == 1)
            {
                var currentChild = transform.GetChild(0);
                child = CreateChild(transform, name);
                currentChild.SetParent(child);
            }
            else
            {
                var children = new Transform[childCount];
                for (int i = 0; i < childCount; i++)
                {
                    children[i] = transform.GetChild(i);
                }

                child = CreateChild(transform, name);
                for (int i = 0; i < childCount; i++)
                {
                    children[i].SetParent(child);
                }
            }

            return child;
        }

        public static Transform CreateLocalChild(this Transform transform)
        {
            return CreateChildGroup(transform, "Local");
        }

        public static void AddDeltaX(this Transform transform, float deltaX)
        {
            if (transform != null)
            {
                var pos = transform.position;
                pos.x += deltaX;
                transform.position = pos;
            }
        }

        public static void AddDeltaY(this Transform transform, float deltaY)
        {
            if (transform != null)
            {
                var pos = transform.position;
                pos.y += deltaY;
                transform.position = pos;
            }
        }

        public static void AddDeltaPosition(this Transform transform, Vector2 deltaPos)
        {
            if (transform != null)
            {
                var pos = transform.position;
                pos.x += deltaPos.x;
                pos.y += deltaPos.y;
                transform.position = pos;
            }
        }

        public static void AddDeltaPosition(this Transform transform, float deltaX, float deltaY)
        {
            if (transform != null)
            {
                var pos = transform.position;
                pos.x += deltaX;
                pos.y += deltaY;
                transform.position = pos;
            }
        }

        public static void SetAnchoredPosition(this Transform transform, float x, float y)
        {
            if (transform != null)
            {
                var rectTransform = transform.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.anchoredPosition3D = new Vector3(x, y, 0);
                }
            }
        }

        public static Transform GetChild(this Transform transform, string name, bool includeInactive = true)
        {
            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                var child = transform.GetChild(i);
                if (!includeInactive && !child.gameObject.activeSelf) continue;

                if (child.name == name)
                {
                    return child;
                }
            }

            return null;
        }

        public static Transform GetChildByPath(this Transform transform, string path, bool includeInactive = true)
        {
            int index = path.IndexOf('/');
            if (index > 0)
            {
                var child = GetChild(transform, path.Substring(0, index), includeInactive);
                if (child != null)
                {
                    return GetChildByPath(child, path.Substring(index + 1), includeInactive);
                }
                return null;
            }
            return GetChild(transform, path, includeInactive);
        }

        public static Transform GetChildBFS(this Transform transform, string name, bool includeInactive = true)
        {
            // Level 1
            var child = GetChild(transform, name, includeInactive);
            if (child != null) return child;

            // Level 2
            int childCount = transform.childCount;
            if (childCount > 0)
            {
                var children = new List<Transform>();
                for (int i = 0; i < childCount; i++)
                {
                    child = transform.GetChild(i);
                    int childCount2 = child.childCount;
                    for (int j = 0; j < childCount2; j++)
                    {
                        var child2 = child.GetChild(j);
                        if (includeInactive || child2.gameObject.activeSelf)
                        {
                            if (child2.name == name)
                            {
                                return child2;
                            }
                        }

                        children.Add(child2);
                    }
                }

                return GetChildBFS(children, name, includeInactive);
            }

            return null;
        }

        static Transform GetChildBFS(List<Transform> children, string name, bool includeInactive)
        {
            if (children.Count == 0) return null;

            var first = children[0];
            children.RemoveAt(0);

            int childCount = first.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child2 = first.GetChild(i);
                if (includeInactive || child2.gameObject.activeSelf)
                {
                    if (child2.name == name)
                    {
                        return child2;
                    }
                }

                children.Add(child2);
            }

            return GetChildBFS(children, name, includeInactive);
        }

        public static void SetAsPrevLastSibling(this Transform transform)
        {
            if (transform != null)
            {
                transform.SetSiblingIndex(transform.parent.childCount - 2);
            }
        }

        public static void BrowseRootChildren(this Transform transform, Func<Transform, bool> continueFunc)
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                if (!continueFunc(child))
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Browses all children exclude this transform (DFS).
        /// </summary>
        public static void BrowseChildren(this Transform transform, Action<Transform> callback)
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                callback(child);
                BrowseChildren(child, callback);
            }
        }

        //public static void BrowseComponentsInChildren<T>(this Transform transform, Action<T> callback) where T : Component
        //{
        //    T comp = transform.GetComponent<T>();
        //    if (comp != null)
        //    {
        //        callback?.Invoke(comp);
        //    }

        //    int childCount = transform.childCount;
        //    for (int i = 0; i < childCount; i++)
        //    {
        //        BrowseComponentsInChildren(transform.GetChild(i), callback);
        //    }
        //}

        /// <summary>
        /// func(component, continue)
        /// </summary>
        public static bool BrowseComponentsInChildren<T>(this Transform transform, Func<T, bool> func) where T : Component
        {
            T comp = transform.GetComponent<T>();
            if (comp != null)
            {
                if (!func(comp))
                {
                    return false;
                }
            }

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                if (!BrowseComponentsInChildren(transform.GetChild(i), func))
                {
                    return false;
                }
            }

            return true;
        }

        public static void DestroyChild(this Transform transform, string childName)
        {
            if (Application.isPlaying)
            {
                for (int i = transform.childCount - 1; i >= 0; i--)
                {
                    var child = transform.GetChild(i);
                    if (child.name == childName)
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
                    if (child.name == childName)
                    {
                        GameObject.DestroyImmediate(child.gameObject);
                    }
                }
            }
        }

        public static void HideChildren(this Transform transform)
        {
            if (transform != null)
            {
                int childCount = transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        public static void TrySetChildIconTransform(this Transform transform, ref Transform iconTransform)
        {
            if (iconTransform == null)
            {
                Transform iconTransform2 = null;
                transform.BrowseRootChildren(child =>
                {
                    if (child.name.Contains("icon", StringComparison.OrdinalIgnoreCase))
                    {
                        iconTransform2 = child;
                        return false;
                    }
                    return true;
                });
                iconTransform = iconTransform2;
            }
        }

        public static void TrySetChildTextObject(this Transform transform, ref GameObject textObject)
        {
            if (textObject == null)
            {
                GameObject textObject2 = null;
                transform.BrowseRootChildren(child =>
                {
                    if (child.name.Contains("text", StringComparison.OrdinalIgnoreCase))
                    {
                        textObject2 = child.gameObject;
                        return false;
                    }
                    return true;
                });
                textObject = textObject2;
            }
        }

        public static bool ForEachTransform(this Transform transform, Func<Transform, bool> continueFunc)
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

    }
}