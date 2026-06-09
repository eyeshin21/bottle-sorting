using Anvil.Legacy;
using MarbleMania.Scripts.Game;
using UnityEngine;

namespace MarbleMania.LevelEditor
{
    public class LevelEditor : DestroyableSingletonBehaviour<LevelEditor>
    {
        [SerializeField] GameController _gameController;
        [SerializeField] private MainBoard _board;
        public MainBoard Board => _board;
        public void GenerateBoard(int toInt, int i)
        {
            
        }
    }
}