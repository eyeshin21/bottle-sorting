using UnityEngine;
using UnityEngine.SceneManagement;
using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Anvil.Legacy
{
    public static partial class Helper
    {
        public static string ActiveSceneName => SceneManager.GetActiveScene().name;

        public static GameObject[] RootGameObjects => SceneManager.GetActiveScene().GetRootGameObjects();

        public static GameObject GetSceneRootGameObject(string name)
        {
            foreach (var gameObject in RootGameObjects)
            {
                if (gameObject.name == name)
                {
                    return gameObject;
                }
            }
            return null;
        }

        public static Transform GetSceneRootTransform(string name)
        {
            return GetSceneRootGameObject(name)?.transform;
        }

        public static T GetSceneRootComponent<T>(bool includeInactive = false) where T : Component
        {
            foreach (var gameObject in RootGameObjects)
            {
                if (includeInactive || gameObject.activeSelf)
                {
                    T comp = gameObject.GetComponent<T>();
                    if (comp != null)
                    {
                        return comp;
                    }
                }
            }
            return null;
        }

        public static void ForEachSceneGameObject(ContinueFunc<GameObject> continueFunc)
        {
            foreach (var gameObject in RootGameObjects)
            {
                gameObject.transform.ForEachGameObject(continueFunc);
            }
        }

        public static void LoadScene(string sceneName)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                LoadEditorScene($"Assets/Scenes/{sceneName}.unity");
                return;
            }
#endif
            SceneManager.LoadScene(sceneName);
        }

        public static void LoadScene(string path, string sceneName)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                LoadEditorScene($"{path}/{sceneName}.unity");
                return;
            }
#endif
            SceneManager.LoadScene(sceneName);
        }

        public static void ReloadScene()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorSceneManager.OpenScene(EditorSceneManager.GetActiveScene().path);
                return;
            }
#endif
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

#if UNITY_EDITOR
        public static void OpenEditorScene(string path, Callback callback = null)
        {
            if (!path.EndsWith(".unity", StringComparison.Ordinal))
            {
                path += ".unity";
            }

            if (path.IndexOf('/') < 0)
            {
                path = "Assets/Scenes/" + path;
            }

            var activeScene = EditorSceneManager.GetActiveScene();
            if (activeScene.path != path)
            {
                if (activeScene.isDirty)
                {
                    EditorApplication.delayCall += () =>
                    {
                        EditorSceneManager.SaveScene(activeScene);
                        EditorSceneManager.OpenScene(path);
                        callback?.Invoke();
                    };
                }
                else
                {
                    EditorSceneManager.OpenScene(path);
                    callback?.Invoke();
                }
            }
            else
            {
                callback?.Invoke();
            }
        }

        public static void LoadEditorScene(string path)
        {
            OpenEditorScene(path, () => EditorApplication.isPlaying = true);
        }
#endif
    }
}