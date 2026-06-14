using System;
using System.Collections.Generic;
using Anvil;
using Anvil.Legacy;
using NaughtyAttributes;
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

        [SerializeField, ElementName(typeof(CrateType))]
        private List<string> _crateTypeNameDictionary;

        [SerializeField] private GameObject _textIndicatedColorPrefab;
        [SerializeField] private Transform _colorCounterContainer;
        [SerializeField] private GameObject _crateButtonPrefab;
        [SerializeField] private Transform _buttonContainer;
        private CrateGrid _grid => LevelEditor.CrateGrid;
        private List<IndicatedLabledToggle> _crateButtons = new();
        private Crate _activeCrate;
        private Crate preview;
        private ColorType _currentColorType;

        private void Start()
        {
            LevelEditor.RebuildSignal += BuildData;
            LevelEditor.ReloadSignal += Reload;
            LevelEditor.ClearSignal += Clear;
            _generateButton.AddListener(OnBoardGenerate);

            for (var i = 0; i < GameConfig.CratePrefabs.Count; i++)
            {
                var prefab = GameConfig.CratePrefabs[i];
                if (prefab == null) continue;
                var crate = prefab.GetComponent<Crate>();
                var type = (CrateType)i;

                IndicatedLabledToggle button =
                    GameObjectPool.CreateObject<IndicatedLabledToggle>(_buttonContainer, _crateButtonPrefab);
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

        private void RebuildColorIndicator()
        {
            GameObjectPool.ClearManagedChild(_colorCounterContainer.gameObject);
            Dictionary<ColorType, int> colorCount = new Dictionary<ColorType, int>();
            foreach (var crate in _grid.Crates)
            {
                if (crate == null) continue;
                foreach (var bottle in crate.Bottles)
                {
                    ColorType colorType = bottle.ColorType;
                    if (!colorCount.ContainsKey(colorType))
                    {
                        colorCount[colorType] = 0;
                    }
                    
                    colorCount[colorType] += 1;
                }
            }

            foreach (var entry in colorCount)
            {
                var indicator =
                    GameObjectPool.CreateObject<TextIndicatedColor>(_colorCounterContainer, _textIndicatedColorPrefab);
                indicator.SetColor(entry.Key.ToColor());
                indicator.SetText(entry.Value.ToString());
            }
        }

        private void Clear()
        {
            _grid?.Clear();
        }

        private void Reload()
        {
            _colInput.text = _grid.ColCount.ToString();
            _rowInput.text = _grid.RowCount.ToString();
            RebuildColorIndicator();
        }

        private void BuildData()
        {
            LevelEditor.EditingData.crateGridData = _grid.CreateData();
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
            if (preview!= null)
            {
                Destroy(preview.gameObject);
                preview = null;
            }
        }

        private void Update()
        {
            if (_activeCrate == null)
                return;
            if (preview == null)
            {
                preview = GameObjectPool.CreateObject<Crate>(null, _activeCrate.gameObject, resetScale: false);
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
                RebuildColorIndicator();
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