using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public class CustomImage : UIMeshBehaviour
    {
        enum ImageType
        {
            Normal,

            FlipHorizontal,
            FlipVertical,
            FlipAll,

            AddFlipHorizontal,
            AddFlipVertical,
            AddFlipAll,

            Sliced,
            SlicedAddFlipHorizontal,
            //SlicedAddFlipVertical,
            //SlicedAddFlipAll,

            SlicedTiledHorizontal = Sliced + 4,
            SlicedTiledVertical,
            SlicedTiledAll,

            Tiled,
        }

        [SerializeField] ImageType _imageType;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            OnPopulateMesh(vh, sprite =>
            {
                sprite.GetUVs(out float uvLeft, out float uvTop, out float uvRight, out float uvBottom);
                GetAABB(sprite, out float left, out float top, out float right, out float bottom);

                if (_imageType == ImageType.Normal)
                {
                    AddQuad(vh, left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom);
                }
                else
                {
                    void AddSlicedTiledHorizontal(float width)
                    {
                        var texture = sprite.texture;
                        float widthFactor = 1f / texture.width;
                        var size = sprite.rect.size;
                        var border = sprite.border;
                        float uvX = uvLeft + border.x * widthFactor;
                        float uvZ = uvRight - border.z * widthFactor;
                        float leftRight = border.x + border.z;
                        float spriteMiddle = size.x - leftRight;
                        float middle = width - leftRight;
                        int middleCount = Mathf.Max(Helper.RoundToInt(middle / spriteMiddle), 1);
                        float scaleX = middle / (middleCount * spriteMiddle);
                        float x = left + border.x;
                        float stepX = spriteMiddle * scaleX;
                        // Left
                        AddQuad(vh, left, top, x, bottom, uvLeft, uvTop, uvX, uvBottom);
                        // Middle
                        for (int i = 0; i < middleCount; i++)
                        {
                            float x2 = x + stepX;
                            AddQuad(vh, x, top, x2, bottom, uvX, uvTop, uvZ, uvBottom);
                            x = x2;
                        }
                        // Right
                        AddQuad(vh, x, top, right, bottom, uvZ, uvTop, uvRight, uvBottom);
                    }

                    void AddSlicedTiledVertical(float height)
                    {
                        var texture = sprite.texture;
                        float heightFactor = 1f / texture.height;
                        var size = sprite.rect.size;
                        var border = sprite.border;
                        float uvY = uvBottom + border.y * heightFactor;
                        float uvW = uvTop - border.w * heightFactor;
                        float topBottom = border.y + border.w;
                        float spriteMiddle = size.y - topBottom;
                        float middle = height - topBottom;
                        int middleCount = Mathf.Max(Helper.RoundToInt(middle / spriteMiddle), 1);
                        float scaleY = middle / (middleCount * spriteMiddle);
                        float y = bottom + border.y;
                        float stepY = spriteMiddle * scaleY;
                        // Bottom
                        AddQuad(vh, left, y, right, bottom, uvLeft, uvY, uvRight, uvBottom);
                        // Middle
                        for (int i = 0; i < middleCount; i++)
                        {
                            float y2 = y + stepY;
                            AddQuad(vh, left, y2, right, y, uvLeft, uvW, uvRight, uvY);
                            y = y2;
                        }
                        // Top
                        AddQuad(vh, left, top, right, y, uvLeft, uvTop, uvRight, uvW);
                    }

                    if (_imageType == ImageType.FlipHorizontal)
                    {
                        AddQuad(vh, left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom, UVTransformType.FlipHorizontal);
                    }
                    else if (_imageType == ImageType.FlipVertical)
                    {
                        AddQuad(vh, left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom, UVTransformType.FlipVertical);
                    }
                    else if (_imageType == ImageType.FlipAll)
                    {
                        AddQuad(vh, left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom, UVTransformType.FlipAll);
                    }
                    else if (_imageType == ImageType.AddFlipHorizontal)
                    {
                        float middle = (left + right) * 0.5f;
                        AddQuad(vh, left, top, middle, bottom, uvLeft, uvTop, uvRight, uvBottom);
                        AddQuad(vh, middle, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom, UVTransformType.FlipHorizontal);
                    }
                    else if (_imageType == ImageType.AddFlipVertical)
                    {
                        float middle = (bottom + top) * 0.5f;
                        AddQuad(vh, left, top, right, middle, uvLeft, uvTop, uvRight, uvBottom);
                        AddQuad(vh, left, middle, right, bottom, uvLeft, uvTop, uvRight, uvBottom, UVTransformType.FlipVertical);
                    }
                    else if (_imageType == ImageType.AddFlipAll)
                    {
                        float middle1 = (left + right) * 0.5f;
                        float middle2 = (bottom + top) * 0.5f;
                        AddQuad(vh, left, top, middle1, middle2, uvLeft, uvTop, uvRight, uvBottom);
                        AddQuad(vh, middle1, top, right, middle2, uvLeft, uvTop, uvRight, uvBottom, UVTransformType.FlipHorizontal);
                        AddQuad(vh, left, middle2, middle1, bottom, uvLeft, uvTop, uvRight, uvBottom, UVTransformType.FlipVertical);
                        AddQuad(vh, middle1, middle2, right, bottom, uvLeft, uvTop, uvRight, uvBottom, UVTransformType.FlipAll);
                    }
                    else if (_imageType == ImageType.Sliced)
                    {
                        AddSliced(vh, left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom);
                    }
                    else if (_imageType == ImageType.SlicedAddFlipHorizontal)
                    {
                        float height = top - bottom;
                        float spriteHeight = sprite.GetRectHeight();
                        float middle = (left + right) * 0.5f;
                        if (height > spriteHeight)
                        {
                            float heightFactor = 1f / sprite.texture.height;
                            var border = sprite.border;
                            float uvY = uvBottom + border.y * heightFactor;
                            float uvW = uvTop - border.w * heightFactor;
                            float y = bottom + border.y;
                            float z = top - border.w;
                            // Bottom
                            AddQuad(vh, left, y, middle, bottom, uvLeft, uvY, uvRight, uvBottom);
                            AddQuad(vh, middle, y, right, bottom, uvLeft, uvY, uvRight, uvBottom, UVTransformType.FlipHorizontal);
                            // Middle
                            AddQuad(vh, left, z, middle, y, uvLeft, uvW, uvRight, uvY);
                            AddQuad(vh, middle, z, right, y, uvLeft, uvW, uvRight, uvY, UVTransformType.FlipHorizontal);
                            // Top
                            AddQuad(vh, left, top, middle, z, uvLeft, uvTop, uvRight, uvW);
                            AddQuad(vh, middle, top, right, z, uvLeft, uvTop, uvRight, uvW, UVTransformType.FlipHorizontal);
                        }
                        else
                        {
                            AddQuad(vh, left, top, middle, bottom, uvLeft, uvTop, uvRight, uvBottom);
                            AddQuad(vh, middle, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom, UVTransformType.FlipHorizontal);
                        }
                    }
                    else if (_imageType == ImageType.SlicedTiledHorizontal)
                    {
                        float width = right - left;
                        float spriteWidth = sprite.GetRectWidth();
                        var border = sprite.border;
                        if (width > spriteWidth && (border.x > 0 || border.z > 0))
                        {
                            AddSlicedTiledHorizontal(width);
                        }
                        else
                        {
                            AddQuad(vh, left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom);
                        }
                    }
                    else if (_imageType == ImageType.SlicedTiledVertical)
                    {
                        float height = top - bottom;
                        float spriteHeight = sprite.GetRectHeight();
                        var border = sprite.border;
                        if (height > spriteHeight && (border.y > 0 || border.w > 0))
                        {
                            AddSlicedTiledVertical(height);
                        }
                        else
                        {
                            AddQuad(vh, left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom);
                        }
                    }
                    else if (_imageType == ImageType.SlicedTiledAll)
                    {
                        float width = right - left;
                        float height = top - bottom;
                        sprite.GetRectSize(out float spriteWidth, out float spriteHeight);
                        var border = sprite.border;
                        bool tiledHorizontal = width > spriteWidth && (border.x > 0 || border.z > 0);
                        bool tiledVertical = height > spriteHeight && (border.y > 0 || border.w > 0);
                        if (tiledHorizontal)
                        {
                            if (tiledVertical)
                            {
                                var texture = sprite.texture;
                                float widthFactor = 1f / texture.width;
                                float heightFactor = 1f / texture.height;
                                float uvX = uvLeft + border.x * widthFactor;
                                float uvZ = uvRight - border.z * widthFactor;
                                float uvY = uvBottom + border.y * heightFactor;
                                float uvW = uvTop - border.w * heightFactor;
                                float leftRight = border.x + border.z;
                                float topBottom = border.y + border.w;
                                float spriteHorMiddle = spriteWidth - leftRight;
                                float spriteVerMiddle = spriteHeight - topBottom;
                                float horMiddle = width - leftRight;
                                float verMiddle = height - topBottom;
                                int horMiddleCount = Mathf.Max(Helper.RoundToInt(horMiddle / spriteHorMiddle), 1);
                                int verMiddleCount = Mathf.Max(Helper.RoundToInt(verMiddle / spriteVerMiddle), 1);
                                float scaleX = horMiddle / (horMiddleCount * spriteHorMiddle);
                                float scaleY = verMiddle / (verMiddleCount * spriteVerMiddle);
                                float x = left + border.x;
                                float y = bottom + border.y;
                                float z = right - border.z;
                                float w = top - border.w;
                                float stepX = spriteHorMiddle * scaleX;
                                float stepY = spriteVerMiddle * scaleY;
                                float l, t, r, b;

                                // Top
                                {
                                    l = left;
                                    t = top;
                                    r = x;
                                    b = w;

                                    // Left
                                    AddQuad(vh, l, t, r, b, uvLeft, uvTop, uvX, uvW);
                                    l = r;
                                    // Middle
                                    for (int i = 0; i < horMiddleCount; i++)
                                    {
                                        r += stepX;
                                        AddQuad(vh, l, t, r, b, uvX, uvTop, uvZ, uvW);
                                        l = r;
                                    }
                                    // Right
                                    AddQuad(vh, l, t, right, b, uvZ, uvTop, uvRight, uvW);
                                }

                                // Center-Left
                                {
                                    t = w;

                                    // Middle
                                    for (int i = 0; i < verMiddleCount; i++)
                                    {
                                        b = t - stepY;
                                        AddQuad(vh, left, t, x, b, uvLeft, uvW, uvX, uvY);
                                        t = b;
                                    }
                                }

                                // Center
                                AddQuad(vh, x, w, z, y, uvX, uvW, uvZ, uvY);

                                // Center-Right
                                {
                                    t = w;

                                    // Middle
                                    for (int i = 0; i < verMiddleCount; i++)
                                    {
                                        b = t - stepY;
                                        AddQuad(vh, z, t, right, b, uvZ, uvW, uvRight, uvY);
                                        t = b;
                                    }
                                }

                                // Bottom
                                {
                                    l = left;
                                    t = y;
                                    r = x;
                                    b = bottom;

                                    // Left
                                    AddQuad(vh, l, t, r, b, uvLeft, uvY, uvX, uvBottom);
                                    l = r;
                                    // Middle
                                    for (int i = 0; i < horMiddleCount; i++)
                                    {
                                        r += stepX;
                                        AddQuad(vh, l, t, r, b, uvX, uvY, uvZ, uvBottom);
                                        l = r;
                                    }
                                    // Right
                                    AddQuad(vh, l, t, right, b, uvZ, uvY, uvRight, uvBottom);
                                }
                            }
                            else
                            {
                                AddSlicedTiledHorizontal(width);
                            }
                        }
                        else
                        {
                            if (tiledVertical)
                            {
                                AddSlicedTiledVertical(height);
                            }
                            else
                            {
                                AddQuad(vh, left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom);
                            }
                        }
                    }
                    else if (_imageType == ImageType.Tiled)
                    {
                        float width = right - left;
                        float height = top - bottom;
                        sprite.GetRectSize(out float spriteWidth, out float spriteHeight);
                        int columnCount = Mathf.CeilToInt((width - Mathf.Epsilon) / spriteWidth);
                        int rowCount = Mathf.CeilToInt((height - Mathf.Epsilon) / spriteHeight);
                        float t = top;
                        for (int i = 0; i < rowCount; i++)
                        {
                            float l = left;
                            float b = t - spriteHeight;
                            for (int j = 0; j < columnCount; j++)
                            {
                                float t2 = t;
                                float b2 = b;
                                if (t2 > bottom)
                                {
                                    float uvBottom2 = uvBottom;
                                    if (b2 < bottom)
                                    {
                                        b2 = bottom;
                                        uvBottom2 = uvTop - (uvTop - uvBottom) * (t2 - bottom) / spriteHeight;
                                    }

                                    float r = l + spriteWidth;
                                    float uvRight2 = uvRight;
                                    if (r > right)
                                    {
                                        r = right;
                                        uvRight2 = uvLeft + (uvRight - uvLeft) * (right - l) / spriteWidth;
                                    }

                                    AddQuad(vh, l, t2, r, b2, uvLeft, uvTop, uvRight2, uvBottom2);
                                }

                                l += spriteWidth;
                            }

                            t -= spriteHeight;
                        }
                    }
                    else
                    {
                        LegacyLog.Todo(_imageType);
                        AddQuad(vh, left, top, right, bottom, uvLeft, uvTop, uvRight, uvBottom);
                    }
                }
            });
        }

        protected override void SetNativeSize(Sprite sprite)
        {
            if (sprite == null) return;

            var size = sprite.rect.size;
            if (_imageType == ImageType.AddFlipHorizontal)
            {
                size.x *= 2;
            }
            else if (_imageType == ImageType.AddFlipVertical)
            {
                size.y *= 2;
            }
            else if (_imageType == ImageType.AddFlipAll)
            {
                size.x *= 2;
                size.y *= 2;
            }

            rectTransform.sizeDelta = size;
#if UNITY_EDITOR
            gameObject.SetDirty();
#endif
        }

#if UNITY_EDITOR
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUIHelper.ButtonCenter("Set Native Size"))
            {
                SetNativeSize(activeSprite);
            }
        }
#endif
    }
}