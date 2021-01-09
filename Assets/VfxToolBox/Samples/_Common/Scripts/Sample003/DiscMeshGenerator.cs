namespace VfxToolBox.Sample._003
{
    using UnityEngine;
    
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [ExecuteInEditMode]
    public class DiscMeshGenerator : MonoBehaviour
    {
        [SerializeField] private int divsU = 32; // 円周方向の分割数 
        [SerializeField] private int divsV = 4; // 半径方向の分割数 
        [SerializeField] private float innerRadius = 0f;
        [SerializeField] private float outerRadius = 1f;
        
        [SerializeField] public Gradient vertexColorU = new Gradient();
        [SerializeField] public Gradient vertexColorV = new Gradient();
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
            int vertexCount = divsU * divsV;
            var vertices = new Vector3[vertexCount];
            var colors32 = new Color32[vertexCount];
            var uv = new Vector2[vertexCount];
            for (int ui = 0; ui < divsU; ui++)
            {
                float tu = (float) ui / (divsU - 1);
                var colorU = vertexColorU.Evaluate(tu);

                for (int vi = 0; vi < divsV; vi++)
                {
                    float tv = (float) vi / (divsV - 1);
                    var colorV = vertexColorV.Evaluate(tv);
                    
                    int vertexIndex =  ui * divsV + vi;

                    // position
                    vertices[vertexIndex] = Vector3.Lerp(innerPoints[ui], outerPoints[ui], tv);

                    // color
                    colors32[vertexIndex] = colorU * colorV;
                
                    // uv
                    uv[vertexIndex] = new Vector2(tu, tv); 
                }
            }
            
            // compute triangles
            int triangleCount = (divsU) * (divsV - 1) * 6;
            int[] triangles = new int[triangleCount];
            int ti = 0;
            for (int ui = 0; ui < divsU; ui++)
            {
                for (int vi = 0; vi < divsV - 1; vi++)
                {
                    int pi = (ui) * divsV + vi;
                    
                    triangles[ti] = pi % vertexCount;
                    triangles[ti + 1] = (pi + divsV) % vertexCount;
                    triangles[ti + 2] = (pi + divsV + 1) % vertexCount; 
                
                    triangles[ti + 3] = triangles[ti];
                    triangles[ti + 4] = triangles[ti + 2];
                    triangles[ti + 5] = (pi + 1) % vertexCount;

                    ti += 6;
                }
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
                float radian = i * Mathf.PI * 2f / (divs - 1);
                points[i] = new Vector3(Mathf.Cos(radian), 0f, Mathf.Sin(radian)) * radius;
            }
        }
    }
}