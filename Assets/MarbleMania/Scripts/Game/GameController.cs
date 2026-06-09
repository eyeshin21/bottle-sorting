using System;
using System.Collections.Generic;
using Anvil;
using Anvil.Legacy;
using NaughtyAttributes;
using UnityEngine;

namespace MarbleMania.Scripts.Game
{
    [Serializable]
    public class LevelData : ISerializable, IDeserializable
    {
        public List<TrayGridData> trayGridDatas = new List<TrayGridData>();
        public CrateGridData crateGridData;
        public int conveyorSlot;
        public string Serialize()
        {
            return string.Empty;
        }

        public void Deserialize(string data)
        {
        }
    }
    public class GameController : DestroyableSingletonBehaviour<GameController>
    {
        [SerializeField] private MainBoard _board;
        [SerializeField] private Conveyor _conveyor;
        [SerializeField] private CrateGrid _crateGrid;
        [SerializeField] private LevelData _testLevelData;
        
        [SerializeField] private ConveyorInlet _conveyorInlet;
        [SerializeField] private Transform _lowerMeshContainer;
        [SerializeField] private float crateGridDistance = 0.7f;
        protected override void Awake()
        {
            base.Awake();
            // GenerateGame(_testLevelData);
        }

        public void GenerateGame(LevelData levelData)
        {
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

        [Button]
        public void GenerateTestLevel()
        {
            GenerateGame(_testLevelData);
        }
    }
}