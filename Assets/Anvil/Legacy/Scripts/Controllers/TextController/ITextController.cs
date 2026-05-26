using UnityEngine;

namespace Anvil.Legacy
{
    public interface ITextController : IController
    {
        string Text { get; set; }
        float FontSize { get; set; }
        TextAnchor Alignment { get; set; }
        Color Color { get; set; }
        bool ResizeEnabled { get; set; }
        bool WrappingEnabled { get; set; }

        Vector2 TextSize { get; }
        void SetText(object text);
        void ForceUpdateText();
    }
}