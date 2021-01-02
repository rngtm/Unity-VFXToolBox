using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace VfxToolBox.Shuriken.ShaderPresetGenerator
{
    // [CreateAssetMenu]
    public class ShaderPresetDatabase : ScriptableObject
    {
        [SerializeField] private List<ShaderPresetData> presets = new List<ShaderPresetData>();
        
        

        /// <summary>
        /// Shaderを指定してプリセット取得
        /// </summary>
        public ShaderPresetData FindPreset(Shader shader)
        {
            return presets.FirstOrDefault(preset => preset.GetShader() == shader);
        }

        public static ShaderPresetData FindPresetInProject(Shader shader)
        {
            return AssetDatabase.FindAssets("t:scriptableobject", null)
                .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(ShaderPresetData)) as ShaderPresetData)
                .Where(preset => preset != null)
                // .FirstOrDefault();
                .FirstOrDefault(preset => preset.GetShader() == shader);
        }
        
        [MenuItem(VfxMenuConfig.TestRootMenuName + "Get Test")]
        public static ShaderPresetDatabase Get()
        {
            Debug.Log(VfxToolConfig.PresetDatabasePath);
            var asset = AssetDatabase.LoadAssetAtPath(VfxToolConfig.PresetDatabasePath, typeof(ShaderPresetDatabase)) as ShaderPresetDatabase;            
            Debug.Log(asset);

            return asset;
        }
    }
}