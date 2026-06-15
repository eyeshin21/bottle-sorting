using System;
using System.Collections.Generic;
using Anvil;
using UnityEngine;

namespace MarbleMania.LevelEditor
{
    public class ColorManager : MonoBehaviour
    {
        public  static ColorManager Instance { get; private set; }
        
        [SerializeField] private GameObject _colorTogglePrefab;
        [SerializeField] private Transform _colorToggleParent;

        public static ColorType activeColor;
        private List<IndicatedLabledToggle> _colorToggles = new List<IndicatedLabledToggle>();
        
        private void Start()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            foreach (ColorType color in Enum.GetValues(typeof(ColorType)))
            {
                var toggle = GameObjectPool.CreateObject<IndicatedLabledToggle>(_colorToggleParent, _colorTogglePrefab);
                toggle.SetDisplayButtonColor(GameConfig.GetColor(color));
                _colorToggles.Add(toggle);
                toggle.RegisterOnToggleAction((state) =>
                {
                    if (state)
                    {
                        SetActiveColor(color);
                        SetActiveToggle(toggle);
                    }
                });
            }
        }

        private void SetActiveToggle(IndicatedLabledToggle indicatedLabledToggle)
        {
            foreach (var VARIABLE in _colorToggles)
            {
                if (VARIABLE != indicatedLabledToggle)
                {
                    VARIABLE.SetIsOn(false);
                }
            }
        }

        private void SetActiveColor(ColorType color)
        {
            activeColor = color;
        }
    }
}