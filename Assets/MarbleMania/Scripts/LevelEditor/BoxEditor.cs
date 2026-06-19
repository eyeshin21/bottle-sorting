using System;
using System.Collections.Generic;
using Anvil;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MarbleMania.LevelEditor
{
    public class BoxEditor : DestroyableSingletonBehaviour<BoxEditor>
    {
        [SerializeField] private TMP_InputField _rowInput;
        [SerializeField] private TMP_InputField _colInput;
        [SerializeField] private UIButton _generateButton;

        [SerializeField, ElementName(typeof(BoxType))]
        private List<string> _crateTypeNameDictionary;

        [SerializeField] private GameObject _textIndicatedColorPrefab;
        [SerializeField] private Transform _colorCounterContainer;
        [SerializeField] private GameObject _crateButtonPrefab;
        [SerializeField] private Transform _buttonContainer;
        private CrateGrid _grid => Editor.CrateGrid;
        private List<IndicatedLabledToggle> _crateButtons = new();
        private Box _activeBox;
        private Box preview;
        private ColorType _currentColorType;

        private void Start()
        {
            Editor.RebuildSignal += BuildData;
            Editor.ReloadSignal += Reload;
            Editor.ClearSignal += Clear;
            _generateButton.AddListener(OnBoardGenerate);

            for (var i = 0; i < GameConfig.CratePrefabs.Count; i++)
            {
                var prefab = GameConfig.CratePrefabs[i];
                if (prefab == null) continue;
                var crate = prefab.GetComponent<Box>();
                var type = (BoxType)i;

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
                        
                    }

                });
                button.SetIsOn(false);
                _crateButtons.Add(button);
            }
        }

        public void RebuildColorIndicator()
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
            Editor.EditingData.crateGridData = _grid.CreateData();
        }

        private void OnCratePresetActive(Box box, BoxType type)
        {
            _activeBox = box;
            for (var i = 0; i < _crateButtons.Count; i++)
            {
                if (i == (int)type)
                {
                    continue;
                }

                _crateButtons[i].ForceSwitchOff();
            }
            Destroy(preview.gameObject);
            preview = null;
        }

        private void OnBoardGenerate()
        {
            _grid.Clear();
            _grid.Init(_rowInput.text.ToInt(), _colInput.text.ToInt());
            if (preview != null)
            {
                Destroy(preview.gameObject);
                preview = null;
            }
        }

        private void Update()
        {
            if (_activeBox == null)
                return;
            if (preview == null)
            {
                preview = GameObjectPool.CreateObject<Box>(null, _activeBox.gameObject, resetScale: false);
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

            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
            {
                var crate = _grid.GetCrate(coord.x, coord.y);
                _grid.RemoveCrate(coord.x, coord.y);
                // Destroy(crate?.gameObject);
                RebuildColorIndicator();
                return;
            }

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
            for (int i = 0; i < _activeBox.SlotCount; i++)
            {
                colorTypes.Add(type);
            }
            BoxData data = new BoxData()
            {
                type = _activeBox.Type,
                colorData = colorTypes,
                colorType = type
            };
            preview.Init(data);
            _currentColorType = type;
        }
    }
}