using MarbleMania.Scripts.Game;

namespace MarbleMania
{
    public static class GameContext
    {
        private static LevelData _levelToLoad;

        public static LevelData LevelToLoad
        {
            get => _levelToLoad;
            set => _levelToLoad = value;
        }
    }
}