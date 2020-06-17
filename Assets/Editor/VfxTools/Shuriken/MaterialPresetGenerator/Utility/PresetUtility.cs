using UnityEngine;
using UnityEditor;

namespace VfxTools.Shuriken.ShaderPresetGenerator
{
    public static class PresetUtility
    {
        
        /// <summary>
        /// マテリアルプリセットの適用
        /// </summary>
        public static void ApplyPreset(ParticleSystem ps, ShaderPresetData preset)
        {
            var psr = ps.gameObject.GetComponent<ParticleSystemRenderer>();
            if (psr == null) return;

            bool needRestart = ps.isPlaying;
            ps.Stop();

            Undo.RegisterFullObjectHierarchyUndo(ps.gameObject, "Apply Effect Material Preset");
            ApplyPresetCustomData(ps.gameObject, preset);
            ApplyPresetVertexStream(ps.gameObject, preset);

            if (needRestart)
            {
                ps.Play();
            }
        }
        
        /// <summary>
        /// マテリアルプリセットの適用
        /// </summary>
        public static void ApplyPreset(GameObject gameObject, ShaderPresetData preset, Material material)
        {
            if (gameObject == null) return;
            
            var ps = gameObject.GetComponent<ParticleSystem>();
            if (ps == null) return;

            var psr = gameObject.GetComponent<ParticleSystemRenderer>();
            if (psr == null) return;
            psr.material = material;

            bool needRestart = ps.isPlaying;
            ps.Stop();

            Undo.RegisterFullObjectHierarchyUndo(gameObject, "Apply Effect Material Preset");
            ApplyPresetCustomData(ps, preset);
            ApplyPresetVertexStream(psr, preset);

            if (needRestart)
            {
                ps.Play();
            }
        }
        
        private static void ApplyPresetVertexStream(ParticleSystemRenderer psr, ShaderPresetData preset)
        {
            CustomDataUtility.SetVertexStream(psr, preset);
        }

        private static void ApplyPresetCustomData(ParticleSystem ps, ShaderPresetData preset)
        {
            CustomDataUtility.SetCustomData(ps, preset);
        }

        private static void ApplyPresetVertexStream(GameObject gameObject, ShaderPresetData preset)
        {
            var psr = gameObject.GetComponent<ParticleSystemRenderer>();
            if (psr == null) return;
            
            CustomDataUtility.SetVertexStream(psr, preset);
        }

        private static void ApplyPresetCustomData(GameObject gameObject, ShaderPresetData preset)
        {
            var ps = gameObject.GetComponent<ParticleSystem>();
            if (ps == null) return;
            
            CustomDataUtility.SetCustomData(ps, preset);
        }
    }
}