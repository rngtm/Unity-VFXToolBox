using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace VfxTools.Shuriken.ShaderPresetGenerator
{
    public class ShaderPresetData : ScriptableObject
    {
        [SerializeField] private Shader shader = null;
        [SerializeField] private string comment = "";
        [SerializeField, HideInInspector] private CustomDataValue[] values = new CustomDataValue[0] { };

        [SerializeField, HideInInspector] private List<ParticleSystemVertexStream> vertexStreams =
            new List<ParticleSystemVertexStream>(new[]
            {
                ParticleSystemVertexStream.Position,
                ParticleSystemVertexStream.Normal,
                ParticleSystemVertexStream.Color,
                ParticleSystemVertexStream.UV,
                ParticleSystemVertexStream.Custom1X,
            });

        public CustomDataValue GetFloatValue(int index) => values[index];
        public IReadOnlyCollection<ParticleSystemVertexStream> VertexStreams => vertexStreams;
        public int Custom1Count => Mathf.Min(values.Length, 4);
        public int Custom2Count => Mathf.Max(values.Length - 4, 0);

        /// <summary>
        /// マテリアル取得
        /// </summary>
        public Shader GetShader()
        {
            return shader;
        }

        public void SetShader(Shader newShader)
        {
            shader = newShader;
        }

        // [MenuItem("Assets/Create/Shader Preset Data")]
        // public static void CreatePreset()
        // {
        //     var shader = Selection.activeObject as Shader;
        //     if (shader == null) return;
        //
        //     var preset = ScriptableObject.CreateInstance<ShaderPresetData>();
        //     preset.SetShader(shader);
        //
        //     var directory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(shader));
        //     Debug.Log(directory);
        //
        //     
        //     var assetPath = Path.Combine(directory, "new Preset.asset");
        //     ProjectWindowUtil.CreateAsset(preset, assetPath);
        // }
    }
}