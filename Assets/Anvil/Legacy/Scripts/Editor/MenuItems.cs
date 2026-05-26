#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        const int DefaultPriority = -1;
        const int PriorityStep = 0;
        const int CreatePriority = DefaultPriority;
        const int CanvasPriority = CreatePriority + PriorityStep;
        const int RectTransformPriority = CanvasPriority + PriorityStep;
        const int UtilitiesPriority = RectTransformPriority + PriorityStep;

        static GameObject SelectedGameObject
        {
            get => Selection.activeGameObject;
            set => Selection.activeGameObject = value;
        }

        static GameObject[] SelectedGameObjects => Selection.gameObjects;

        /// <summary>
        /// Returns the first selected asset path ("Assets/...").
        /// </summary>
        static string SelectedAssetPath
        {
            get
            {
                var guids = Selection.assetGUIDs;
                return guids.IsNullOrEmpty() ? "Assets" : AssetDatabase.GUIDToAssetPath(guids[0]);
            }
        }

        /// <summary>
        /// "Assets/...".
        /// </summary>
        static string[] SelectedAssetPaths
        {
            get
            {
                var guids = Selection.assetGUIDs;
                int count = guids.GetCount();
                var assetPaths = new string[count];
                for (int i = 0; i < count; i++)
                {
                    assetPaths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
                }
                return assetPaths;
            }
        }

        static T GetSelectedComponent<T>() where T : Component
        {
            var selectedGameObject = SelectedGameObject;
            if (selectedGameObject != null)
            {
                var component = selectedGameObject.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
                LogComponentRequired<T>();
            }
            else
            {
                LogGameObjectRequired();
            }

            return null;
        }

        static void SetSelected<T>(Callback<T> callback) where T : Component
        {
            var component = GetSelectedComponent<T>();
            if (component != null)
            {
                callback(component);
            }
        }

        static T AddComponentTo<T>(Component component) where T : Component
        {
            var currentComponent = component.gameObject.GetComponent<T>();
            if (currentComponent != null)
            {
                LogComponentExisted(currentComponent);
                return null;
            }

            return component.gameObject.AddComponent<T>();
        }

        static TComponent AddComponentToSelectedGameObject<TSelected, TComponent>()
            where TSelected : Component
            where TComponent : Component
        {
            var selectedGameObject = SelectedGameObject;
            if (selectedGameObject != null)
            {
                if (!selectedGameObject.HasComponent<TSelected>())
                {
                    LogComponentRequired<TSelected>();
                    return null;
                }

                if (selectedGameObject.HasComponent<TComponent>())
                {
                    //LogComponentExisted(component);
                    return null;
                }

                var component = selectedGameObject.AddComponent<TComponent>();
                TrySetName(component);
                return component;
            }

            LogGameObjectRequired();
            return null;
        }

        static Transform CreateChildTransform(string name)
        {
            var selectedGameObject = SelectedGameObject;
            if (selectedGameObject != null)
            {
                var child = new GameObject(name);
                var transform = child.transform;
                transform.SetParent(selectedGameObject.transform);
                transform.localPosition = Vector3.zero;
                transform.localScale = Vector3.one;
                SelectedGameObject = child;

                return transform;
            }

            LogGameObjectRequired();
            return null;
        }

        static T Create<T>(string name) where T : Component
        {
            var go = Create(name);
            return go.AddComponent<T>();
        }

        static GameObject Create(string name)
        {
            var gameObject = new GameObject(name);
            EditorHelper.FocusHierachyAndSetActiveGameObject(gameObject);
            return gameObject;
        }

        static GameObject CreateChild(string name)
        {
            var selectedGameObject = SelectedGameObject;
            if (selectedGameObject != null)
            {
                var child = new GameObject(name);
                var transform = child.transform;
                transform.SetParent(selectedGameObject.transform);
                transform.localPosition = Vector3.zero;
                transform.localScale = Vector3.one;

                SelectedGameObject = child;
                return child;
            }

            LogGameObjectRequired();
            return null;
        }

        static GameObject CreateChild(GameObject prefab)
        {
            if (prefab == null)
            {
                LogNullPrefab();
                return null;
            }

            var selectedGameObject = SelectedGameObject;
            if (selectedGameObject != null)
            {
                var child = prefab.Create(selectedGameObject.transform);
                EditorHelper.ConnectGameObjectToPrefab(ref child, prefab);

                SelectedGameObject = child;
                return child;
            }

            LogGameObjectRequired();
            return null;
        }

        static T CreateChild<T>(string name) where T : Component
        {
            var go = CreateChild(name);
            return go != null ? go.AddComponent<T>() : default;
        }

        static RectTransform CreateChildRectTransform(string name)
        {
            var selectedGameObject = SelectedGameObject;
            if (selectedGameObject != null)
            {
                if (!selectedGameObject.HasComponent<RectTransform>())
                {
                    LogRectTransformRequired();
                    return null;
                }

                var child = new GameObject(name);
                child.transform.SetParent(selectedGameObject.transform);
                var rectTransform = child.AddComponent<RectTransform>();
                rectTransform.anchoredPosition3D = Vector3.zero;
                rectTransform.localScale = Vector3.one;

                SelectedGameObject = child;
                return rectTransform;
            }

            LogGameObjectRequired();
            return null;
        }

        static void CreateChildRectTransform(string name, Callback<RectTransform> callback)
        {
            var rectTransform = CreateChildRectTransform(name);
            if (rectTransform != null)
            {
                TryIgnoreLayout(rectTransform);
                callback(rectTransform);
            }
        }

        /// <summary>
        /// Set name to type's name if it not start with "GameObject".
        /// </summary>
        static void TrySetName<T>(T component) where T : Component
        {
            TrySetName(component, typeof(T).Name);
        }

        /// <summary>
        /// Set name to the specified one if it start with "GameObject".
        /// </summary>
        static void TrySetName<T>(T component, string name) where T : Component
        {
            if (component.name.StartsWith("GameObject"))
            {
                component.name = name;
            }
        }

        static void TryIgnoreLayout(RectTransform rectTransform)
        {
            if (rectTransform != null)
            {
                var parent = rectTransform.parent;
                if (parent != null)
                {
                    if (parent.HasComponent<LayoutGroup>())
                    {
                        var layoutElement = rectTransform.GetOrAddComponent<LayoutElement>();
                        layoutElement.ignoreLayout = true;
                    }
                }
            }
        }

        static void SetName(Transform transform, string name)
        {
            if (transform.name != name)
            {
                transform.name = name;
                EditorHelper.SetDirty(transform);
            }
        }

        static void SetNameBySprite(Transform transform)
        {
            var image = transform.GetComponent<Image>();
            if (image != null)
            {
                var sprite = image.sprite;
                if (sprite != null)
                {
                    SetName(transform, sprite.name);
                }
            }
            else
            {
                var spriteRenderer = transform.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    var sprite = spriteRenderer.sprite;
                    if (sprite != null)
                    {
                        SetName(transform, sprite.name);
                    }
                }
            }
        }

        static void LogNullPrefab()
        {
            LegacyLog.Warning("Prefab is null!");
        }

        static void LogGameObjectRequired()
        {
            LegacyLog.Warning("<b>GameObject</b> required!");
        }

        static void LogRectTransformRequired()
        {
            LegacyLog.Warning("<b>RectTransform</b> required!");
        }

        static void LogComponentRequired<T>() where T : Component
        {
            LegacyLog.Warning($"<b>{typeof(T).Name}</b> required!");
        }

        static void LogComponentExisted<T>(T component) where T : Component
        {
            LegacyLog.Warning($"<b>{typeof(T).Name}</b> existed!");
        }

        static void RefreshAssets()
        {
            AssetDatabase.Refresh();
        }

        static void OpenScene(string gameName, string sceneName)
        {
            if (Application.isPlaying)
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                string path;
                if (string.IsNullOrEmpty(gameName))
                {
                    path = $"Assets/Scenes/{sceneName}.unity";
                }
                else
                {
                    path = $"{gameName}/Scenes/{sceneName}";
                    if (!path.StartsWith("Assets/"))
                    {
                        path = $"Assets/{path}";
                    }
                    if (!path.EndsWith(".unity"))
                    {
                        path = $"{path}.unity";
                    }
                }
                Helper.OpenEditorScene(path);
            }
        }

        static void OpenWindow<T>() where T : EditorWindow
        {
            EditorHelper.ShowEditorWindow<T>();
        }

        //static void OpenScene(string sceneName, Func<string> pathFormatFunc = null)
        //{
        //    if (Application.isPlaying)
        //    {
        //        SceneManager.LoadScene(sceneName);
        //    }
        //    else
        //    {
        //        string path;
        //        if (pathFormatFunc != null)
        //        {
        //            path = string.Format(pathFormatFunc(), sceneName);
        //            if (!path.StartsWith("Assets/"))
        //            {
        //                path = $"Assets/{path}";
        //            }
        //            if (!path.EndsWith(".unity"))
        //            {
        //                path = $"{path}.unity";
        //            }
        //        }
        //        else
        //        {
        //            path = $"Assets/Scenes/{sceneName}.unity";
        //        }
        //        Helper.OpenEditorScene(path);
        //    }
        //}

        //[MenuItem("Gametamin/Test")]
        //static void Test()
        //{
        //    var chars = new char[] { 'a', 'u', 'A', 'U' };
        //    chars.ForEach(c =>LegacyLog.Debug($"{c}:{(int)c}"));
        //}
    }
}
#endif