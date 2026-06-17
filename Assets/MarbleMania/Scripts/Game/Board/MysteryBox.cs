using System.Collections.Generic;
using MarbleMania.LevelEditor;
using UnityEngine;

namespace MarbleMania
{
    public class MysteryBox : Box
    {
        [SerializeField] private GameObject _mysteryIndicator;
        [SerializeField] private GameObject _mysteryIndicatorLE;
        [SerializeField] private SpriteRenderer _colorIndicator;
        private bool _isHidden;
        public override void Init(List<ColorType> colorData)
        {
            base.Init(colorData);
            _isHidden = true;
            SetHidden(true);

            if (Editor.IsActive)
            {
                DisplayAsEditor(colorData);
            }
        }

        public override void OnGridActive()
        {
            base.OnGridActive();
            SetHidden(false);
            foreach (var bottle in Bottles)
            {
                bottle.gameObject.SetActive(true);
            }
        }
        public void SetHidden(bool isHidden)
        {
            _isHidden = isHidden;
            _mysteryIndicator.SetActive(isHidden);
            if (_isHidden)
            {
                foreach (var bottle in Bottles)
                {
                    bottle.gameObject.SetActive(false);
                }
            }
        }

        public void DisplayAsEditor(List<ColorType> colorData)
        {
            _mysteryIndicatorLE.SetActive(true);
            _mysteryIndicator.SetActive(false);
            ColorType colorType = colorData[0];
            _colorIndicator.color = colorType.ToColor();
        }

        public void SetColor(ColorType activeColor)
        {
            foreach (var bottle in Bottles)
            {
                bottle.SetColor(activeColor);
            }

            if (Editor.IsActive)
            {
                _colorIndicator.color = activeColor.ToColor();
            }
        }
    }
}