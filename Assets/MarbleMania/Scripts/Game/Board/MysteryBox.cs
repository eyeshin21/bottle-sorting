using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Editor = MarbleMania.LevelEditor.Editor;

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
            _grid.AddOnBoxRemoved(OnBoxRemove);
        }

        private void OnDestroy()
        {
            _grid.RemoveOnBoxRemoved(OnBoxRemove);
        }

        private void OnBoxRemove(Box box)
        {
            int r = box.row;
            int c = box.col;
            if (((r == row + 1 || r == row - 1) && c == col)
                || ((c == col + 1 || c == col - 1) && r == row))
            {
                EditorGUIUtility.PingObject(gameObject);
                Debug.Log($"unlock at {row}.{col}");
                SetHidden(false);
                foreach (var bottle in Bottles)
                {
                    bottle.gameObject.SetActive(true);
                }
            }
        }

        public override void SetColor(ColorType activeColor)
        {
            base.SetColor(activeColor);
            
            foreach (var bottle in Bottles)
            {
                bottle.SetColor(activeColor);
            }

            if (Editor.IsActive)
            {
                _colorIndicator.color = activeColor.ToColor();
            }
            
            if (Editor.IsActive)
            {
                DisplayAsEditor(activeColor);
            }else
                SetHidden(true);
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

       
    }
}