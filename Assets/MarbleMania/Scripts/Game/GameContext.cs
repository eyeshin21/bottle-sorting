using MarbleMania.Scripts.Game;

namespace MarbleMania
{
    public static class GameContext
    {
        private static LevelData _levelToLoad;
        private static int _levelIndex;
        public static int LevelIndex
        {
            get => _levelIndex;
            set => _levelIndex = value;
        }
        public static LevelData LevelToLoad
        {
            get => _levelToLoad;
            set => _levelToLoad = value;
        }

    }
}