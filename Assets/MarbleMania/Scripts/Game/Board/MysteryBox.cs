using System.Collections.Generic;
using MarbleMania.LevelEditor;
using UnityEngine;

namespace MarbleMania
{
    public class MysteryBox : Box
    {
        [SerializeField] private GameObject _mysteryIndicator;
        [SerializeField] private GameObject _mysteryIndicatorLE;
        private bool _isHidden;
        public override void Init(List<ColorType> colorData)
        {
            base.Init(colorData);
            _mysteryIndicator.SetActive(true);
        }

        public override void OnGridActive()
        {
            base.OnGridActive();
            SetHidden(false);
        }

        public void SetHidden(bool isHidden)
        {
            if (Editor.IsActive)
            {
                _mysteryIndicatorLE.SetActive(isHidden);
                _mysteryIndicator.SetActive(isHidden);
                return;
            }
            
            _isHidden = isHidden;
            _mysteryIndicator.SetActive(isHidden);
        }
        
    }
}