namespace VfxToolBox.Sample._003
{
    using UnityEngine;
    
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [ExecuteInEditMode]
    public class CircleMeshGenerator : MonoBehaviour
    {
        [SerializeField] private int divsU = 32; // 円周方向の分割数 
        [SerializeField] private float innerRadius = 0f;
        [SerializeField] private float outerRadius = 1f;
        
        [SerializeField] public Gradient vertexColorU = new Gradient();
        [SerializeField, HideInInspector] public Gradient vertexColorV = new Gradient();
        [SerializeField, HideInInspector] private Mesh mesh;
        [SerializeField, HideInInspector] private MeshFilter meshFilter;
        private bool needComputeMesh = false;
        
        private void Start()
        {
            mesh = new Mesh();

            meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            
            ComputeMesh(mesh);
        }
        
        /// <summary>
        /// 描画フレーム時 実行処理
        /// </summary>
        void Update()
        {
            if (needComputeMesh)
            {
                divsU = Mathf.Max(3, divsU);
                
                ComputeMesh(mesh);
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
        
        /// <summary>
        /// メッシュの作成
        /// </summary>
        /// <param name="mesh"></param>
        private void ComputeMesh(Mesh mesh)
        {
            // compute points
            Vector3[] innerPoints;
            Vector3[] outerPoints;
            ComputeCirclePoints(divsU, innerRadius, out innerPoints);
            ComputeCirclePoints(divsU, outerRadius, out outerPoints);
            
            // compute vertices, color, uv
            int vertexCount = divsU * 2;
            var vertices = new Vector3[vertexCount];
            var colors32 = new Color32[vertexCount];
            var uv = new Vector2[vertexCount];
            for (int ui = 0; ui < divsU; ui++)
            {
                float tu = (float) ui / (divsU - 1);

                // position
                vertices[ui] = innerPoints[ui];
                vertices[ui + divsU] = outerPoints[ui];

                // color
                Color32 colorU = vertexColorU.Evaluate(tu);
                colors32[ui] = colorU;
                colors32[ui + divsU] = colorU;
                
                // uv
                uv[ui] = new Vector2(tu, 0f); // inner circle
                uv[ui + divsU] = new Vector2(tu, 1f); // outer circle
            }
            
            // compute triangles
            int triangleCount = vertexCount * 6;
            int[] triangles = new int [triangleCount];
            int ti = 0;
            for (int ui = 0; ui < divsU - 1; ui++)
            {
                triangles[ti] = ui;
                triangles[ti + 1] = (ui + divsU + 1) ;
                triangles[ti + 2] = ui + divsU;
                
                triangles[ti + 3] = ui;
                triangles[ti + 4] = ui + 1;
                triangles[ti + 5] = (ui + divsU + 1) ;

                ti += 6;
            }

            {
                int ui = divsU - 1;
                triangles[ti] = ui;
                triangles[ti + 1] = divsU;
                triangles[ti + 2] = ui + divsU;
                
                triangles[ti + 3] = ui;
                triangles[ti + 4] = 0;
                triangles[ti + 5] = divsU;
            }

            // clear 
            mesh.triangles = null;
            
            // set
            mesh.vertices = vertices;
            mesh.colors32 = colors32;
            mesh.uv = uv;
            mesh.triangles = triangles;
        }
        
        /// <summary>
        /// 円周上のポイントを計算する
        /// </summary>
        /// <param name="points"></param>
        private void ComputeCirclePoints(int divs, float radius, out Vector3[] points)
        {
            points = new Vector3[divs];
            for (int i = 0; i < divs; i++)
            {
                float radian = i * Mathf.PI * 2f / divs;
                points[i] = new Vector3(Mathf.Cos(radian), 0f, Mathf.Sin(radian)) * radius;
            }
        }
    }
}