namespace VfxToolBox.Sample._003
{
    using UnityEngine;

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [ExecuteInEditMode]
    public class SpiralMeshGenerator : MonoBehaviour
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
        
        [SerializeField, HideInInspector] private Mesh mesh;
        [SerializeField, HideInInspector] private MeshFilter meshFilter;

        private bool needComputeMesh = false;

        public Mesh Mesh => mesh;

        private void Start()
        {
            mesh = new Mesh();
            meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            ComputeMesh();
        }

        /// <summary>
        /// 描画フレーム時 実行処理
        /// </summary>
        void Update()
        {
            if (needComputeMesh)
            {
                ComputeMesh();
                needComputeMesh = false;
            }
        }

        /// <summary>
        /// インスペクターの値が変更されたときに呼ばれる
        /// </summary>
        public void OnValidate()
        {
            needComputeMesh = true;
        }
        
        void ComputeMesh()
        {
            if (mesh == null)
            {
                mesh = new Mesh();
            }
            
            SpiralMeshGeneratorCore.ComputeMesh(this, mesh);
            meshFilter.mesh = mesh;
        }

        private void OnDestroy()
        {
            DestroyImmediate(mesh);
        }
    }
}