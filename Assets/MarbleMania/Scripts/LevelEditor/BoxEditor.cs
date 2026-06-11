using System;
using System.Collections.Generic;
using Anvil;
using Anvil.Legacy;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MarbleMania.LevelEditor
{
    public class BoxEditor : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _rowInput;
        [SerializeField] private TMP_InputField _colInput;
        [SerializeField] private UIButton _generateButton;

        [SerializeField, ElementName(typeof(CrateType))] private List<string> _crateTypeNameDictionary;
        
        [SerializeField] private GameObject _crateButtonPrefab;
        [SerializeField] private Transform _buttonContainer;
        
        private CrateGrid _grid;
        private List<IndicatedLabledToggle> _crateButtons = new ();
        private Crate _activeCrate;
        private Crate preview;
        private ColorType _currentColorType;
        private void Awake()
        {
            _grid = LevelEditor.CrateGrid;
            _generateButton.AddListener(OnBoardGenerate);
            
            for (var i = 0; i < GameConfig.CratePrefabs.Count; i++)
            {
                var prefab = GameConfig.CratePrefabs[i];
                if (prefab == null) continue;
                var crate = prefab.GetComponent<Crate>();
                var type = (CrateType)i;
                
                IndicatedLabledToggle button = GameObjectPool.CreateObject<IndicatedLabledToggle>(_buttonContainer, _crateButtonPrefab);
                button.SetLabel(_crateTypeNameDictionary[i]);
                button.RegisterOnToggleAction((state) =>
                {
                    if (state)
                    {
                        OnCratePresetActive(crate, type);
                    }
                    else
                    {
                        Destroy(preview);
                        _activeCrate = null;
                    }
                });
                button.SetIsOn(false);
                _crateButtons.Add(button);
            }
        }

        private void OnCratePresetActive(Crate crate, CrateType type)
        {
            _activeCrate = crate;
            for (var i = 0; i < _crateButtons.Count; i++)
            {
                if (i == (int)type)
                {
                    continue;
                }
                _crateButtons[i].ForceSwitchOff();
            }
        }

        private void OnBoardGenerate()
        {
            _grid.Init(_rowInput.text.ToInt(), _colInput.text.ToInt());
            Destroy(preview.gameObject);
            preview = null;
        }
        
        private void Update()
        {
            if (_activeCrate == null)
                return;
            if (preview == null)
            {
                preview = GameObjectPool.CreateObject<Crate>(null,  _activeCrate.gameObject, resetScale: false);
                preview.transform.localScale *= _grid.transform.localScale.x;
                UpdateContentColor();
                preview.gameObject.SetActive(false);
            }

            if (_currentColorType != ColorManager.activeColor)
            {
                UpdateContentColor();
            }
            var worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.y = 0;

            if (!_grid.IsPointInside(worldPoint))
            {
                preview.gameObject.SetActive(false);
                return;
            }
            var coord = _grid.ConvertToGridCoordinates(worldPoint);
            bool isvalid = _grid.IsValidForNewCrate(coord);
            if (!isvalid)
            {
                preview.gameObject.SetActive(false);
                return;
            }
            preview.gameObject.SetActive(true);
            Vector3 cellPosition = _grid.GetCellLocalPosition(coord.x, coord.y);
            cellPosition = _grid.transform.TransformPoint(cellPosition);
            // preview.transform.position = cellPosition;
            preview.transform.position = cellPosition;
            float size = 0.3f;
            if (Input.GetMouseButtonDown(0))
            {
                _grid.RegisterCrate(preview, coord.x, coord.y);
                preview = null;
            }            
        }

        private void UpdateContentColor()
        {
            ColorType type = ColorManager.activeColor;
            List<ColorType> colorTypes = new List<ColorType>();
            for (int i = 0; i < _activeCrate.SlotCount; i++)
            {
                colorTypes.Add(type);
            }
            preview.Generate(colorTypes);
            _currentColorType = type;
            Debug.Log($"actice {type}");
        }
    }
}