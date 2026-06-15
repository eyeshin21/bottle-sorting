using System;
using Anvil;
using MarbleMania.Scripts.Game;
using TMPro;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MarbleMania.LevelEditor
{
    public class LevelEditor : MonoBehaviour
    {
        private static LevelEditor _instance;
        public static LevelEditor Instance => _instance;
        public static bool IsPlayedFromEditor;
        
        [SerializeField] GameController _gameController;
        [SerializeField] private MainBoard _board;
        [SerializeField] private CrateGrid _crateGrid;
        
        [SerializeField] private TMP_InputField _levelInput;
        [SerializeField] private UIButton _loadButton;
        [SerializeField] private UIButton _saveButton;
        [SerializeField] private UIButton _clearButton;
        [SerializeField] private UIButton _playButton;
        public MainBoard Board => _board;
        public static CrateGrid  CrateGrid => Instance._crateGrid;
        
        [SerializeField] private Object _saveFolder;

        [SerializeField] private LevelData _levelData;
        public static LevelData EditingData
        {
            get => Instance._levelData;
        }
        public static string CurrentLevelID;
        
        public static Action RebuildSignal;
        public static Action ReloadSignal;
        public static Action ClearSignal;
        
        protected void Awake()
        {
            _instance = this;

            ClearSignal = null;
            RebuildSignal = null;
            ReloadSignal = null;
            
            _saveButton.AddListener(SaveCurrent);
            _loadButton.AddListener(LoadLevel);
            _clearButton.AddListener(Clear);
            _playButton.AddListener(OnPlay);

            IsPlayedFromEditor = true;

            if (!string.IsNullOrEmpty(CurrentLevelID))
            {
                LoadLevel(CurrentLevelID);
                CurrentLevelID = null;
            }
        }

        private void OnPlay()
        {
            GameContext.LevelToLoad = _levelData;
            TransitionManager.LoadScene(SceneName.MainGame);
        }

        private void Clear()
        {
            ClearSignal?.Invoke();
            _levelData = new LevelData();
        }

        private const string extension = ".json";
        private void LoadLevel()
        {
            var file = AssetDatabase.GetAssetPath(_saveFolder) + "/" + _levelInput.text + extension;
            LoadLevel(file);
        }

        private void LoadLevel(string file)
        {
            ClearSignal?.Invoke();
            EditingData.Deserialize(System.IO.File.ReadAllText(file));
            _gameController.LoadAsEditor(EditingData);
            ReloadSignal?.Invoke();
        }

        public void SaveCurrent()
        {
            BuildData();
            SaveAs(_levelInput.text);
        }

        private void BuildData()
        {
            RebuildSignal?.Invoke();
        }

        public void SaveAs(string filename)
        {
            string data = _levelData.Serialize();
            string path = AssetDatabase.GetAssetPath(_saveFolder);
            path = System.IO.Path.Combine(path, filename + extension);
            System.IO.File.WriteAllText(path, data);
        }
        public void GenerateBoard(int toInt, int i)
        {
            
        }
    }
}