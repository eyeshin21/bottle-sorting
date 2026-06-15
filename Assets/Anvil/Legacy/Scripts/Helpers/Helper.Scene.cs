using UnityEngine;
using UnityEngine.SceneManagement;
using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Anvil
{
	public static partial class Helper
	{
		public static void ReloadScene()
		{
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorSceneManager.OpenScene(EditorSceneManager.GetActiveScene().path);
                return;
            }
#endif
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

    }
}