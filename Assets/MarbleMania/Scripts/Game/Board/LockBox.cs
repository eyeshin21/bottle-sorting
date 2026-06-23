using System;
using Anvil;
using UnityEngine;

namespace MarbleMania
{
    public class LockBox : Box, IBoxCollector
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private GameObject _lockObject;
        private bool _isLocked;
        private ColorType _color;

        public ColorType Color
        {
            get => _color;
            set
            {
                Debug.Log("color set");
                _color = value;
                _renderer.material = GameConfig.GetMaterial(value);
                
            }
        }

        public override void Init(BoxData boxData)
        {
            base.Init(boxData);
            SetLocked(true);
            // _grid?.AddOnBoxRemoved(OnBoxRemove);
            // Color = Enum.Parse<ColorType>(boxData.customData);
            _grid?.RegisterBoxCollector(this);
            SetColor(boxData.colorType);
        }

        public override void SetColor(ColorType activeColor)
        {
            Color = activeColor;
        }

        private void OnBoxRemove(Box box)
        {
            if (box is not Key key) return;
            if (key.Color == Color)
            {
                SetLocked(false);
            }
        }

        private void SetLocked(bool locked)
        {
            _isLocked = locked;
            _lockObject.SetActive(locked);
        }

        public override BoxData CreateData()
        {
            var data = base.CreateData();
            data.colorType = Color;
            return data;
        }

        public bool TryCollect(Box box)
        {
            if (box is not Key key) return false;
            if (key.Color == Color)
            {
                SetLocked(false);
                _grid?.UnRegisterBoxCollector(this);
                return true;
            }
            GameObjectPool.RemoveObject(key.gameObject);
            return false;
        }
    }
}