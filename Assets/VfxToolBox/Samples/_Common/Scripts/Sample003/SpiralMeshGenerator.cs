namespace VfxToolBox.Sample._003
{
    using UnityEngine;

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [ExecuteInEditMode]
    public class SpiralMeshGenerator : MeshGeneratorBase
    {
        [SerializeField] public int curveDivsU = 32;
        [SerializeField] public int curveDivsV = 32;
        [SerializeField] public float curveWidth = 0.25f;
        [SerializeField] public float height = 2f;
        [SerializeField] public float loops = 2f;
        [SerializeField] public float roll = 0f;

        [SerializeField] public AnimationCurve radiusCurve = new AnimationCurve(new Keyframe[]
        {
            new Keyframe(0f, 0f),
            new Keyframe(1f, 1f),
        });

        [SerializeField] public Gradient vertexColorU = new Gradient();
        [SerializeField] public Gradient vertexColorV = new Gradient();
        
        protected override void ComputeMesh(Mesh mesh)
        {
            SpiralMeshGeneratorCore.ComputeMesh(this, mesh);
        }
    }
}