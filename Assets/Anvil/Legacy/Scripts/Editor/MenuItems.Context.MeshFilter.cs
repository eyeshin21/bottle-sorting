#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Anvil.Legacy
{
    public static partial class MenuItems
    {
        [MenuItem("CONTEXT/MeshFilter/Save Mesh...")]
        static void MeshFilterSaveMesh(MenuCommand menuCommand)
        {
            var meshFilter = menuCommand.To<MeshFilter>();
            var mesh = meshFilter.sharedMesh;
            SaveMesh(mesh, mesh.name, false, true);
        }

        [MenuItem("CONTEXT/MeshFilter/Save Mesh As New Instance...")]
        static void MeshFilterSaveMeshNewInstance(MenuCommand menuCommand)
        {
            var meshFilter = menuCommand.To<MeshFilter>();
            var mesh = meshFilter.sharedMesh;
            SaveMesh(mesh, mesh.name, true, true);
        }

        static void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
        {
            //string path = EditorUtility.SaveFilePanel("Save Mesh Asset", "Assets/", name, "asset");
            //if (path.IsNullOrEmpty()) return;
            //path = FileUtil.GetProjectRelativePath(path);

            string path = EditorUtility.SaveFilePanelInProject("Save Mesh Asset", name, "asset", "");
            if (path.IsNullOrEmpty()) return;

            var meshToSave = makeNewInstance ? Object.Instantiate(mesh) as Mesh : mesh;
            if (optimizeMesh)
            {
                MeshUtility.Optimize(meshToSave);
            }

            AssetDatabase.CreateAsset(meshToSave, path);
            AssetDatabase.SaveAssets();
        }
    }
}
#endif