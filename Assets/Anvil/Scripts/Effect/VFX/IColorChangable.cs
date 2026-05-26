using UnityEngine;

namespace Anvil
{
    public interface IColorChangable
    {
        void ChangeColor(Color color);
        void ChangeColor(Gradient gradient);
        void ChangeColor(Color minColor, Color maxColor);
    }
}
