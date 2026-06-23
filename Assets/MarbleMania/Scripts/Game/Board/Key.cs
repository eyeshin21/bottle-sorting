using System;
using UnityEngine;

namespace MarbleMania
{
    public class Key : Box
    {
        [SerializeField] private MeshRenderer _renderer;
        private ColorType _color;
        public ColorType Color
        {
            get => _color;
            set
            {
                _color = value;
                _renderer.material = GameConfig.GetMaterial(value);
            }
        }
        public override void Init(BoxData boxData)
        {
            SetColor(boxData.colorType);
        }
        public override void SetColor(ColorType activeColor)
        {
            Color = activeColor;
        }

        public override void OnGridActive()
        {
            _grid.RemoveCrate(this);
        }

        public override void OnSelected()
        
        {
        }
        public override BoxData CreateData()
        {
            var data = new BoxData();
            data.type = Type;
            data.colorType = Color;
            return data;
        }
    }
}