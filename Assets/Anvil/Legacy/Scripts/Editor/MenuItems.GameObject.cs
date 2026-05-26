#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        [MenuItem("GameObject/Gametamin/Set Scene 1080x1920", false, DefaultPriority)]
        static void SetScene1080x1920()
        {
            EditorHelper.SetGameView1080x1920();
            SetScene(1080, 1920);
        }

        [MenuItem("GameObject/Gametamin/Set Scene 1920x1080", false, DefaultPriority)]
        static void SetScene1920x1080()
        {
            EditorHelper.SetGameView1920x1080();
            SetScene(1920, 1080);
        }

        static void SetScene(int screenWidth, int screenHeight)
        {
            var camera = Helper.GetSceneRootComponent<Camera>();
            if (camera != null)
            {
                //camera.orthographicSize = screenHeight * 0.5f * 0.01f;
                if (screenHeight == 1920)
                {
                    camera.orthographicSize = 9.6f; // Fix 9.599999
                }
                else
                {
                    camera.orthographicSize = screenHeight * 0.5f * 0.01f;
                }
                EditorHelper.CheckSetActiveObject(camera);
            }

            var canvas = Helper.GetSceneRootComponent<Canvas>();
            if (canvas == null)
            {
                canvas = new GameObject("Canvas").AddComponent<Canvas>();
            }
            canvas.SetScreenSpaceCamera(screenWidth, screenHeight);

            var eventSystem = Helper.GetSceneRootComponent<EventSystem>();
            if (eventSystem == null)
            {
                eventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }
        }
    }
}
#endif