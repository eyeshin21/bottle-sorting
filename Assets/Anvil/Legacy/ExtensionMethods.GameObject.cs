using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil
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
        public static T CheckAddComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject == null)
            {
                return null;
            }

            T ret = gameObject.GetComponent<T>(); 
            if (ret != null)
            {
                return ret;
            }
            return gameObject.AddComponent<T>();
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

        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
            {
                component = go.AddComponent<T>();
            }
            return component;
        }

        public static void ForceLayout(this GameObject go)
        {
            if (go != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(go.transform as RectTransform);
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