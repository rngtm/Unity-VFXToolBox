using UnityEngine;

namespace VfxToolBox.Sample._003
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [ExecuteInEditMode]
    public class CylinderMeshGenerator : MonoBehaviour
    {
        [SerializeField] private int divsU = 16; // 円周方向の分割数
        [SerializeField] private int divsV = 4; // 高さ方向の分割数

        [SerializeField] public AnimationCurve radiusCurveV = new AnimationCurve(new Keyframe[]
        {
            new Keyframe(0f, 0f),
            new Keyframe(1f, 1f),
        });

        [SerializeField] private float height = 1;

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

            ComputeMesh(mesh);
        }

        void Update()
        {
            if (needComputeMesh)
            {
                divsU = Mathf.Max(3, divsU);
                divsV = Mathf.Max(2, divsV);

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
        private void ComputeMesh(Mesh mesh)
        {
            float curveTimeMin = radiusCurveV[0].time;
            float curveTimeMax = radiusCurveV[radiusCurveV.length - 1].time;

            // compute vertices, color, uv
            int vertexCount = divsU * divsV;
            var vertices = new Vector3[vertexCount];
            var colors32 = new Color32[vertexCount];
            var uv = new Vector2[vertexCount];
            var normals = new Vector3[vertexCount];
            for (int vi = 0; vi < divsV; vi++)
            {
                float tv = (float) vi / (divsV - 1);
                float radius = radiusCurveV.Evaluate(Remap(tv, 0f, 1f, curveTimeMin, curveTimeMax));
                Vector3[] circlePoints;
                ComputeCirclePoints(divsU, radius, height * tv, out circlePoints);

                var colorV = vertexColorV.Evaluate(tv);
                
                for (int ui = 0; ui < divsU; ui++)
                {
                    float tu = (float) ui / (divsU - 1);
                    var colorU = vertexColorU.Evaluate(tu);

                    int vertexIndex = ui + vi * divsU;

                    // position
                    vertices[vertexIndex] = circlePoints[ui];

                    // color
                    colors32[vertexIndex] = colorU * colorV;

                    // uv
                    uv[vertexIndex] = new Vector2(tu, tv);
                    
                    // normal
                    normals[vertexIndex] = new Vector3(circlePoints[ui].x, 0f, circlePoints[ui].z).normalized;
                }
            }

            // compute triangles
            int triangleCount = (divsU) * (divsV - 1) * 6;
            int[] triangles = new int[triangleCount];
            int ti = 0;
            for (int vi = 0; vi < divsV - 1; vi++)
            {
                for (int ui = 0; ui < divsU - 1; ui++)
                {
                    int vertexIndex = ui + vi * divsU;

                    triangles[ti] = vertexIndex;
                    triangles[ti + 1] = vertexIndex + divsU + 1;
                    triangles[ti + 2] = vertexIndex + 1;

                    triangles[ti + 3] = vertexIndex;
                    triangles[ti + 4] = vertexIndex + divsU;
                    triangles[ti + 5] = vertexIndex + divsU + 1;

                    ti += 6;
                }

                {
                    int ui = divsU - 1;
                    int vertexIndex = ui + vi * divsU;

                    triangles[ti] = vertexIndex;
                    triangles[ti + 1] = vertexIndex + 1;
                    triangles[ti + 2] = vertexIndex + 1 - divsU;

                    triangles[ti + 3] = vertexIndex;
                    triangles[ti + 4] = vertexIndex + divsU;
                    triangles[ti + 5] = vertexIndex + 1;

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

            mesh.normals = normals;

            // mesh.RecalculateNormals();
            // mesh.RecalculateTangents();
        }

        /// <summary>
        /// 円周上のポイントを計算する
        /// </summary>
        private void ComputeCirclePoints(int divs, float radius, float height, out Vector3[] points)
        {
            points = new Vector3[divs];
            for (int i = 0; i < divs; i++)
            {
                float radian = i * Mathf.PI * 2f / (divs - 1);
                points[i] = new Vector3(
                    Mathf.Cos(radian) * radius,
                    height,
                    Mathf.Sin(radian) * radius
                );
            }
        }

        private static float Remap(float x, float a, float b, float c, float d)
        {
            return (x - a) / (b - a) * (d - c) + c;
        }
    }
}