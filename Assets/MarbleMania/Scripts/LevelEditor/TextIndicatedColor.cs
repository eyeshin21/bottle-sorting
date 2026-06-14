using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MarbleMania.LevelEditor
{
    public class TextIndicatedColor : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _image;

        public void SetColor(Color color)
        {
            _image.color = color;
        }

        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}