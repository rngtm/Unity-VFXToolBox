namespace VfxToolBox.Sample._003
{
    using UnityEngine;

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [ExecuteInEditMode]
    public class SpiralMeshGenerator : MonoBehaviour
    {
        [SerializeField] private int curveDivsU = 32;
        [SerializeField] private int curveDivsV = 32;
        [SerializeField] private float curveWidth = 0.25f;
        [SerializeField] private float height = 2f;
        [SerializeField] private float loops = 2f;
        [SerializeField] private float roll = 0f;

        [SerializeField] private AnimationCurve radiusCurve = new AnimationCurve(new Keyframe[]
        {
            new Keyframe(0f, 0f),
            new Keyframe(1f, 1f),
        });

        [SerializeField] private Gradient vertexColorU = new Gradient();
        [SerializeField] private Gradient vertexColorV = new Gradient();
        
        [SerializeField, HideInInspector] private Mesh mesh;
        [SerializeField, HideInInspector] private MeshFilter meshFilter;

        private bool needComputeMesh = false;

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

        /// <summary>
        /// メッシュの更新
        /// </summary>
        void ComputeMesh()
        {
            curveDivsU = Mathf.Max(2, curveDivsU);
            curveDivsV = Mathf.Max(3, curveDivsV);
            curveWidth = Mathf.Max(0f, curveWidth);
            loops = Mathf.Max(0f, loops);
            
            // compute points
            Vector3[] points;
            ComputeCurvePoints(out points);

            // compute vertex uvs, position, color
            int vertexCount = curveDivsU * curveDivsV * 2;
            Vector2[] uv = new Vector2[vertexCount];
            Vector3[] vertices = new Vector3[vertexCount];
            Color[] colors = new Color[vertexCount];
            Vector3 up = new Vector3(0f, 1f, 0f); // up vector
            float rollRadian = roll * Mathf.Deg2Rad;
            for (int vi = 0; vi < curveDivsV; vi++)
            {
                // compute vectors
                var forwardVector =
                    (vi + 1 < curveDivsV) ? (points[vi + 1] - points[vi]) : (points[vi] - points[vi - 1]);
                forwardVector = forwardVector.normalized;
                var rightVector = Vector3.Cross(up, forwardVector);
                var normalVector = Vector3.Cross(rightVector, forwardVector);

                float tv = (float) vi / (curveDivsV - 1);
                for (int ui = 0; ui < curveDivsU; ui++)
                {
                    int vertexIndex = ui + vi * curveDivsU;

                    // values in [0, 1]
                    float tu = (float) ui / (curveDivsU - 1);

                    // position
                    var rightPosition = rightVector * Mathf.Cos(rollRadian) + normalVector * Mathf.Sin(rollRadian);
                    rightPosition *= curveWidth * tu;
                    vertices[vertexIndex] = points[vi] + rightPosition;

                    // color
                    var colorU = vertexColorU.Evaluate(tu);
                    var colorV = vertexColorV.Evaluate(tv);
                    colors[vertexIndex] = colorU * colorV;

                    // uv
                    uv[vertexIndex] = new Vector2(tu, tv);
                }
            }

            // compute triangles
            int triangleCount = (curveDivsU - 1) * (curveDivsV - 1) * 6;
            int[] triangles = new int[triangleCount];
            int ti = 0;
            for (int vi = 0; vi < curveDivsV - 1; vi++)
            {
                for (int ui = 0; ui < curveDivsU - 1; ui++)
                {
                    int pi = ui + vi * curveDivsU;

                    triangles[ti + 0] = pi; 
                    triangles[ti + 1] = pi + curveDivsU; 
                    triangles[ti + 2] = pi + 1;

                    triangles[ti + 3] = pi + 1;
                    triangles[ti + 4] = pi + curveDivsU;
                    triangles[ti + 5] = pi + curveDivsU + 1;
                    ti += 6;
                }
            }

            // clear
            mesh.triangles = null;

            // set
            mesh.vertices = vertices;
            mesh.colors = colors;
            mesh.uv = uv;
            mesh.triangles = triangles;

            meshFilter.mesh = mesh;
        }

        /// <summary>
        /// カーブに沿ったポイントを作成
        /// </summary>
        private void ComputeCurvePoints(out Vector3[] points)
        {
            float curveTimeMin = radiusCurve[0].time;
            float curveTimeMax = radiusCurve[radiusCurve.length - 1].time;
            points = new Vector3[curveDivsV];
            for (int i = 0; i < curveDivsV; i++)
            {
                float t = (float) i / (curveDivsV - 1);
                float radian = t * 2f * Mathf.PI * loops;
                float radius = radiusCurve.Evaluate(Remap(t, 0f, 1f, curveTimeMin, curveTimeMax));
                float x = Mathf.Cos(radian) * radius;
                float y = height * t;
                float z = Mathf.Sin(radian) * radius;
                points[i] = new Vector3(x, y, z);
            }
        }

        private float Remap(float x, float a, float b, float c, float d)
        {
            return (x - a) / (b - a) * (d - c) + c;
        }

        private void OnDestroy()
        {
            DestroyImmediate(mesh);
        }
    }
}