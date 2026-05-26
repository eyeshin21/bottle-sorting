using UnityEngine;
using System;
using System.IO;

namespace Anvil.Legacy
{
    /*
     * width = 2, height = 3:
     * 
     * y
     * ^
     * |
     * +---+---+
     * | 2 | 5 |
     * +---+---+
     * | 1 | 4 |
     * +---+---+
     * | 0 | 3 |
     * +---+---+----> x
     *
     * pixels = [0, 3, 1, 4, 2, 5]
     * index = y * width + x
     */
    public static class TextureHelper
    {
        static readonly Color TransparentColor = new Color(0, 0, 0, 0);

        public static Texture2D CreateTexture(int width, int height, Color[] pixels, FilterMode? filterMode = null)
        {
            var texture = new Texture2D(width, height);
            texture.SetPixels(pixels);
            if (filterMode.HasValue)
            {
                texture.filterMode = filterMode.Value;
            }
            texture.Apply();

            return texture;
        }

        public static Texture2D CreateTexture(int width, int height, Color? color = null)
        {
            int count = width * height;
            var pixels = new Color[count];
            Color c = color ?? TransparentColor;
            for (int i = 0; i < count; i++)
            {
                pixels[i] = c;
            }

            return CreateTexture(width, height, pixels);
        }

        public static Texture2D CreateTexture(int width, int height, Action<Color[]> callback)
        {
            int count = width * height;
            var pixels = new Color[count];
            callback(pixels);

            return CreateTexture(width, height, pixels);
        }

        public static Texture2D RemoveCorners(Texture2D texture, int cornerSize)
        {
            int width = texture.width;
            int height = texture.height;
            if (cornerSize * 2 > Mathf.Max(width, height))
            {
                return texture;
            }

            var pixels = texture.GetPixels();

            var newTexture = new Texture2D(width, height);
            var color = new Color(0, 0, 0, 0);

            // Top
            {
                int index1 = (height - cornerSize) * width;
                int index2 = index1 + width - 1;

                for (int i = 0; i < cornerSize; i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        pixels[index1 + j] = color;
                        pixels[index2 - j] = color;
                    }

                    index1 += width;
                    index2 += width;
                }
            }

            // Bottom
            {
                int index1 = 0;
                int index2 = width - 1;

                for (int i = 0; i < cornerSize; i++)
                {
                    for (int j = 0; j < cornerSize - i; j++)
                    {
                        pixels[index1 + j] = color;
                        pixels[index2 - j] = color;
                    }

                    index1 += width;
                    index2 += width;
                }
            }

            newTexture.SetPixels(pixels);
            newTexture.Apply();

            return newTexture;
        }

#if UNITY_EDITOR
        public static Texture2D SetColor(Texture2D texture, Color color)
        {
            int width = texture.width;
            int height = texture.height;
            var pixels = texture.GetPixels();
            int pixelCount = pixels.Length;
            var newPixels = new Color[pixelCount];

            for (int i = 0; i < pixelCount; i++)
            {
                var pixel = pixels[i];
                if (pixel.a > 0)
                {
                    color.a = pixel.a;
                    newPixels[i] = color;
                }
            }

            return CreateTexture(width, height, newPixels);
        }

        public static Texture2D FlipHorizontal(Texture2D texture)
        {
            int width = texture.width;
            int height = texture.height;
            var pixels = texture.GetPixels();
            int pixelCount = pixels.Length;
            var newPixels = new Color[pixelCount];

            int index = 0, newIndex = width - 1;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    newPixels[newIndex] = pixels[index++];
                    newIndex--;
                }
                newIndex += 2 * width;
            }

            return CreateTexture(width, height, newPixels);
        }

        public static Texture2D FlipVertical(Texture2D texture)
        {
            int width = texture.width;
            int height = texture.height;
            var pixels = texture.GetPixels();
            int pixelCount = pixels.Length;
            var newPixels = new Color[pixelCount];

            int index = 0, newIndex = width * (height - 1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    newPixels[newIndex] = pixels[index++];
                    newIndex++;
                }
                newIndex -= 2 * width;
            }

            return CreateTexture(width, height, newPixels);
        }

        public static Texture2D AddFlipHorizontal(Texture2D texture)
        {
            int width = texture.width;
            int height = texture.height;
            var pixels = texture.GetPixels();
            int pixelCount = pixels.Length;

            int newWidth = width * 2;
            var newPixels = new Color[pixelCount * 2];

            int index = 0, newIndex = 0;
            for (int y = 0; y < height; y++)
            {
                int leftIndex = newIndex;
                int rightIndex = leftIndex + newWidth - 1;
                for (int x = 0; x < width; x++)
                {
                    var pixel = pixels[index++];
                    newPixels[leftIndex++] = pixel;
                    newPixels[rightIndex--] = pixel;
                }
                newIndex += newWidth;
            }

            return CreateTexture(newWidth, height, newPixels);
        }

        public static Texture2D AddFlipVertical(Texture2D texture)
        {
            int width = texture.width;
            int height = texture.height;
            var pixels = texture.GetPixels();
            int pixelCount = pixels.Length;

            int newHeight = height * 2;
            var newPixels = new Color[pixelCount * 2];

            int index = 0, topIndex = width * height, bottomIndex = topIndex - width;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var pixel = pixels[index++];
                    newPixels[bottomIndex++] = pixel;
                    newPixels[topIndex++] = pixel;
                }
                bottomIndex -= 2 * width;
            }

            return CreateTexture(width, newHeight, newPixels);
        }

        public static Texture2D AddFlipAll(Texture2D texture)
        {
            return AddFlipVertical(AddFlipHorizontal(texture));
        }

        public static Texture2D SplitHorizontal(Texture2D texture)
        {
            int width = texture.width;
            int height = texture.height;
            var pixels = texture.GetPixels();
            int newWidth = (width + 1) >> 1;
            var newPixels = new Color[newWidth * height];

            int index = 0, index2 = 0, newIndex = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    newPixels[newIndex++] = pixels[index2++];
                }
                index += width;
                index2 = index;
            }

            return CreateTexture(newWidth, height, newPixels);
        }

        public static Texture2D RotateLeft(Texture2D texture)
        {
            int width = texture.width;
            int height = texture.height;
            var pixels = texture.GetPixels();
            int pixelCount = pixels.Length;

            int newWidth = height;
            int newHeight = width;
            var newPixels = new Color[pixelCount];

            int index = 0, newIndex;
            for (int x = newWidth - 1; x >= 0; x--)
            {
                newIndex = x;
                for (int y = 0; y < newHeight; y++)
                {
                    newPixels[newIndex] = pixels[index++];
                    newIndex += newWidth;
                }
            }

            return CreateTexture(newWidth, newHeight, newPixels);
        }

        public static Texture2D RotateRight(Texture2D texture)
        {
            int width = texture.width;
            int height = texture.height;
            var pixels = texture.GetPixels();
            int pixelCount = pixels.Length;

            int newWidth = height;
            int newHeight = width;
            var newPixels = new Color[pixelCount];

            int index = 0, newIndex;
            for (int x = 0; x < newWidth; x++)
            {
                newIndex = newWidth * (newHeight - 1) + x;
                for (int y = 0; y < newHeight; y++)
                {
                    newPixels[newIndex] = pixels[index++];
                    newIndex -= newWidth;
                }
            }

            return CreateTexture(newWidth, newHeight, newPixels);
        }

        public static Texture2D Trim(Texture2D texture, float alphaThreshold)
        {
            int width = texture.width;
            int height = texture.height;
            int left = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (texture.GetPixel(x, y).a >= alphaThreshold)
                    {
                        left = x;
                        x = width;
                        break;
                    }
                }
            }

            int right = width - 1;
            for (int x = width - 1; x >= 0; x--)
            {
                for (int y = 0; y < height; y++)
                {
                    if (texture.GetPixel(x, y).a >= alphaThreshold)
                    {
                        right = x;
                        x = -1;
                        break;
                    }
                }
            }

            int bottom = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (texture.GetPixel(x, y).a >= alphaThreshold)
                    {
                        bottom = y;
                        y = height;
                        break;
                    }
                }
            }

            int top = height - 1;
            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = 0; x < width; x++)
                {
                    if (texture.GetPixel(x, y).a >= alphaThreshold)
                    {
                        top = y;
                        y = -1;
                        break;
                    }
                }
            }

            int newWidth = right - left + 1;
            int newHeight = top - bottom + 1;
            var newPixels = new Color[newWidth * newHeight];

            int index = 0;
            for (int y = bottom; y <= top; y++)
            {
                for (int x = left; x <= right; x++)
                {
                    newPixels[index++] = texture.GetPixel(x, y);
                }
            }

            return CreateTexture(newWidth, newHeight, newPixels);
        }

        public static Texture2D Scale(Texture2D texture, int newWidth, int newHeight)
        {
            texture.filterMode = FilterMode.Point;
            RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
            rt.filterMode = FilterMode.Point;
            RenderTexture.active = rt;
            Graphics.Blit(texture, rt);

            var newTexture = new Texture2D(newWidth, newHeight);
            newTexture.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
            newTexture.Apply();
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);

            return newTexture;
        }

        public static void SetPixels(Texture2D texture, Color[] dstPixels, int dstTextureWidth, int dstX, int dstY)
        {
            int textureWidth = texture.width;
            int textureHeight = texture.height;
            var srcPixels = texture.GetPixels();
            int dstIndex = dstY * dstTextureWidth + dstX;
            int srcIndex = 0;

            for (int i = 0; i < textureHeight; i++)
            {
                int index = dstIndex;
                for (int j = 0; j < textureWidth; j++)
                {
                    dstPixels[index++] = srcPixels[srcIndex++];
                }
                dstIndex += dstTextureWidth;
            }
        }

        public static void SetPixels(Color[] srcPixels, int srcTextureWidth, int srcX, int srcY, int srcWidth, int srcHeight,
                                    Color[] dstPixels, int dstTextureWidth, int dstX, int dstY)
        {
            int srcIndex = srcY * srcTextureWidth + srcX;
            int dstIndex = dstY * dstTextureWidth + dstX;
            for (int i = 0; i < srcHeight; i++)
            {
                int index1 = srcIndex;
                int index2 = dstIndex;
                for (int j = 0; j < srcWidth; j++)
                {
                    dstPixels[index2++] = srcPixels[index1++];
                }
                srcIndex += srcTextureWidth;
                dstIndex += dstTextureWidth;
            }
        }

        public static bool IsEquals(Texture2D texture1, Texture2D texture2)
        {
            var pixels1 = texture1.GetPixels();
            var pixels2 = texture2.GetPixels();
            int length1 = pixels1.Length;
            int length2 = pixels2.Length;
            if (length1 != length2) return false;
            for (int i = 0; i < length1; i++)
            {
                if (!pixels1[i].Equals(pixels2[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static Texture2D LoadTexture(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            if (bytes != null)
            {
                var texture = new Texture2D(2, 2);
                texture.LoadImage(bytes); // This will auto-resize the texture dimensions
                return texture;
            }

            LegacyLog.Warning($"Can't load texture at \"{path}\"!");
            return null;
        }

        public static bool SaveTexture(Texture2D texture, string folderPath, string fileName)
        {
            return SaveTexture(texture, $"{folderPath}{Constants.PathSeparator}{fileName}.png");
        }

        public static bool SaveTexture(Texture2D texture, string path)
        {
            var bytes = texture.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
            return true;
        }
#endif
    }
}