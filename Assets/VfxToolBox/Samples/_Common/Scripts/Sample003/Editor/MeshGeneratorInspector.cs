namespace VfxToolBox.Sample._003
{
    using UnityEngine;
    using UnityEditor;
    
    [CustomEditor(typeof(MeshGeneratorBase), true)]
    public class MeshGeneratorInspector : Editor
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
            var meshGenerator = target as MeshGeneratorBase;
            if (meshGenerator == null || meshGenerator.Mesh == null) return;
            
            MeshExporter.SaveMeshDialog(meshGenerator.Mesh, "Save Mesh", $"{target.name}.asset");
        }
    }
}