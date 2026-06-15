using System;
using System.Linq;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

internal class EndNameEdit : EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        AssetDatabase.CreateAsset(EditorUtility.EntityIdToObject(instanceId), AssetDatabase.GenerateUniqueAssetPath(pathName));
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
    private static GUIStyle GetStyleSafe(Func<GUIStyle> primary, string fallbackName, GUIStyle fallback)
    {
        try
        {
            var s = primary?.Invoke();
            if (s != null) return s;
        }
        catch { }
        var fromSkin = GUI.skin?.FindStyle(fallbackName);
        return fromSkin ?? fallback;
    }
    public void OnGUI()
    {
        var toolbarStyle = GetStyleSafe(() => EditorStyles.toolbar, "Toolbar", GUI.skin?.FindStyle("toolbar") ?? GUI.skin?.box);
        var searchFieldStyle = GetStyleSafe(() => EditorStyles.toolbarSearchField, "ToolbarSeachTextField", GUI.skin?.textField);
        var searchCancelStyle = GetStyleSafe(() => EditorStyles.toolbarButton, "ToolbarSeachCancelButton", GUI.skin?.button);
        
        //SimpleSearch bar
        GUILayout.BeginHorizontal(toolbarStyle);
        _strSearch = GUILayout.TextField(_strSearch ?? "", searchFieldStyle);
        if (GUILayout.Button("", searchCancelStyle))
        {
            // Remove focus if cleared
            _strSearch = "";
            GUI.FocusControl(null);
        }
        GUILayout.EndHorizontal();
        
        // GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
        // _strSearch = GUILayout.TextField(_strSearch, GUI.skin.FindStyle("ToolbarSeachTextField"));
        // if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
        // {
        //     // Remove focus if cleared
        //     _strSearch = "";
        //     GUI.FocusControl(null);
        // }
        // GUILayout.EndHorizontal();

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