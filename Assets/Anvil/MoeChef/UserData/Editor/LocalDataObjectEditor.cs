#if UNITY_EDITOR
using System;
using System.Collections;
using System.Reflection;
using Anvil;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocalDataObject))]
public class LocalDataObjectEditor : Editor
{
    string newKey;
    string newValue;

    public override void OnInspectorGUI()
    {

        
        DrawDefaultInspector();
        EditorGUILayout.Space(10);

        var obj = (LocalDataObject)target;
        var serializer = obj.Serializer;
        if (GUILayout.Button("UnLoad"))
        {
            obj.Unload();
        }
        if (GUILayout.Button("Refresh"))
        {
            obj.Init();
        }
        if (serializer == null)
        {
            EditorGUILayout.HelpBox("Serializer not initialized.", MessageType.Warning);

            return;
        }

        var dataField = serializer.GetType()
            .GetField("_data", BindingFlags.NonPublic | BindingFlags.Instance);

        var dict = dataField?.GetValue(serializer) as IDictionary;

        if (dict == null)
        {

            EditorGUILayout.HelpBox("Data dictionary is null.", MessageType.Info);
            return;
        }

        EditorGUILayout.LabelField("Local Data", EditorStyles.boldLabel);

        var removeList = new System.Collections.Generic.List<object>();

        foreach (DictionaryEntry entry in dict)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(entry.Key.ToString(), GUILayout.Width(160));

            var wrapper = entry.Value;
            var valueProp = wrapper.GetType().GetProperty("Value");

            string value = valueProp.GetValue(wrapper)?.ToString();
            string newVal = EditorGUILayout.TextField(value);

            if (newVal != value)
                valueProp.SetValue(wrapper, newVal);

            if (GUILayout.Button("X", GUILayout.Width(22)))
                removeList.Add(entry.Key);

            EditorGUILayout.EndHorizontal();
        }

        foreach (var key in removeList)
            dict.Remove(key);

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("➕ Add Entry", EditorStyles.boldLabel);

        newKey = EditorGUILayout.TextField("Key", newKey);
        newValue = EditorGUILayout.TextField("Value", newValue);

        if (GUILayout.Button("Add"))
        {
            if (!string.IsNullOrEmpty(newKey) && !dict.Contains(newKey))
            {
                var wrapper = Activator.CreateInstance(
                    typeof(Anvil.DataWrapper), newValue);

                dict.Add(newKey, wrapper);
                newKey = "";
                newValue = "";
            }
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("💾 Save (UpSync)"))
            serializer.UpSync();
    }
}
#endif
