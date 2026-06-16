using Anvil;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace MarbleMania.Scripts.Game
{
    public class GameController : DestroyableSingletonBehaviour<GameController>
    {
        [SerializeField] private MainBoard _board;
        [SerializeField] private Conveyor _conveyor;
        [SerializeField] private CrateGrid _crateGrid;
        [SerializeField] private LevelData _testLevelData;
        
        [SerializeField] private ConveyorInlet _conveyorInlet;
        [SerializeField] private Transform _lowerMeshContainer;
        [SerializeField] private float crateGridDistance = 0.7f;

        private LevelData  _levelData;
        
        public LevelData LevelData => _levelData;
        protected override void Awake()
        {
            base.Awake();

            if (SceneManager.GetActiveScene().name == nameof(SceneName.LevelEditor))
            {
                return;
            }
            
            if (GameContext.LevelToLoad != null)
            {
                LoadGame(GameContext.LevelToLoad);
                return;
            }
            LoadGame(LevelData.LoadLevel("1"));
        }
       
        public void LoadGame(LevelData levelData)
        {
            _levelData = levelData;
            
            _board.Init(levelData.trayGridDatas);
            _conveyor.Init(_board.Size, levelData.conveyorSlot);

            Vector3 entryPoint = _conveyorInlet.transform.position;
            entryPoint = _conveyor.EvaluateDistanceWorld(_conveyor.FindClosestDistance(entryPoint));
            _conveyorInlet.transform.position = entryPoint;
            _lowerMeshContainer.position = entryPoint;
            
            entryPoint.z -= crateGridDistance;
            _crateGrid.transform.position = entryPoint;
            _crateGrid.Init(levelData.crateGridData);
        }

        // seperate so null check doesnt need to be in runtime
        public void LoadAsEditor(LevelData levelData)
        {
            _board.Init(levelData.trayGridDatas);
            _crateGrid.Init(levelData.crateGridData);
        }

        [Button]
        public void GenerateTestLevel()
        {
            LoadGame(_testLevelData);
        }

        public void OnLevelComplete()
        {
            UILoader.ShowConfirmPopup((popup) =>
            {
                popup.SetCancelButtonText("Replay");
                popup.SetConfirmButtonText("Level Edtor");
                popup.SetMessage("Level Completed");
            }, confirm =>
            {
                if (confirm)
                {
                    TransitionManager.LoadScene(SceneName.LevelEditor);
                }
                else
                {
                    LoadGame(GameContext.LevelToLoad);
                }
            });
        }

        public void Restart()
        { 
            LoadGame(GameContext.LevelToLoad);
        }

        public static void OnConveyorFull()
        {
            var validType = Instance._board.GetActiveIntakeType();
            if (validType == null)
            {
                Debug.Log("conveyor full, but no valid type");
                return;
            }

            foreach (Bottle bottle in Instance._conveyor.BottlesOnConveyor)
            {
                if(validType.Contains(bottle.ColorType))
                {
                    return;
                }
            }

            OnGameFailed();
        }

        private static void OnGameFailed()
        {
            UILoader.ShowConfirmPopup((popup) =>
            {
                popup.SetCancelButtonText("Replay");
                popup.SetConfirmButtonText("Level Edtor");
                popup.SetMessage("Level Failed");
            }, confirm =>
            {
                if (confirm)
                {
                    TransitionManager.LoadScene(SceneName.LevelEditor);
                }
                else
                {
                    Instance.LoadGame(GameContext.LevelToLoad);
                }
            });
        }
    }
}