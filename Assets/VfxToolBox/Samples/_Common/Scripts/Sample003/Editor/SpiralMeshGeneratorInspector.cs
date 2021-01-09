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
            
            MeshExporter.SaveMeshDialog(meshGenerator.Mesh, "Save Spiral Mesh", "New Spiral Mesh.asset");
        }
    }
}