using System;
using System.Linq;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

internal class EndNameEdit : EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        AssetDatabase.CreateAsset(EditorUtility.InstanceIDToObject(instanceId), AssetDatabase.GenerateUniqueAssetPath(pathName));
    }
}

/// <summary>
/// Scriptable object window.
/// </summary>
public class ScriptableObjectWindow : EditorWindow
{
    private string _strSearch = "";
    private Vector2 _scrollPos;

    private string[] _names;
    private Type[] _types;

    public Type[] Types
    {
        get { return _types; }
        set
        {
            _types = value;
            _names = _types.Select(t => t.FullName).ToArray();
        }
    }

    public void OnGUI()
    {
        //Search bar
        GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
        _strSearch = GUILayout.TextField(_strSearch, GUI.skin.FindStyle("ToolbarSearchTextField"));
        if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSearchCancelButton")))
        {
            // Remove focus if cleared
            _strSearch = "";
            GUI.FocusControl(null);
        }
        GUILayout.EndHorizontal();

        if (_types == null)
            return;

        _scrollPos = GUILayout.BeginScrollView(_scrollPos, false, true);

        for (int i = 0; i < _types.Length; i++)
        {
            if (_strSearch != "" && _types[i].Name.IndexOf(_strSearch, StringComparison.OrdinalIgnoreCase) < 0)
                continue;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(_types[i].Name))
            {
                var asset = ScriptableObject.CreateInstance(_types[i]);
                ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                    asset.GetInstanceID(),
                    ScriptableObject.CreateInstance<EndNameEdit>(),
                    string.Format("{0}.asset", _names[i]),
                    AssetPreview.GetMiniThumbnail(asset),
                    null);

                Close();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
    }
}