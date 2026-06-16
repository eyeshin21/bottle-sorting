using System;
using System.Collections.Generic;
using Anvil;
using Drawing;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace MarbleMania.LevelEditor
{
    public struct String2
    {
        public string key;
        public string value;
    }

    public class TrayEditor : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _rowInput;
        [SerializeField] private TMP_InputField _colInput;
        [SerializeField] private UIButton _generateButton;
        [SerializeField] private UIButton _addButton;

        [SerializeField, ElementName(typeof(TrayType))]
        private List<string> _trayTypeNameDictionary;

        [SerializeField] private GameObject _trayButtonPrefab;
        [SerializeField] private GameObject _togglePrefab;
        [SerializeField] private Transform _buttonContainer;
        [SerializeField] private Transform _layerToggleContainer;
        [SerializeField] private GameObject _textIndicatedColorPrefab;
        [SerializeField] private Transform _colorCounterContainer;

        private List<TrayGrid> _grids = new List<TrayGrid>();
        private List<IndicatedLabledToggle> _layerToggles = new();
        private List<IndicatedLabledToggle> _trayButtons = new();
        private TrayGrid _currentGrid;
        public int layerIndex = 0;
        private MainBoard _Board => LevelEditor.Instance.Board;

        private Tray _activeTray;

        public void Start()
        {
            LevelEditor.RebuildSignal += BuildData;
            LevelEditor.ReloadSignal += Reload;
            LevelEditor.ClearSignal += ClearAll;
            _generateButton.AddListener(OnBoardGenerate);
            _addButton.AddListener(OnBoardAdd);

            GenerateTrayPreset();
        }

        private void ClearAll()
        {
            Clear();
            _Board.Clear();
        }

        private void Clear()
        {
            _currentGrid = null;
            _grids.Clear();
            _layerToggles.Clear();
            GameObjectPool.ClearManagedChild(_layerToggleContainer.gameObject);
        }

        private void Reload()
        {
            Clear();
            layerIndex = 0;
            foreach (var grid in _Board.Grids)
            {
                var toggle = GameObjectPool.CreateObject(_layerToggleContainer, _togglePrefab)
                    .GetComponent<IndicatedLabledToggle>();
                _layerToggles.Add(toggle);
                toggle.RegisterOnToggleAction((state) => { OnLayerToggle(toggle); });
                toggle.ForceSwitchOff();
                toggle.SetLabel("L" + layerIndex);
                layerIndex++;
                _grids.Add(grid);
            }

            layerIndex = 0;
            SetActiveLayer(0);
            RebuildColorIndicator();
        }

        private void BuildData()
        {
            List<TrayGridData> data = new List<TrayGridData>();
            foreach (TrayGrid grid in _grids)
            {
                data.Add(grid.CreateData());
            }

            LevelEditor.EditingData.trayGridDatas = data;
        }

        private void RebuildColorIndicator()
        {
            GameObjectPool.ClearManagedChild(_colorCounterContainer.gameObject);
            Dictionary<ColorType, int> colorCount = new Dictionary<ColorType, int>();
            foreach (var grid in _grids)
            {
                foreach (var tray in grid.Trays)
                {
                    if (tray == null) continue;
                    ColorType colorType = tray.ColorType;
                    if (!colorCount.ContainsKey(colorType))
                    {
                        colorCount[colorType] = 0;
                    }

                    colorCount[colorType] = colorCount[colorType] + tray.SlotCount;
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

        private void GenerateTrayPreset()
        {
            for (var i = 0; i < GameConfig.TrayPrefabs.Count; i++)
            {
                var prefab = GameConfig.TrayPrefabs[i];
                if (prefab == null) continue;
                var tray = prefab.GetComponent<Tray>();
                var type = (TrayType)i;

                IndicatedLabledToggle button =
                    GameObjectPool.CreateObject<IndicatedLabledToggle>(_buttonContainer, _trayButtonPrefab);
                button.SetLabel(_trayTypeNameDictionary[i]);
                button.RegisterOnToggleAction((state) =>
                {
                    if (state)
                    {
                        OnTrayPresetActive(tray, type);
                    }
                    else
                    {
                        Destroy(preview);
                        _activeTray = null;
                    }
                });
                button.SetIsOn(false);
                _trayButtons.Add(button);
            }
        }

        private void OnTrayPresetActive(Tray tray, TrayType type)
        {
            if (preview != null)
            {
                Destroy(preview.gameObject);
                preview = null;
            }
            _activeTray = tray;
            for (var i = 0; i < _trayButtons.Count; i++)
            {
                if (i == (int)type)
                {
                    continue;
                }

                _trayButtons[i].ForceSwitchOff();
            }
        }


        private void SetActiveLayer(int index)
        {
            if (_currentGrid != null)
            {
                _currentGrid.drawDebug = false;
                _currentGrid.gameObject.SetActive(false);
            }

            TrayGrid grid = _grids.TryGet(index);
            if (grid == null)
            {
                Debug.LogError($"No grid found for index {index}");
                return;
            }

            grid.drawDebug = true;
            _currentGrid = grid;
            _currentGrid.gameObject.SetActive(true);
            _layerToggles.TryGet(index)?.ForceSwitchOn();
            foreach (TrayGrid trayGrid in _grids)
            {
                if (trayGrid == _currentGrid) continue;
                trayGrid.drawDebug = false;
                trayGrid.gameObject.SetActive(false);
            }
        }

        private void OnBoardGenerate()
        {
            if (_grids.Count == 0)
            {
                OnBoardAdd();
            }

            var grid = _Board.GenerateLayer(layerIndex, _rowInput.text.ToInt(), _colInput.text.ToInt());
            RebuildColorIndicator();
        }

        private void OnBoardAdd()
        {
            if (_grids.Count == 0)
            {
                layerIndex = 0;
            }
            else
            {
                layerIndex++;
            }

            var toggle = GameObjectPool.CreateObject(_layerToggleContainer, _togglePrefab)
                .GetComponent<IndicatedLabledToggle>();
            _layerToggles.Add(toggle);
            toggle.RegisterOnToggleAction((state) => { OnLayerToggle(toggle); });
            toggle.SetIsOn(true);
            toggle.SetLabel("L" + layerIndex);

            var grid = _Board.GenerateLayer(layerIndex, _rowInput.text.ToInt(), _colInput.text.ToInt());
            _grids.Add(grid);
            SetActiveLayer(layerIndex);
        }

        private void OnLayerToggle(IndicatedLabledToggle indicatedLabledToggle)
        {
            for (var i = 0; i < _layerToggles.Count; i++)
            {
                var layerToggle = _layerToggles[i];
                if (indicatedLabledToggle == layerToggle)
                {
                    layerIndex = i;
                    SetActiveLayer(layerIndex);
                    Debug.Log($"select {i}");
                    continue;
                }

                layerToggle.ForceSwitchOff();
            }
        }

        private Tray preview = null;

        private void Update()
        {
            if (_activeTray == null)
                return;
            if (preview == null)
            {
                preview = GameObjectPool.CreateObject<Tray>(null, _activeTray.gameObject);
                preview.Init(new TrayPositionData()
                {
                    trayColor = ColorType.Blue,
                });
                preview.gameObject.SetActive(false);
            }

            if (preview.ColorType != ColorManager.activeColor)
            {
                preview.InitColor(ColorManager.activeColor);
            }

            var worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.y = 0;

            if (!_currentGrid.IsPointInside(worldPoint))
            {
                preview.gameObject.SetActive(false);
                return;
            }

            var cell = _currentGrid.GetCellNear(worldPoint, out var _);
            
            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
            {
                var tray = _currentGrid.RemoveTray(cell.Row, cell.Column);
                RebuildColorIndicator();
                Destroy(tray.gameObject);
                return;
            }
            
            bool isvalid = _currentGrid.IsValid(preview, cell, out List<TrayGridCell> cells);
            if (!isvalid)
            {
                preview.gameObject.SetActive(false);
                return;
            }

            preview.gameObject.SetActive(true);
            Vector3 cellPosition = _currentGrid.transform.TransformPoint(cell.localPosition);
            // preview.transform.position = cellPosition;
            preview.SetPositionToCenter(cellPosition);
            float size = 0.3f;
            foreach (var gridCell in cells)
            {
                Vector3 pos = _currentGrid.transform.TransformPoint(gridCell.localPosition);
                Draw.WireBox(new float3(pos.x, pos.y + 0.01f, pos.z), new float3(size, size, size), Color.green);
            }

            if (Input.GetMouseButtonDown(0))
            {
                _currentGrid.RegisterTray(preview, cell.Row, cell.Column);
                preview = null;
                RebuildColorIndicator();
            }
            
        }
    }
}