using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static string GetName(this GameObject gameObject)
        {
            return gameObject != null ? gameObject.name : "(null)";
        }

        public static bool IsShow(this GameObject gameObject)
        {
            return gameObject != null ? gameObject.activeInHierarchy : false;
        }

        public static void SetShow(this GameObject gameObject, bool show)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(show);
            }
        }

        public static Vector3 GetPosition(this GameObject gameObject)
        {
            return gameObject != null ? gameObject.transform.position : Vector3.zero;
        }

        public static void SetPosition(this GameObject gameObject, Vector3 pos)
        {
            if (gameObject != null)
            {
                gameObject.transform.position = pos;
            }
        }

        public static void SetLocalPosition(this GameObject gameObject, Vector3 pos)
        {
            if (gameObject != null)
            {
                gameObject.transform.localPosition = pos;
            }
        }

        public static void SetLocalPositionX(this GameObject gameObject, float x)
        {
            if (gameObject != null)
            {
                var pos = gameObject.transform.localPosition;
                pos.x = x;
                gameObject.transform.localPosition = pos;
            }
        }

        public static void SetLocalPosition(this GameObject gameObject, Transform localTransform, Vector3 pos)
        {
            if (gameObject != null)
            {
                var transform = gameObject.transform;
                var parentTransform = transform.parent;
                transform.SetParent(localTransform);
                transform.localPosition = pos;
                transform.SetParent(parentTransform);
            }
        }

        public static float GetLocalScale(this GameObject gameObject)
        {
            if (gameObject != null)
            {
                var scale = gameObject.transform.localScale;
                return (scale.x + scale.y) * 0.5f;
            }
            return 1f;
        }

        public static void SetLocalScale(this GameObject gameObject, float scale)
        {
            if (gameObject != null)
            {
                gameObject.transform.localScale = new Vector3(scale, scale, scale);
            }
        }

        public static void SetLocalScale(this GameObject gameObject, Vector3 scale)
        {
            if (gameObject != null)
            {
                gameObject.transform.localScale = scale;
            }
        }

        /// <summary>
        /// gameObject.localScale *= scale
        /// </summary>
        public static void ApplyLocalScale(this GameObject gameObject, float scale)
        {
            if (gameObject != null)
            {
                gameObject.transform.localScale *= scale;
            }
        }

        /// <summary>
        /// Angle in degrees.
        /// </summary>
        public static void SetAngle(this GameObject gameObject, float angle)
        {
            if (gameObject != null)
            {
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, angle);
            }
        }

        public static void SetLocalRotation(this GameObject gameObject, Quaternion rotation)
        {
            if (gameObject != null)
            {
                gameObject.transform.localRotation = rotation;
            }
        }

        public static void SetParent(this GameObject gameObject, Transform parent)
        {
            if (gameObject != null)
            {
                gameObject.transform.SetParent(parent);
            }
        }

        public static void SetLayer(this GameObject gameObject, int layer)
        {
            if (gameObject != null)
            {
                gameObject.transform.ForEachTransform(child => child.gameObject.layer = layer);
            }
        }

        public static bool HasComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject != null)
            {
                return gameObject.GetComponent<T>() != null;
            }
            return false;
        }

        public static T CheckGetComponent<T>(this GameObject gameObject) //where T : Component
        {
            return gameObject != null ? gameObject.GetComponent<T>() : default;
        }

        public static bool IsPrefab(this GameObject gameObject)
        {
            if (gameObject == null) return false;
            //return gameObject.scene.rootCount == 0;

            // I don't care about GameObjects *inside* prefabs, just the overall prefab.
            var scene = gameObject.scene;
            return !scene.IsValid() && !scene.isLoaded && gameObject.GetInstanceID() >= 0 &&
                // I noticed that ones with IDs under 0 were objects I didn't recognize
                !gameObject.hideFlags.HasFlag(HideFlags.HideInHierarchy);
        }

        public static Sprite GetSprite(this GameObject gameObject)
        {
            return SpriteController.GetSprite(gameObject);
        }

        public static void SetSprite(this GameObject gameObject, Sprite sprite)
        {
            SpriteController.SetSprite(gameObject, sprite);
        }

        public static Color GetColor(this GameObject gameObject)
        {
            return ColorController.GetColor(gameObject);
        }

        public static void SetColor(this GameObject gameObject, Color color)
        {
            ColorController.SetColor(gameObject, color);
        }

        public static Vector3 GetRGB(this GameObject gameObject)
        {
            return ColorController.GetRGB(gameObject);
        }

        public static void SetRGB(this GameObject gameObject, Vector3 rgb)
        {
            ColorController.SetRGB(gameObject, rgb);
        }

        public static float GetAlpha(this GameObject gameObject)
        {
            return AlphaController.GetAlpha(gameObject);
        }

        public static void SetAlpha(this GameObject gameObject, float alpha)
        {
            AlphaController.SetAlpha(gameObject, alpha);
        }

        public static void SetSortingOrder(this GameObject gameObject, int sortingOrder)
        {
            SortingController.SetSortingOrder(gameObject, sortingOrder);
        }

        public static string GetText(this GameObject gameObject)
        {
            return TextController.GetText(gameObject);
        }

        public static void SetText(this GameObject gameObject, string text)
        {
            TextController.SetText(gameObject, text);
        }

        public static void SetSize(this GameObject gameObject, float width, float height)
        {
            if (gameObject != null)
            {
                //TODO
                var iSizeSetter = gameObject.GetComponentInChildren<ISizeSetter>();
                if (iSizeSetter != null)
                {
                    iSizeSetter.SetSize(width, height);
                }
                else
                {
                    var spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.size = new Vector2(width, height);
                    }
                }
            }
            else
            {
                LegacyLog.Warning($"Can't set size: GameObject is null!");
            }
        }

        public static AABB GetAABB(this GameObject go)
        {
            if (go != null)
            {
                var rectTransform = go.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    return rectTransform.GetAABB();
                }
            }
            return AABB.Default;
        }

        public static void ShowAndPlayAnimation(this GameObject gameObject, string animationName)
        {
            gameObject.SetShow(true);
            AnimationController.PlayAnimation(gameObject, animationName);
        }
        //
        // public static float GetAnimationLength(this GameObject gameObject, string animationName, float defaultValue = 0)
        // {
        //     AnimationController.GetAnimationLength(gameObject, animationName, out float length);
        //     if (length <= 0)
        //     {
        //         length = defaultValue;
        //     }
        //     return length;
        // }

        public static void PlayAnimation(this GameObject gameObject, string animationName)
        {
            AnimationController.PlayAnimation(gameObject, animationName);
        }

        public static void PlayAnimation(this GameObject gameObject, string animationName, Callback doneCallback)
        {
            AnimationController.PlayAnimation(gameObject, animationName, ()=>
            {
                doneCallback?.Invoke();
            });
        }
        public static void PlayAnimation(this GameObject gameObject, string animationName, Action doneCallback)
        {
            AnimationController.PlayAnimation(gameObject, animationName, doneCallback);
        }

        public static void SetAnimationSpeed(this GameObject gameObject, float speed)
        {
            AnimationController.SetAnimationSpeed(gameObject, speed);
        }

        public static GameObject Create(this GameObject prefab, Transform parent = null, bool worldPositionStays = false)
        {
            if (prefab != null)
            {
                var go = GameObject.Instantiate<GameObject>(prefab, parent, worldPositionStays);
                FixName(go);
                return go;
            }

            return null;
        }

        public static GameObject Create(this GameObject prefab, Transform parent, Vector3 position, bool worldPositionStays = false)
        {
            if (prefab != null)
            {
                var go = GameObject.Instantiate<GameObject>(prefab, parent, worldPositionStays);
                FixName(go);
                go.transform.position = position;
                return go;
            }

            return null;
        }

        public static T Create<T>(this GameObject prefab, Transform parent = null, bool worldPositionStays = false) //where T : Component
        {
            if (prefab != null)
            {
                var go = GameObject.Instantiate<GameObject>(prefab, parent, worldPositionStays);
                FixName(go);
                return go.GetComponent<T>();
            }

            return default;
        }

        public static T Create<T>(this GameObject prefab, Transform parent, Vector3 position, bool worldPositionStays = false) //where T : Component
        {
            if (prefab != null)
            {
                var go = GameObject.Instantiate<GameObject>(prefab, parent, worldPositionStays);
                FixName(go);
                go.transform.position = position;
                return go.GetComponent<T>();
            }

            return default;
        }

        public static GameObject CreateUI(this GameObject prefab, bool worldPositionStays = false)
        {
            if (prefab != null)
            {
                var go = GameObject.Instantiate<GameObject>(prefab, Context.MainCanvas.transform, worldPositionStays);
                FixName(go);
                return go;
            }

            return null;
        }

        public static T CreateUI<T>(this GameObject prefab, bool worldPositionStays = false) where T : Component
        {
            return CreateUI<T>(prefab, Context.MainCanvas, worldPositionStays);
        }

        public static T CreateUI<T>(this GameObject prefab, Canvas canvas, bool worldPositionStays = false) where T : Component
        {
            if (prefab != null)
            {
                var safeArea = canvas.transform.GetChildComponent<SafeArea>();
                Transform parent = safeArea != null ? safeArea.transform : canvas.transform;
                var go = GameObject.Instantiate<GameObject>(prefab, parent, worldPositionStays);
                FixName(go);
                return go.GetComponent<T>();
            }
            else
            {
                LegacyLog.Warning("Prefab is null!");
            }

            return null;
        }

        public static T CreateChild<T>(this GameObject go, string name = "") where T : Component
        {
            var child = new GameObject();
            if (!name.IsNullOrEmpty())
            {
                child.name = name;
            }
            var childTransform = child.transform;
            childTransform.SetParent(go.transform);
            childTransform.localPosition = Vector3.zero;
            childTransform.localRotation = Quaternion.identity;
            childTransform.localScale = Vector3.one;
            return child.AddComponent<T>();
        }

        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
            {
                component = go.AddComponent<T>();
            }
            return component;
        }

        public static GameObject GetChild(this GameObject gameObject, string childName)
        {
            if (gameObject != null)
            {
                var childSetter = gameObject.GetComponent<ChildSetter>();
                if (childSetter != null)
                {
                    return childSetter.GetChild(childName);
                }

                GameObject child = null;
                gameObject.transform.ForEachChildTransform(transform =>
                {
                    if (transform.name == childName)
                    {
                        child = transform.gameObject;
                        return false;
                    }
                    return true;
                });
                return child;
            }
            return null;
        }

        public static void ForceLayout(this GameObject go)
        {
            if (go != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(go.transform as RectTransform);
            }
        }

        public static void AddButtonListener(this GameObject go, Listener onClick)
        {
            if (go != null && onClick != null)
            {
                var button = go.GetComponentInChildren<Button>();
                if (button != null)
                {
                    button.AddListener(onClick);
                }
                else
                {
                    LegacyLog.Warning($"{go.GetHierarchyPath()}: Button not found!");
                }
            }
        }

        public static void AddAnimationEventListener(this GameObject gameObject, Listener<string> listener)
        {
            if (gameObject != null)
            {
                var eventHandler = gameObject.GetComponentInChildren<AnimationEventHandler>();
                if (eventHandler != null)
                {
                    eventHandler.AddListener(listener);
                }
                else
                {
                    LegacyLog.Warning($"Can't add animation event listener to {gameObject.GetHierarchyPath()}: AnimationEventHandler not found!");
                }
            }
            else
            {
                LegacyLog.Warning("Can't add animation event listener: GameObject is null!");
            }
        }

        public static void RemoveAnimationEventListener(this GameObject gameObject, Listener<string> listener)
        {
            if (gameObject != null)
            {
                var eventHandler = gameObject.GetComponentInChildren<AnimationEventHandler>();
                if (eventHandler != null)
                {
                    eventHandler.RemoveListener(listener);
                }
                else
                {
                    LegacyLog.Warning($"Can't remove animation event listener from {gameObject.GetHierarchyPath()}: AnimationEventHandler not found!");
                }
            }
            else
            {
                LegacyLog.Warning("Can't remove animation event listener: GameObject is null!");
            }
        }

        static List<GameObject> _gameObjectBFSChildren;
        static List<GameObject> GameObjectBFSChildren => Helper.CreateOrClear(ref _gameObjectBFSChildren);

        static List<GameObject> _gameObjectBFSNextChildren;
        static List<GameObject> GameObjectBFSNextChildren => Helper.CreateOrClear(ref _gameObjectBFSNextChildren);

        public static void ForEachComponent<T>(this GameObject go, Callback<T> callback) where T : Component
        {
            if (go == null) return;

            T comp = go.GetComponent<T>();
            if (comp != null)
            {
                callback(comp);
            }

            var transform = go.transform;
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                ForEachComponent(transform.GetChild(i).gameObject, callback);
            }
        }

        /// <summary>
        /// Only children's transform.
        /// </summary>
        public static void ForEachChildTransform(this GameObject gameObject, Callback<Transform> callback)
        {
            if (gameObject != null)
            {
                ForEachChildTransform(gameObject.transform, callback);
            }
            else
            {
                LegacyLog.Warning("Game object is null");
            }
        }

        public static void ForEachChildTransform(this GameObject gameObject, ContinueFunc<Transform> continueFunc)
        {
            if (gameObject != null)
            {
                ForEachChildTransform(gameObject.transform, continueFunc);
            }
            else
            {
                LegacyLog.Warning("Game object is null");
            }
        }

        public static void ForEachChildBFS(this GameObject go, ContinueFunc<GameObject, BFSContinueType> continueFunc)
        {
            if (go == null) return;

            var result = continueFunc(go);
            if (result != BFSContinueType.Continue) return;

            var transform = go.transform;
            int childCount = transform.childCount;
            if (childCount > 0)
            {
                if (childCount == 1)
                {
                    ForEachChildBFS(transform.GetChild(0).gameObject, continueFunc);
                }
                else
                {
                    var children = GameObjectBFSChildren;
                    children.AddChildren(transform);
                    ForEachChildBFS(children, GameObjectBFSNextChildren, continueFunc);
                }
            }
        }

        static void ForEachChildBFS(List<GameObject> children, List<GameObject> nextChildren, ContinueFunc<GameObject, BFSContinueType> continueFunc)
        {
            Assert.IsEmpty(nextChildren);
            int childCount = children.Count;
            for (int i = 0; i < childCount; i++)
            {
                var child = children[i];
                var result = continueFunc(child);
                if (result == BFSContinueType.Continue)
                {
                    nextChildren.AddChildren(child);
                }
                else if (result == BFSContinueType.Break)
                {
                    children.Clear();
                    nextChildren.Clear();
                    return;
                }
            }
            children.Clear();

            if (nextChildren.Count > 0)
            {
                ForEachChildBFS(nextChildren, children, continueFunc);
            }
        }

        /// <summary>
        /// continueFunc: Return true to continue to browse current's children.
        /// </summary>
        //public static void ForEachChildBFS(this GameObject go, ContinueFunc<GameObject> continueFunc)
        //{
        //    if (go == null) return;
        //    if (!continueFunc(go)) return;

        //    var transform = go.transform;
        //    int childCount = transform.childCount;
        //    if (childCount > 0)
        //    {
        //        if (childCount == 1)
        //        {
        //            ForEachChildBFS(transform.GetChild(0).gameObject, continueFunc);
        //        }
        //        else
        //        {
        //            var children = GameObjectBFSChildren;
        //            children.AddChildren(transform);
        //            ForEachChildBFS(children, GameObjectBFSNextChildren, continueFunc);
        //        }
        //    }
        //}

        //static void ForEachChildBFS(List<GameObject> children, List<GameObject> nextChildren, ContinueFunc<GameObject> continueFunc)
        //{
        //    Assert.IsEmpty(nextChildren);
        //    int childCount = children.Count;
        //    for (int i = 0; i < childCount; i++)
        //    {
        //        var child = children[i];
        //        if (continueFunc(child))
        //        {
        //            nextChildren.AddChildren(child);
        //        }
        //    }
        //    children.Clear();

        //    if (nextChildren.Count > 0)
        //    {
        //        ForEachChildBFS(nextChildren, children, continueFunc);
        //    }
        //}

        public static void ForEachChild(this GameObject go, ContinueFunc<GameObject> continueFunc)
        {
            if (go == null) return;
            if (!continueFunc(go)) return;

            var transform = go.transform;
            int childCount = transform.childCount;
            if (childCount == 1)
            {
                ForEachChild(transform.GetChild(0).gameObject, continueFunc);
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
                    if (!_ForEachChild(transform.GetChild(i).gameObject, continueFunc))
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Returns true to continue.
        /// </summary>
        static bool _ForEachChild(GameObject go, ContinueFunc<GameObject> continueFunc)
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
                    if (!_ForEachChild(transform.GetChild(i).gameObject, continueFunc))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        //public static void BrowseTransforms(this GameObject go, Action<Transform> callback)
        //{
        //    if (go != null)
        //    {
        //        go.transform.BrowseTransforms(callback);
        //    }
        //}

        public static void ForEachTransform(this GameObject go, Callback<Transform> callback)
        {
            if (go != null)
            {
                go.transform.ForEachTransform(callback);
            }
        }

        public static void BrowseGameObjects(this GameObject go, Action<GameObject> callback)
        {
            callback(go);

            Transform transform = go.transform;
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                BrowseGameObjects(transform.GetChild(i).gameObject, callback);
            }
        }

        public static void BrowseGameObjects(this GameObject go, Func<GameObject, bool> continueFunc)
        {
            if (!continueFunc(go)) return;

            Transform transform = go.transform;
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                if (!_BrowseGameObjects(transform.GetChild(i).gameObject, continueFunc))
                {
                    return;
                }
            }
        }

        static bool _BrowseGameObjects(GameObject go, Func<GameObject, bool> continueFunc)
        {
            if (!continueFunc(go)) return false;

            Transform transform = go.transform;
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                if (!_BrowseGameObjects(transform.GetChild(i).gameObject, continueFunc))
                {
                    return false;
                }
            }

            return true;
        }

        public static string GetHierarchyPath(this GameObject gameObject)
        {
            if (gameObject != null)
            {
                return GetHierarchyPath(gameObject.transform);
            }
            return "";
        }

        public static void Destroy(this GameObject go)
        {
            if (go != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    GameObject.DestroyImmediate(go);
                    return;
                }
#endif
                GameObject.Destroy(go);
            }
        }

        public static void DelayDestroy(this GameObject go, float delay)
        {
            if (go != null)
            {
                go.DelayCall(delay, () => GameObject.Destroy(go));
            }
        }

        [Conditional("UNITY_EDITOR")]
        static void FixName(GameObject go)
        {
            if (go != null)
            {
                var name = go.name.Replace("(Clone)", "");
                if (name.EndsWith("Spine", StringComparison.Ordinal))
                {
                    name = name.Substring(0, name.Length - 5);
                }
                go.name = name;
            }
        }

#if UNITY_EDITOR
        public static void SetDirty(this GameObject go)
        {
            if (go != null)
            {
                EditorUtility.SetDirty(go);
            }
        }
#endif
    }
}