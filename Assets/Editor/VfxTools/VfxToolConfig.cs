using UnityEditor;
using UnityEngine;

namespace VfxTools
{
    // [CreateAssetMenu]
    public class VfxToolConfig : ScriptableObject
    {
        public const string VfxToolConfigPath 
            = "Assets/Editor/VfxTools/Data/VfxToolConfig.asset";
        public const string DefaultPresetExportDirectory 
            = "Assets/Editor/VfxTools/Data/ShaderPresets";
        public const string PresetDatabasePath 
            = "Assets/Editor/VfxTools/Data/ShaderPresetDatabase.asset"; // プリセットの一覧保存先
        
        [SerializeField] private Texture2D frameTexture = null;

        public static VfxToolConfig Get()
        {
            return AssetDatabase.LoadAssetAtPath( VfxToolConfig.VfxToolConfigPath, typeof(VfxToolConfig)) as VfxToolConfig;            
        }

        public Texture2D GetFrameTexture()
        {
            return frameTexture;
        }
    }
}