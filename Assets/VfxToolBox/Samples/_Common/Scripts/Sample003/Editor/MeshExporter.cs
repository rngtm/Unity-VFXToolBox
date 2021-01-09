using System.ComponentModel;

namespace VfxToolBox.Sample._003
{
    using UnityEditor;
    using UnityEngine;
    
    public static class MeshExporter
    {
        /// <summary>
        /// メッシュを保存するダイアログを表示
        /// </summary>
        public static void SaveMeshDialog(Mesh mesh, string dialogTitle, string defaultAssetName)
        {
            var savePath = EditorUtility.SaveFilePanelInProject(
                dialogTitle, defaultAssetName, "asset", "Save Mesh");

            if (string.IsNullOrEmpty(savePath)) return;

            // clone mesh
            mesh = Object.Instantiate(mesh);
            MeshUtility.Optimize(mesh);

            var oldAsset = AssetDatabase.LoadAssetAtPath(savePath, typeof(Object));
            if (oldAsset != null)
            {
                // copy
                EditorUtility.CopySerialized(mesh, oldAsset);
                AssetDatabase.SaveAssets();
            }
            else
            {
                // create
                AssetDatabase.CreateAsset(mesh, savePath);
                AssetDatabase.Refresh();
            }

            var asset = AssetDatabase.LoadAssetAtPath(savePath, typeof(Mesh));
            if (asset == null) return;

            EditorGUIUtility.PingObject(asset);
        }
    }
}