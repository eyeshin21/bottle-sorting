using UnityEngine;

namespace Anvil.Legacy
{
    public static partial class GUIHelper
    {
        public static float GetMaxLabelWidth(GUIContent content1, GUIContent content2)
        {
            return Mathf.Max(GetLabelWidth(content1), GetLabelWidth(content2));
        }

        public static GUIContent CreateContent(Sprite sprite)
        {
            return new GUIContent(sprite?.texture);
        }

        public static GUIContent[] GetColorContents<T>(Func<T, Color> colorFunc) where T : struct
        {
            var values = Helper.GetEnumValues<T>();
            int count = values.Length;
            var contents = new GUIContent[count];
            int size = (int)LineHeight;
            for (int i = 0; i < count; i++)
            {
                var color = colorFunc(values[i]);
                contents[i] = new GUIContent(TextureHelper.CreateTexture(size, size, color));
            }
            return contents;
        }
    }
}