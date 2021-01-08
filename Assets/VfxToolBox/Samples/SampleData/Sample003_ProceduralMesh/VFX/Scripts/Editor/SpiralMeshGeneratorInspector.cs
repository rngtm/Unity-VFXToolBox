namespace VfxToolBox.Sample._003
{
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(SpiralMeshGenerator))]
    public class SpiralMeshGeneratorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Save Mesh"))
            {
                SaveMesh();
            }
        }

        private void SaveMesh()
        {
            var meshGenerator = target as SpiralMeshGenerator;
            if (meshGenerator == null || meshGenerator.Mesh == null) return;

            var savePath = EditorUtility.SaveFilePanelInProject(
                "Save Spiral Mesh", "New Spiral Mesh.asset", "asset", "Save Mesh");

            if (string.IsNullOrEmpty(savePath)) return;

            // clone mesh
            var mesh = Instantiate(meshGenerator.Mesh);
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