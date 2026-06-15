using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Anvil;

public class GameObjectPoolDebuger : EditorWindow
{
    private Dictionary<string, Stack<GameObject>> pools;
    private Vector2 scroll;
    private Dictionary<string, bool> foldouts = new();

    private Dictionary<string, string> prefabNameCache = new();

    [MenuItem("Debug/GameObject Pool Viewer")]
    public static void Open()
    {
        GetWindow<GameObjectPoolDebuger>("Pool Viewer");
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh"))
        {
            FetchPools();
            BuildPrefabCache();
        }
        if (GUILayout.Button("Clear"))
        {
            GameObjectPool.Clear();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Validate: Objects outside"))
        {
            ValidateObjectsOutsidePoolParent();
        }

        if (GUILayout.Button("Validate: Duplicate"))
        {
            ValidateDuplicateObjects();
        }

        if (GUILayout.Button("Validate all"))
        {
            ValidateObjectsOutsidePoolParent();
            ValidateDuplicateObjects();
            ValidateNullInPool();
        }
        GUILayout.EndHorizontal();
        if (pools == null)
        {
            GUILayout.Label("No pool data");
            return;
        }

        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var kvp in pools)
        {
            string poolId = kvp.Key;
            Stack<GameObject> stack = kvp.Value;

            if (!foldouts.ContainsKey(poolId))
                foldouts[poolId] = false;

            string prefabName = GetPrefabName(poolId);

            EditorGUILayout.BeginVertical("box");

            foldouts[poolId] = EditorGUILayout.Foldout(
                foldouts[poolId],
                $"{poolId}  ({prefabName})  [{stack.Count}]",
                true);

            if (foldouts[poolId])
            {
                foreach (var obj in stack)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.ObjectField(obj, typeof(GameObject), true);

                    if (GUILayout.Button("Ping", GUILayout.Width(50)))
                        EditorGUIUtility.PingObject(obj);

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
    }

    void FetchPools()
    {
        var field = typeof(GameObjectPool).GetField(
            "_pools",
            BindingFlags.Static | BindingFlags.NonPublic);

        pools = field.GetValue(null) as Dictionary<string, Stack<GameObject>>;
    }

    void BuildPrefabCache()
    {
        prefabNameCache.Clear();

        var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (var go in allObjects)
        {
            int id = go.GetInstanceID();
            string idStr = id.ToString();

            if (!prefabNameCache.ContainsKey(idStr))
            {
                prefabNameCache[idStr] = go.name;
            }
        }
    }

    string GetPrefabName(string poolId)
    {
        if (prefabNameCache.TryGetValue(poolId, out var name))
            return name;

        return "UnknownPrefab";
    }
    Transform GetPoolParent()
    {
        var field = typeof(GameObjectPool).GetField(
            "_poolParent",
            BindingFlags.Static | BindingFlags.NonPublic);

        return field?.GetValue(null) as Transform;
    }
    void ValidateObjectsOutsidePoolParent()
    {
        if (pools == null)
        {
            Debug.LogWarning("Pool data not loaded.");
            return;
        }

        Transform poolParent = GetPoolParent();

        if (poolParent == null)
        {
            Debug.LogWarning("Pool parent is null.");
            return;
        }

        int invalidCount = 0;

        foreach (var kvp in pools)
        {
            string poolId = kvp.Key;
            Stack<GameObject> stack = kvp.Value;

            foreach (var obj in stack)
            {
                if (obj == null)
                    continue;

                if (obj.transform.parent != poolParent)
                {
                    invalidCount++;

                    Debug.LogWarning(
                        $"[Pool Validation] Object not under pool parent\n" +
                        $"Pool: {poolId}\n" +
                        $"Object: {obj.name}",
                        obj);
                }
            }
        }

        Debug.Log($"[Pool Validation] Objects outside pool parent: {invalidCount}");
    }

    void ValidateNullInPool()
    {
        if (pools == null)
        {
            return;
        }

        foreach (var entry in pools)
        {
            string poolId = entry.Key;
            Stack<GameObject> stack = entry.Value;

            foreach (var obj in stack)
            {
                if (obj == null)
                {
                    Debug.LogError($"[Pool Validation] Null object found in pool: {poolId}");
                }
            }
        }
    }
    void ValidateDuplicateObjects()
    {
        if (pools == null)
        {
            Debug.LogWarning("Pool data not loaded.");
            return;
        }

        HashSet<GameObject> seen = new();
        int duplicates = 0;

        foreach (var kvp in pools)
        {
            string poolId = kvp.Key;
            Stack<GameObject> stack = kvp.Value;

            foreach (var obj in stack)
            {
                if (obj == null)
                    continue;

                if (!seen.Add(obj))
                {
                    duplicates++;

                    Debug.LogError(
                        $"[Pool Validation] Duplicate object detected in pool\n" +
                        $"Pool: {poolId}\n" +
                        $"Object: {obj.name}",
                        obj);
                }
            }
        }

        Debug.Log($"[Pool Validation] Duplicate pooled objects: {duplicates}");
    }
}