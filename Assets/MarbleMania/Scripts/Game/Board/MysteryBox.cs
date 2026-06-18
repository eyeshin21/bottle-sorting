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
        public override void Init(BoxData data)
        {
            base.Init(data);
            _isHidden = true;
            SetHidden(true);
            List<ColorType> colorData = data.colorData;
            if (Editor.IsActive)
            {
                ColorType  colorType = colorData[0];
                DisplayAsEditor(colorType);
            }
        }

        public override void Init(ColorType activeColor)
        {
            base.Init(activeColor);
            if (Editor.IsActive)
            {
                DisplayAsEditor(activeColor);
            }else
                SetHidden(true);
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

        public void DisplayAsEditor(ColorType type)
        {
            _mysteryIndicatorLE.SetActive(true);
            _mysteryIndicator.SetActive(false);
            ColorType colorType = type;
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