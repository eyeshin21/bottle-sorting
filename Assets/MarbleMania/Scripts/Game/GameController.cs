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
        protected override void Awake()
        {
            base.Awake();
            GenerateGame(_testLevelData);
        }

        public void GenerateGame(LevelData levelData)
        {
            _board.Init(levelData.trayGridDatas);
            _crateGrid.Init(levelData.crateGridData);
        }

        [Button]
        public void GenerateTestLevel()
        {
            GenerateGame(_testLevelData);
        }
    }
}