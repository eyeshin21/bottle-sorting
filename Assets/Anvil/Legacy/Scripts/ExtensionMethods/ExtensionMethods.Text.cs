using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public static partial class ExtensionMethods
    {
        public static Vector2 GetSize(this Text uiText)
        {
            var text = uiText.text;
            var generator = new TextGenerator();
            var settings = uiText.GetGenerationSettings(uiText.rectTransform.rect.size);
            float width = generator.GetPreferredWidth(text, settings);
            float height = generator.GetPreferredHeight(text, settings);
            return new Vector2(width, height);
        }
    }
}