using Anvil.Legacy;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace MarbleMania
{
    public enum SceneName
    {
        MainGame,
        LevelEditor,
    }
    public class TransitionManager : SingletonBehaviour<TransitionManager>
    {
        public static void LoadScene(SceneName sceneName)
        {
            SceneManager.LoadScene((int)sceneName, LoadSceneMode.Single);
        }


        [MenuItem("Drunk/Load MainGame")]
        public static void LoadMainGame()
        {
            EditorSceneManager.OpenScene($"Assets/Scenes/{nameof(SceneName.MainGame)}.unity");
        }

        [MenuItem("Drunk/Load LevelEditor")]
        public static void LoadLevelEditor()
        {
            EditorSceneManager.OpenScene($"Assets/Scenes/{nameof(SceneName.LevelEditor)}.unity");
        }
    }
}