#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        [MenuItem("CONTEXT/Renderer/Disable Shadow & Light")]
        static void RendererDisableShadowAndLight(MenuCommand menuCommand)
        {
            var renderer = menuCommand.To<Renderer>();
            EditorHelper.Set(renderer, "Disable Shadow & Light", renderer.DisableShadowAndLight);
        }

        [MenuItem("CONTEXT/Renderer/Set Color/Red")]
        static void RendererSetColorRed(MenuCommand menuCommand)
        {
            var renderer = menuCommand.To<Renderer>();
            renderer.SetColor(Color.red);
        }

        [MenuItem("CONTEXT/Renderer/Set Color/Green")]
        static void RendererSetColorGreen(MenuCommand menuCommand)
        {
            var renderer = menuCommand.To<Renderer>();
            renderer.SetColor(Color.green);
        }

        [MenuItem("CONTEXT/Renderer/Set Color/Blue")]
        static void RendererSetColorBlue(MenuCommand menuCommand)
        {
            var renderer = menuCommand.To<Renderer>();
            renderer.SetColor(Color.blue);
        }

        [MenuItem("CONTEXT/Renderer/Set Color/Yellow")]
        static void RendererSetColorYellow(MenuCommand menuCommand)
        {
            var renderer = menuCommand.To<Renderer>();
            renderer.SetColor(Color.yellow);
        }
    }
}
#endif