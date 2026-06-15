#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectUIImagesInHierarchy
{
    [MenuItem("Tools/Select All Images")]
    private static void SelectImages()
    {
        if (Selection.gameObjects == null || Selection.gameObjects.Length == 0)
        {
            Debug.LogWarning("No GameObject selected.");
            return;
        }

        List<GameObject> foundObjects = new List<GameObject>();

        foreach (GameObject root in Selection.gameObjects)
        {
            Image[] images = root.GetComponentsInChildren<Image>(true); // include inactive

            foreach (var img in images)
            {
                foundObjects.Add(img.gameObject);
            }
        }

        if (foundObjects.Count == 0)
        {
            Debug.Log("No Image components found in selected hierarchy.");
            return;
        }

        Selection.objects = foundObjects.ToArray();

        Debug.Log($"Selected {foundObjects.Count} GameObjects with Image components.");
    }
}
#endif
