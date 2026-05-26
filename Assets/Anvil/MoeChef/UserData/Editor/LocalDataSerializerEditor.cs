#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Anvil.LocalDataSerializer), true)]
public class LocalDataSerializerEditor : Editor
{
    private object _dataSerializer;
    private FieldInfo _dataField;
    private MethodInfo _upSyncMethod;

    private string _newKey = "";
    private string _newValue = "";

    private void OnEnable()
    {
        _dataSerializer = target;

        var type = target.GetType();

        _dataField = type.GetField("_data", BindingFlags.NonPublic | BindingFlags.Instance);
        _upSyncMethod = type.GetMethod("UpSync", BindingFlags.Public | BindingFlags.Instance);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space(12);

        if (_dataField == null)
        {
            EditorGUILayout.HelpBox("_data field not found via reflection.", MessageType.Error);
            return;
        }

        var dict = _dataField.GetValue(_dataSerializer) as IDictionary;

        if (dict == null)
        {
            EditorGUILayout.HelpBox("Data not initialized. It will appear after Init() runs.", MessageType.Info);

            if (GUILayout.Button("Force Init via GetInt"))
            {
                // Force lazy init
                var method = target.GetType().GetMethod("GetInt");
                method?.Invoke(target, new object[] { "__init__", 0 });
            }

            return;
        }

        EditorGUILayout.LabelField("📦 Local Data Dictionary", EditorStyles.boldLabel);
        EditorGUILayout.Space(4);

        List<object> keysToRemove = new List<object>();

        foreach (DictionaryEntry entry in dict)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(entry.Key.ToString(), GUILayout.Width(180));

            object wrapper = entry.Value;
            var valueProp = wrapper.GetType().GetProperty("Value");

            string value = valueProp?.GetValue(wrapper)?.ToString();
            string newValue = EditorGUILayout.TextField(value);

            if (newValue != value)
            {
                valueProp?.SetValue(wrapper, newValue);
            }

            if (GUILayout.Button("X", GUILayout.Width(22)))
            {
                keysToRemove.Add(entry.Key);
            }

            EditorGUILayout.EndHorizontal();
        }

        foreach (var key in keysToRemove)
        {
            dict.Remove(key);
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("➕ Add New Entry", EditorStyles.boldLabel);

        _newKey = EditorGUILayout.TextField("Key", _newKey);
        _newValue = EditorGUILayout.TextField("Value", _newValue);

        if (GUILayout.Button("Add"))
        {
            if (!string.IsNullOrEmpty(_newKey))
            {
                if (!dict.Contains(_newKey))
                {
                    var wrapperType = typeof(Anvil.DataWrapper);
                    var wrapper = Activator.CreateInstance(wrapperType, _newValue);
                    dict.Add(_newKey, wrapper);

                    _newKey = "";
                    _newValue = "";
                }
                else
                {
                    Debug.LogWarning("Key already exists: " + _newKey);
                }
            }
        }

        EditorGUILayout.Space(12);

        if (GUILayout.Button("💾 Save (UpSync)"))
        {
            _upSyncMethod?.Invoke(target, null);
            Debug.Log("LocalDataSerializer saved.");
        }
    }
}
#endif
