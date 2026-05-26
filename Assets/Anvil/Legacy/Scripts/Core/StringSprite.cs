using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anvil.Legacy
{
    [System.Serializable]
    public class StringSprite
    {
        [SerializeField] string _string;
        [SerializeField] Sprite _sprite;

        public string String => _string;
        public Sprite Sprite => _sprite;
    }

    public static partial class ExtensionMethods
    {
        public static Dictionary<string, Sprite> ToDictionary(this StringSprite[] a)
        {
            int count = a.GetLength();
            var dict = new Dictionary<string, Sprite>(count);
            for (int i = 0; i < count; i++)
            {
                var item = a[i];
                var key = item.String;
                if (dict.ContainsKey(key))
                {
                    LegacyLog.Warning($"Duplicate key \"{key}\"");
                }
                else
                {
#if DEBUG_MODE
                    if (item.Sprite == null)
                    {
                       LegacyLog.Warning($"{key}: Missing sprite");
                    }
#endif
                    dict.Add(key, item.Sprite);
                }
            }
            return dict;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(StringSprite))]
    public class StringSpriteDrawer : PropertyDrawer
    {
        static TwoPropertyDrawer _propertyDrawer = new("", "_string", "", "_sprite");
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _propertyDrawer.OnGUI(position, property, label);
        }
    }
#endif
}