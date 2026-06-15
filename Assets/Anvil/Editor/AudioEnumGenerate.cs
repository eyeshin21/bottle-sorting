using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class AudioClipEnumGenerator: EditorWindow
{
    private DefaultAsset audioFolder;
    private DefaultAsset outputFolder;

    private const string EnumName = "AudioClipName";
    private const string FileName = "AudioClipName.cs";
    private string namespaceName = "Anvil";

    [MenuItem("Tools/Audio/Generate AudioClip Enum")]
    private static void Open()
    {
        GetWindow<AudioClipEnumGenerator>("Audio Enum Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("AudioClip Enum Generator", EditorStyles.boldLabel);

        audioFolder = (DefaultAsset)EditorGUILayout.ObjectField(
            "Audio Folder",
            audioFolder,
            typeof(DefaultAsset),
            false
        );

        outputFolder = (DefaultAsset)EditorGUILayout.ObjectField(
            "Output Folder",
            outputFolder,
            typeof(DefaultAsset),
            false
        );

        namespaceName = EditorGUILayout.TextField("Namespace", namespaceName);

        GUILayout.Space(8);
        EditorGUILayout.LabelField("Enum Name", EnumName);
        EditorGUILayout.LabelField("File Name", FileName);

        GUILayout.Space(12);

        GUI.enabled = audioFolder != null && outputFolder != null;
        if (GUILayout.Button("Generate Enum"))
        {
            Generate();
        }
        GUI.enabled = true;
    }

    private void Generate()
    {
        var audioFolderPath = AssetDatabase.GetAssetPath(audioFolder);
        var outputFolderPath = AssetDatabase.GetAssetPath(outputFolder);

        if (!AssetDatabase.IsValidFolder(outputFolderPath))
        {
            Debug.LogError("Output folder is not a valid folder.");
            return;
        }

        var guids = AssetDatabase.FindAssets("t:AudioClip", new[] { audioFolderPath });

        if (guids.Length == 0)
        {
            Debug.LogWarning("No AudioClips found in the selected folder.");
            return;
        }

        var sb = new StringBuilder();

        sb.AppendLine($"namespace {namespaceName}");
        sb.AppendLine("{");
        sb.AppendLine($"    public enum {EnumName}");
        sb.AppendLine("    {");

        sb.AppendLine($"        None,");
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var clipName = Path.GetFileNameWithoutExtension(path);
            sb.AppendLine($"        {SanitizeEnumName(clipName)},");
        }

        sb.AppendLine("    }");
        sb.AppendLine("}");

        var outputPath = Path.Combine(outputFolderPath, FileName);
        File.WriteAllText(outputPath, sb.ToString());

        AssetDatabase.Refresh();

        Debug.Log($"AudioClip enum generated: {outputPath}");
    }

    private static string SanitizeEnumName(string name)
    {
        name = Regex.Replace(name, @"[^a-zA-Z0-9_]", "_");

        if (char.IsDigit(name[0]))
            name = "_" + name;

        return name;
    }
}
