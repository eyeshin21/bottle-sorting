using UnityEngine;
using UnityEngine.UI;

namespace Anvil.Legacy
{
    public class ImageColor : MonoBehaviour, IColor
    {
        [SerializeField] Image _image;

        Color _color = Defaults.Color;

        public Color Color
        {
            get
            {
                if (_image != null)
                {
                    return _image.color;
                }
                LegacyLog.Warning($"Image is null! ({gameObject.GetHierarchyPath()})");
                return _color;
            }
            set
            {
                if (_image != null)
                {
                    _image.color = value;
                }
                else
                {
                    LegacyLog.Warning($"Image is null! ({gameObject.GetHierarchyPath()})");
                }
                _color = value;
            }
        }
    }
}