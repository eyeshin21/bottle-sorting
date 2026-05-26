#region
using UnityEngine;
#endregion

namespace Anvil
{
    public class ColorController : MonoBehaviour,IColorChangable
    {
        public virtual void ChangeColor(Color color)
        {
            Debug.Log($"controller havent been inited. canot change color");
        }

        public virtual void ChangeColor(Gradient gradient)
        {
            Debug.Log($"controller havent been inited. canot change color");
        }

        public virtual void ChangeColor(Color minColor,Color maxColor)
        {
            Debug.Log($"controller havent been inited. canot change color");
        }
    }
}