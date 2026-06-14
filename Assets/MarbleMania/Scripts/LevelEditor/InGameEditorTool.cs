using System;
using Anvil;
using MarbleMania.Scripts.Game;
using UnityEngine;

namespace MarbleMania.LevelEditor
{
    public class InGameEditorTool : MonoBehaviour
    {
        [SerializeField] private UIButton _editorButton;

        private void Awake()
        {
            if (!LevelEditor.IsPlayedFromEditor)
            {
                gameObject.SetActive(false);
                return;
            }
            _editorButton.AddListener(BackToEditor);
        }

        private void BackToEditor()
        {
            LevelEditor.CurrentLevelID = GameController.Instance.LevelData.levelID;
            TransitionManager.LoadScene(SceneName.LevelEditor);
        }
    }
}