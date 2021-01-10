namespace VfxToolBox.Sample._003
{
    using UnityEngine;
    
    public abstract class MeshGeneratorBase : MonoBehaviour
    {
        [SerializeField, HideInInspector] private Mesh mesh;
        [SerializeField, HideInInspector] private MeshFilter meshFilter;
        [SerializeField, HideInInspector] private int objectInstanceId = -1;
        private bool needComputeMesh = false;

        public Mesh Mesh => mesh;
        
        protected virtual void Start()
        {
            mesh = new Mesh();
            objectInstanceId = this.GetInstanceID();
            mesh.name = this.GetInstanceID().ToString();

            meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            
            ComputeMesh(mesh);
        }
        
        /// <summary>
        /// 描画フレーム時 実行処理
        /// </summary>
        protected virtual void Update()
        {
            if (needComputeMesh)
            {
                if (objectInstanceId != GetInstanceID())
                {
                    // recreate mesh
                    mesh = new Mesh();
                    objectInstanceId = GetInstanceID();
                    mesh.name = GetInstanceID().ToString();
                    meshFilter.mesh = mesh;
                }
                
                ComputeMesh(mesh);
                needComputeMesh = false;
            }
        }
        
        /// <summary>
        /// コンポーネントが破棄されたタイミングで実行
        /// </summary>
        protected virtual void OnDestroy()
        {
            DestroyImmediate(mesh);
        }
        
        /// <summary>
        /// インスペクターの値が変更されたときに呼ばれる
        /// </summary>
        public virtual void OnValidate()
        {
            needComputeMesh = true;
        }

        protected abstract void ComputeMesh(Mesh mesh);
    }
}