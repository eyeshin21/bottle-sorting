using Anvil.Legacy;
using MarbleMania.Scripts.Game;
using UnityEngine;

namespace MarbleMania.LevelEditor
{
    public class LevelEditor : DestroyableSingletonBehaviour<LevelEditor>
    {
        [SerializeField] GameController _gameController;
        [SerializeField] private MainBoard _board;
        [SerializeField] private CrateGrid _crateGrid;
        public MainBoard Board => _board;
        public static CrateGrid  CrateGrid => Instance._crateGrid;
        public void GenerateBoard(int toInt, int i)
        {
            
        }
    }
}