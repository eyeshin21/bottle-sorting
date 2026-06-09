using System.Collections.Generic;
using Anvil;
using Anvil.Legacy;
using TMPro;
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

        [SerializeField, ElementName(typeof(TrayType))]
        private List<string> _trayTypeNameDictionary;

        [SerializeField] private GameObject _trayButtonPrefab;
        [SerializeField] private GameObject _togglePrefab;
        [SerializeField] private Transform _buttonContainer;
        [SerializeField] private Transform _layerToggleContainer;

        private List<TrayGrid> _grids = new List<TrayGrid>();
        private List<ToggleLabledIconButton> _layerToggles = new();
        private List<UIButton> _trayButtons = new List<UIButton>();
        private TrayGrid _currentGrid;
        public int layerIndex = 0;
        private MainBoard _Board => LevelEditor.Instance.Board;

        public void Awake()
        {
            _generateButton.AddListener(OnBoardGenerate);
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
        }

        private void OnBoardGenerate()
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
                .GetComponent<ToggleLabledIconButton>();
            _layerToggles.Add(toggle);
            toggle.RegisterOnToggleAction((state) => { OnLayerToggle(toggle); });
            toggle.SetIsOn(true);
            toggle.SetLabel("Layer " + layerIndex);

            var grid =_Board.GenerateLayer(layerIndex, _rowInput.text.ToInt(), _colInput.text.ToInt());
            _grids.Add(grid);
            SetActiveLayer(layerIndex);
        }

        private void OnLayerToggle(ToggleLabledIconButton toggle)
        {
            for (var i = 0; i < _layerToggles.Count; i++)
            {
                var layerToggle = _layerToggles[i];
                if (toggle == layerToggle)
                {
                    layerIndex = i;
                    SetActiveLayer(layerIndex);
                    Debug.Log($"select {i}");
                    continue;
                }
                layerToggle.ForceSwitchOff();
            }
        }
    }
}