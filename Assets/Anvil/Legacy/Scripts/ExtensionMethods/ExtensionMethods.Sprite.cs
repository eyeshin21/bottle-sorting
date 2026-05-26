using UnityEngine;
using UnityEngine.Sprites;
using System.Collections.Generic;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Returns sprite width in units.
        /// </summary>
        public static float GetWidth(this Sprite sprite)
        {
            return sprite != null ? sprite.bounds.size.x : 0;
        }

        /// <summary>
        /// Returns sprite height in units.
        /// </summary>
        public static float GetHeight(this Sprite sprite)
        {
            return sprite != null ? sprite.bounds.size.y : 0;
        }

        /// <summary>
        /// Returns sprite size in units.
        /// </summary>
        public static void GetSize(this Sprite sprite, out float width, out float height)
        {
            if (sprite != null)
            {
                var size = sprite.bounds.size;
                width = size.x;
                height = size.y;
            }
            else
            {
                width = height = 0;
            }
        }

        /// <summary>
        /// Returns width in pixels.
        /// </summary>
        public static float GetRectWidth(this Sprite sprite)
        {
            return sprite != null ? sprite.rect.width : 0;
        }

        /// <summary>
        /// Returns height in pixels.
        /// </summary>
        public static float GetRectHeight(this Sprite sprite)
        {
            return sprite != null ? sprite.rect.height : 0;
        }

        /// <summary>
        /// Returns size in pixels.
        /// </summary>
        public static void GetRectSize(this Sprite sprite, out float width, out float height)
        {
            if (sprite != null)
            {
                var size = sprite.rect.size;
                width = size.x;
                height = size.y;
            }
            else
            {
                width = height = 0;
            }
        }

        //public static float GetWidthInPixels(this Sprite sprite)
        //{
        //    return sprite != null ? sprite.bounds.size.x * sprite.pixelsPerUnit : 0;
        //}

        //public static float GetHeightInPixels(this Sprite sprite)
        //{
        //    return sprite != null ? sprite.bounds.size.y * sprite.pixelsPerUnit : 0;
        //}

        public static void GetSizeInPixels(this Sprite sprite, out float width, out float height)
        {
            if (sprite != null)
            {
                var size = sprite.bounds.size;
                float pixelsPerUnit = sprite.pixelsPerUnit;
                width = size.x * pixelsPerUnit;
                height = size.y * pixelsPerUnit;
            }
            else
            {
                width = height = 0;
            }
        }

        /// <summary>
        /// Returns size in pixels.
        /// </summary>
        public static Vector2 GetSize(this Sprite sprite, Vector2 size)
        {
            bool setWidth = size.x <= 0;
            bool setHeight = size.y <= 0;
            if (setWidth || setHeight)
            {
                GetSizeInPixels(sprite, out float spriteWidth, out float spriteHeight);
                if (setWidth)
                {
                    size.x = spriteWidth;
                }
                if (setHeight)
                {
                    size.y = spriteHeight;
                }
            }
            return size;
        }

        public static void CheckSetSize(this Sprite sprite, ref Vector2 size)
        {
            bool setWidth = size.x <= 0;
            bool setHeight = size.y <= 0;
            if (setWidth || setHeight)
            {
                GetSizeInPixels(sprite, out float spriteWidth, out float spriteHeight);
                if (setWidth)
                {
                    size.x = spriteWidth;
                }
                if (setHeight)
                {
                    size.y = spriteHeight;
                }
            }
        }

        /// <summary>
        /// Returns UI's size in pixels.
        /// </summary>
        public static Vector2 GetUISize(this Sprite sprite)
        {
            if (sprite != null)
            {
                return sprite.rect.size;
            }
            return Vector2.zero;
        }

        public static List<Vector3> GetPhysicsShape(this Sprite sprite, float maxDistance)
        {
            return GetPhysicsShape(sprite, 0, maxDistance);
        }

        public static List<Vector3> GetPhysicsShape(this Sprite sprite, int shapeIndex, float maxDistance)
        {
            if (sprite != null && sprite.GetPhysicsShapeCount() > shapeIndex)
            {
                var points = new List<Vector2>();
                sprite.GetPhysicsShape(shapeIndex, points);
                return maxDistance > 0 ? Helper.GetPoints(points, maxDistance, true) : points.ToListVector3();
            }
            return null;
        }

        /// <summary>
        /// Returns borders in units.
        /// </summary>
        public static void GetBorders(this Sprite sprite, out float left, out float top, out float right, out float bottom)
        {
            if (sprite != null)
            {
                var border = sprite.border;
                float factor = 1f / sprite.pixelsPerUnit;
                left = border.x * factor;
                bottom = border.y * factor;
                right = border.z * factor;
                top = border.w * factor;
            }
            else
            {
                left = top = right = bottom = 0;
            }
        }

        public static void GetUVs(this Sprite sprite, out float uvLeft, out float uvTop, out float uvRight, out float uvBottom)
        {
            //var texture = sprite != null ? sprite.texture : null;
            //if (texture != null)
            //{
            //    var rect = sprite.textureRect;
            //    var rectOffset = sprite.textureRectOffset;

            //    uvLeft = (rect.x + rectOffset.x) / texture.width;
            //    uvRight = uvLeft + rect.width / texture.width;
            //    uvBottom = (rect.y + rectOffset.y) / texture.height;
            //    uvTop = uvBottom + rect.height / texture.height;
            //}
            //else
            //{
            //    uvLeft = uvTop = uvRight = uvBottom = 0;
            //}
            if (sprite != null)
            {
                var outer = DataUtility.GetOuterUV(sprite);
                uvLeft = outer.x;
                uvBottom = outer.y;
                uvRight = outer.z;
                uvTop = outer.w;
            }
            else
            {
                uvLeft = 0;
                uvRight = 1;
                uvBottom = 0;
                uvTop = 1;
            }
        }

        public static void GetUVs(this Sprite sprite,
                                  out float uvLeft, out float uvTop, out float uvRight, out float uvBottom,
                                  out float uvX, out float uvY, out float uvZ, out float uvW)
        {
            //var texture = sprite != null ? sprite.texture : null;
            //if (texture != null)
            //{
            //    var rect = sprite.textureRect;
            //    var rectOffset = sprite.textureRectOffset;
            //    float widthFactor = 1f / texture.width;
            //    float heightFactor = 1f / texture.height;

            //    uvLeft = (rect.x + rectOffset.x) * widthFactor;
            //    uvRight = uvLeft + rect.width * widthFactor;
            //    uvBottom = (rect.y + rectOffset.y) * heightFactor;
            //    uvTop = uvBottom + rect.height * heightFactor;

            //    var border = sprite.border;
            //    uvX = uvLeft + border.x * widthFactor;
            //    uvY = uvBottom + border.y * heightFactor;
            //    uvZ = uvRight - border.z * widthFactor;
            //    uvW = uvTop - border.w * heightFactor;
            //}
            //else
            //{
            //    uvLeft = uvTop = uvRight = uvBottom = 0;
            //    uvX = uvY = uvZ = uvW = 0;
            //}
            if (sprite != null)
            {
                var outer = DataUtility.GetOuterUV(sprite);
                uvLeft = outer.x;
                uvBottom = outer.y;
                uvRight = outer.z;
                uvTop = outer.w;

                var inner = DataUtility.GetInnerUV(sprite);
                uvX = inner.x;
                uvY = inner.y;
                uvZ = inner.z;
                uvW = inner.w;
            }
            else
            {
                uvX = uvLeft = 0;
                uvZ = uvRight = 1;
                uvY = uvBottom = 0;
                uvW = uvTop = 1;
            }
        }

        public static void GetUVs(this Sprite sprite, out float uvLeft, out float uvTop, out float uvRight, out float uvBottom,
                                float width, float height, float paddingX, float paddingY, out float uvX, out float uvY, out float uvZ, out float uvW)
        {
            if (sprite != null)
            {
                var outer = DataUtility.GetOuterUV(sprite);
                uvLeft = outer.x;
                uvBottom = outer.y;
                uvRight = outer.z;
                uvTop = outer.w;

                float factorX = paddingX / width;
                float factorY = paddingY / height;
                float uvOffsetX = (uvRight - uvLeft) * factorX;
                float uvOffsetY = (uvTop - uvBottom) * factorY;
                uvX = uvLeft + uvOffsetX;
                uvY = uvBottom + uvOffsetY;
                uvZ = uvRight - uvOffsetX;
                uvW = uvTop - uvOffsetY;
            }
            else
            {
                uvX = uvLeft = 0;
                uvZ = uvRight = 1;
                uvY = uvBottom = 0;
                uvW = uvTop = 1;
            }
        }

        public static float[] GetUVs8(this Sprite sprite)
        {
            GetUVs(sprite, out float uvLeft, out float uvTop, out float uvRight, out float uvBottom, out float uvX, out float uvY, out float uvZ, out float uvW);
            return new float[] { uvLeft, uvTop, uvRight, uvBottom, uvX, uvY, uvZ, uvW };
        }

        public static bool GetBorderAndUVs(this Sprite sprite, out float borderLeft, out float borderTop, out float borderRight, out float borderBottom,
                                            out float uvX, out float uvY, out float uvZ, out float uvW)
        {
            var texture = sprite != null ? sprite.texture : null;
            if (texture != null)
            {
                var border = sprite.border;
                borderLeft = border.x;
                borderTop = border.w;
                borderRight = border.z;
                borderBottom = border.y;
                if (borderLeft > 0 || borderTop > 0 || borderRight > 0 || borderBottom > 0)
                {
                    var rect = sprite.textureRect;
                    var rectOffset = sprite.textureRectOffset;
                    float widthFactor = 1f / texture.width;
                    float heightFactor = 1f / texture.height;

                    float uvLeft = (rect.x + rectOffset.x) * widthFactor;
                    float uvRight = uvLeft + rect.width * widthFactor;
                    float uvBottom = (rect.y + rectOffset.y) * heightFactor;
                    float uvTop = uvBottom + rect.height * heightFactor;

                    uvX = uvLeft + border.x * widthFactor;
                    uvY = uvBottom + border.y * heightFactor;
                    uvZ = uvRight - border.z * widthFactor;
                    uvW = uvTop - border.w * heightFactor;

                    return true;
                }
            }

            borderLeft = borderTop = borderRight = borderBottom = 0;
            uvX = uvY = uvZ = uvW = 0;
            return false;
        }

        public static Mesh CreateMesh(this Sprite sprite)
        {
            var mesh = new Mesh();
#if UNITY_EDITOR
            mesh.name = $"{sprite.name}-Mesh";
#endif
            mesh.vertices = sprite.vertices.ToArrayVector3();
            mesh.uv = sprite.uv;
            mesh.triangles = sprite.triangles.ToArrayInt();
            return mesh;
        }

        public static Texture2D CreateTexture(this Sprite sprite)
        {
            //return UnityEditor.AssetPreview.GetAssetPreview(sprite);

            var texture = sprite.texture;
            int textureWidth = texture.width;
            int textureHeight = texture.height;
            sprite.rect.Get(out int x, out int y, out int width, out int height);
            //Log.Debug($"rect=({x}, {y}, {width}, {height}), textureSize={textureWidth}x{textureHeight}");
            Texture2D newTexture;
            if (width < textureWidth || height < textureHeight)
            {
                newTexture = new Texture2D(width, height);
                newTexture.SetPixels(texture.GetPixels(x, y, width, height));

            }
            else
            {
                newTexture = new Texture2D(textureWidth, textureHeight);
                newTexture.SetPixels(texture.GetPixels());
            }
            newTexture.Apply();
            return newTexture;
        }
    }
}