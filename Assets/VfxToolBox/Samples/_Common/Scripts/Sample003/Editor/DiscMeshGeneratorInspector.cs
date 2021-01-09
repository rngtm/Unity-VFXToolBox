using UnityEditor;
using UnityEngine;

namespace VfxToolBox.Sample._003
{
    [CustomEditor(typeof(DiscMeshGenerator))]
    public class DiscMeshGeneratorInspector : Editor
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
            var meshGenerator = target as DiscMeshGenerator;
            if (meshGenerator == null || meshGenerator.Mesh == null) return;
            
            MeshExporter.SaveMeshDialog(meshGenerator.Mesh, "Save Disc Mesh", "New Disc Mesh.asset");
        }
    }
}