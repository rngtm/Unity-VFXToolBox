using UnityEditor;
using UnityEngine;

namespace VfxToolBox.Sample._003
{
    [CustomEditor(typeof(CylinderMeshGenerator))]
    public class CylinderMeshGeneratorInspector : Editor
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
            var meshGenerator = target as CylinderMeshGenerator;
            if (meshGenerator == null || meshGenerator.Mesh == null) return;
            
            MeshExporter.SaveMeshDialog(meshGenerator.Mesh, "Save Cylinder Mesh", "New Cylinder Mesh.asset");
        }
    }
}