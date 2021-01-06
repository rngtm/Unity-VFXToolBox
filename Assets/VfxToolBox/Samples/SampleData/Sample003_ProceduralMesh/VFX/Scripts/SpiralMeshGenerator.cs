namespace VfxToolBox.Sample._003
{
    using UnityEngine;

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class SpiralMeshGenerator : MonoBehaviour
    {
        [SerializeField] private int curveDivs = 32;
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


        private Mesh mesh;
        private MeshFilter meshFilter;
        private bool needComputeMesh = false;

        private void Start()
        {
            mesh = new Mesh();
            meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            ComputeMesh();
        }

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
            curveDivs = Mathf.Max(3, curveDivs);
            curveWidth = Mathf.Max(0f, curveWidth);
            loops = Mathf.Max(0f, loops);

            // compute points
            Vector3[] points;
            ComputePoints(out points);

            // compute vertex uvs, position, color
            int vertexCount = curveDivs * 2;
            Vector2[] uv = new Vector2[vertexCount];
            Vector3[] vertices = new Vector3[vertexCount];
            Color[] colors = new Color[vertexCount];
            Vector3 up = new Vector3(0f, 1f, 0f); // up vector
            float rollRadian = roll * Mathf.Deg2Rad;
            for (int pi = 0; pi < curveDivs; pi++)
            {
                // compute vectors
                var forwardVector =
                    (pi + 1 < curveDivs) ? (points[pi + 1] - points[pi]) : (points[pi] - points[pi - 1]);
                var rightVector = Vector3.Cross(forwardVector, up).normalized;
                var normalVector = Vector3.Cross(Vector3.right, Vector3.forward).normalized;

                // position
                var rightPosition = (rightVector * Mathf.Cos(rollRadian) + normalVector * Mathf.Sin(rollRadian)) *
                    curveWidth / 2f;
                vertices[2 * pi] = points[pi] + rightPosition;
                vertices[2 * pi + 1] = points[pi] - rightPosition;

                // color
                float t = (float) pi / (curveDivs - 1);

                var colorU = vertexColorU.Evaluate(t);
                var colorV = vertexColorV.Evaluate(t);
                colors[2 * pi] = colorU * colorV;

                // uv
                uv[2 * pi] = new Vector2(t, 0f);
                uv[2 * pi + 1] = new Vector2(t, 1f);
            }

            // compute triangles
            int triangleCount = curveDivs * 6;
            int[] triangles = new int[triangleCount];
            int ti = 0;
            for (int pi = 0; pi < curveDivs - 1; pi++)
            {
                triangles[ti++] = pi;
                triangles[ti++] = pi + 2;
                triangles[ti++] = pi + 1;

                triangles[ti++] = pi + 1;
                triangles[ti++] = pi + 2;
                triangles[ti++] = pi + 3;
            }

            // clear
            mesh.triangles = null;

            // set
            mesh.vertices = vertices;
            mesh.colors = colors;
            mesh.uv = uv;
            mesh.triangles = triangles;
        }

        private void ComputePoints(out Vector3[] points)
        {
            float curveTimeMin = radiusCurve[0].time;
            float curveTimeMax = radiusCurve[radiusCurve.length - 1].time;
            points = new Vector3[curveDivs];
            for (int i = 0; i < curveDivs; i++)
            {
                float t = (float) i / (curveDivs - 1);
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
    }
}