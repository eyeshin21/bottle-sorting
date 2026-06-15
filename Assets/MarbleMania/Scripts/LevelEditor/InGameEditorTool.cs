using System;
using Anvil;
using MarbleMania.Scripts.Game;
using UnityEngine;

namespace MarbleMania.LevelEditor
{
    public class InGameEditorTool : MonoBehaviour
    {
        [SerializeField] private UIButton _editorButton;
        [SerializeField] private UIButton _restartButton;

        private void Awake()
        {
            if (!LevelEditor.IsPlayedFromEditor)
            {
                gameObject.SetActive(false);
                return;
            }
            _editorButton.AddListener(BackToEditor);
            _restartButton.AddListener(RestartLevel);
        }

        private void RestartLevel()
        {
            GameController.Instance.Restart();
        }

        private void BackToEditor()
        {
            LevelEditor.CurrentLevelID = GameController.Instance.LevelData.levelID;
            TransitionManager.LoadScene(SceneName.LevelEditor);
        }
    }
}