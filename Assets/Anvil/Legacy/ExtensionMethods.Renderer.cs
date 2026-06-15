using UnityEngine;

namespace Anvil
{
    public static partial class ExtensionMethods
    {
        public static int GetSortingLayerIndex(this Renderer renderer)
        {
            if (renderer != null)
            {
                var layers = SortingLayer.layers;
                var sortingLayerID = renderer.sortingLayerID;
                for (int i = 0; i < layers.Length; i++)
                {
                    if (layers[i].id == sortingLayerID)
                    {
                        return i;
                    }
                }
            }

            return 0;
        }

        public static void SetSortingLayerIndex(this Renderer renderer, int layerIndex)
        {
            if (renderer != null)
            {
                renderer.sortingLayerID = SortingLayer.layers[layerIndex].id;
            }
        }

        public static int GetSortingLayerID(this Renderer renderer)
        {
            return renderer != null ? renderer.sortingLayerID : 0;
        }

        public static void SetSortingLayerID(this Renderer renderer, int sortingLayerID)
        {
            if (renderer != null)
            {
                renderer.sortingLayerID = sortingLayerID;
            }
        }

        public static int GetSortingOrder(this Renderer renderer)
        {
            return renderer != null ? renderer.sortingOrder : 0;
        }

        public static void SetSortingOrder(this Renderer renderer, int sortingOrder)
        {
            if (renderer != null)
            {
                renderer.sortingOrder = sortingOrder;
            }
        }

        public static Material Material(this Renderer renderer)
        {
            if (renderer != null)
            {
                return Application.isPlaying ? renderer.material : renderer.sharedMaterial;
            }

            return null;
        }

        public static Color GetColor(this Renderer renderer)
        {
            var material = renderer.Material();
            if (material != null)
            {
                return material.color;
            }

            return Color.white;
        }

        public static void SetColor(this Renderer renderer, Color color)
        {
            var material = renderer.Material();
            if (material != null)
            {
                material.color = color;
            }
        }

        public static void SetRGB(this Renderer renderer, Vector3 rgb)
        {
            var material = renderer.Material();
            if (material != null)
            {
                var color = material.color;
                color.r = rgb.x;
                color.g = rgb.y;
                color.b = rgb.z;
                material.color = color;
            }
        }

        public static float GetAlpha(this Renderer renderer)
        {
            var material = renderer.Material();
            if (material != null)
            {
                return material.color.a;
            }

            return 1;
        }

        public static void SetAlpha(this Renderer renderer, float a)
        {
            var material = renderer.Material();
            if (material != null)
            {
                var color = material.color;
                color.a = a;
                material.color = color;
            }
        }

        public static Texture GetTexture(this Renderer renderer)
        {
            var material = renderer.Material();
            if (material != null)
            {
                return material.mainTexture;
            }

            return null;
        }

        public static void SetTexture(this Renderer renderer, Sprite sprite)
        {
            var material = renderer.Material();
            if (material != null)
            {
                material.mainTexture = sprite != null ? sprite.texture : null;
            }
        }
    }
}