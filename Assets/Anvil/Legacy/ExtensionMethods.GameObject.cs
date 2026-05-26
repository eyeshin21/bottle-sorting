using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;
using System.Collections.Generic;
using Anvil.Legacy.Actions;
using Anvil.Legacy;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static bool Showing(this GameObject gameObject)
        {
            return gameObject != null && gameObject.activeSelf;
        }

        // public static void SetShow(this GameObject gameObject, bool show)
        // {
        //     if (gameObject != null)
        //     {
        //         gameObject.SetActive(show);
        //     }
        // }
        public static void BrowseChildren(this GameObject go, Action<GameObject> callback)
        {
            if (go == null || callback == null) return;

            callback(go);

            var transform = go.transform;
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                BrowseChildren(transform.GetChild(i).gameObject, callback);
            }
        }

        public static void PlayAnimationLegacy(this GameObject gameObject, string animationName)
        {
            AnimationController.PlayAnimation(gameObject, animationName);
        }

        static List<GameObject> _bfsChildren;
        static List<GameObject> BFSChildren
        {
            get
            {
                if (_bfsChildren == null)
                {
                    _bfsChildren = new List<GameObject>();
                }
                else
                {
                    _bfsChildren.Clear();
                }
                return _bfsChildren;
            }
        }

        static List<GameObject> _bfsNextChildren;
        static List<GameObject> BFSNextChildren
        {
            get
            {
                if (_bfsNextChildren == null)
                {
                    _bfsNextChildren = new List<GameObject>();
                }
                else
                {
                    _bfsNextChildren.Clear();
                }
                return _bfsNextChildren;
            }
        }

        /// <summary>
        /// continueFunc: Return true to continue to browse current's children.
        /// </summary>
        public static void BrowseChildrenBFS(this GameObject go, Func<GameObject, bool> continueFunc)
        {
            if (go == null) return;
            if (!continueFunc(go)) return;

            var transform = go.transform;
            int childCount = transform.childCount;
            if (childCount > 0)
            {
                if (childCount == 1)
                {
                    BrowseChildrenBFS(transform.GetChild(0).gameObject, continueFunc);
                }
                else
                {
                    var children = BFSChildren;
                    for (int i = 0; i < childCount; i++)
                    {
                        children.Add(transform.GetChild(i).gameObject);
                    }

                    BrowseChildrenBFS(children, BFSNextChildren, continueFunc);
                }
            }
        }

        static void BrowseChildrenBFS(List<GameObject> children, List<GameObject> nextChildren, Func<GameObject, bool> continueFunc)
        {
            Assert.IsEmpty(nextChildren);
            int childCount = children.Count;
            for (int i = 0; i < childCount; i++)
            {
                var child = children[i];
                if (continueFunc(child))
                {
                    var transform = child.transform;
                    int nextChildCount = transform.childCount;
                    if (nextChildCount > 0)
                    {
                        for (int j = 0; j < nextChildCount; j++)
                        {
                            nextChildren.Add(transform.GetChild(j).gameObject);
                        }
                    }
                }
            }

            children.Clear();

            if (nextChildren.Count > 0)
            {
                BrowseChildrenBFS(nextChildren, children, continueFunc);
            }
        }

        public static void BrowseChildren(this GameObject go, Func<GameObject, bool> continueFunc)
        {
            if (go == null) return;
            if (!continueFunc(go)) return;

            var transform = go.transform;
            int childCount = transform.childCount;
            if (childCount == 1)
            {
                BrowseChildren(transform.GetChild(0).gameObject, continueFunc);
            }
            else
            {
                for (int i = 0; i < childCount; i++)
                {
                    if (!continueFunc(transform.GetChild(i).gameObject))
                    {
                        return;
                    }
                }

                for (int i = 0; i < childCount; i++)
                {
                    if (!_BrowseChildren(transform.GetChild(i).gameObject, continueFunc))
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Returns true to continue.
        /// </summary>
        static bool _BrowseChildren(GameObject go, Func<GameObject, bool> continueFunc)
        {
            var transform = go.transform;
            int childCount = transform.childCount;
            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    if (!continueFunc(transform.GetChild(i).gameObject))
                    {
                        return false;
                    }
                }

                for (int i = 0; i < childCount; i++)
                {
                    if (!_BrowseChildren(transform.GetChild(i).gameObject, continueFunc))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static void ForceRebuildLayoutImmediate(this GameObject go)
        {
            if (go != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(go.transform as RectTransform);
            }
        }

    }
}