using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

/// <summary>
/// Helper class for instantiating ScriptableObjects.
/// </summary>
public class ScriptableObjectFactory
{
    //[MenuItem("Project/Create/ScriptableObject")]
    [MenuItem("Assets/Create/ScriptableObject")]
    public static void Create()
    {
        var assembly = GetAssembly();

        // Get all classes derived from ScriptableObject
        var allScriptableObjects = (from t in assembly.GetTypes()
                                    where t.IsSubclassOf(typeof(ScriptableObject))
                                    select t).ToArray();

        // Show the selection window.
        var window = EditorWindow.GetWindow<ScriptableObjectWindow>(true, "Create a new ScriptableObject", true);
        window.ShowPopup();

        window.Types = allScriptableObjects;

//		var window = EditorWindow.GetWindow<SelectTypeWindow>(true, "Select Type", true);
//		window.Show(typeof(ScriptableObject), "Create", (type) => {
//			var asset = ScriptableObject.CreateInstance(type);
//			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
//				asset.GetInstanceID(),
//				ScriptableObject.CreateInstance<EndNameEdit>(),
//				string.Format("{0}.asset", type.FullName),
//				AssetPreview.GetMiniThumbnail(asset), 
//				null);
//		});
    }

    /// <summary>
    /// Returns the assembly that contains the script code for this project (currently hard coded)
    /// </summary>
    private static Assembly GetAssembly()
    {
        return Assembly.Load(new AssemblyName("Assembly-CSharp"));
    }
}