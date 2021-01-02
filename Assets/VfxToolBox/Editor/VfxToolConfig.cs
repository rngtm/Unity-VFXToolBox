using UnityEditor;
using UnityEngine;

namespace VfxToolBox
{
    // [CreateAssetMenu]
    public class VfxToolConfig : ScriptableObject
    {
        private static InternalToolpath interanlToolPath = new InternalToolpath();
        private static UnityProjectToolpath unityToolPath = new UnityProjectToolpath(interanlToolPath);
        private static PackageToolPath packageToolPath = new PackageToolPath(interanlToolPath);
        
        public static string VfxToolConfigPath => toolPath.VfxToolConfigPath; // ツールコンフィグ
        public static string DefaultPresetExportDirectory => toolPath.DefaultPresetExportDirectory; // プリセット出力先
        public static string PresetDatabasePath => toolPath.PresetDatabasePath; // プリセットデータ
        private static IToolPath toolPath => PackageConfig.IsInstallPackage == true ? (IToolPath)packageToolPath : unityToolPath;

        [SerializeField] private Texture2D frameTexture = null;

        public static VfxToolConfig Get()
        {
            return AssetDatabase.LoadAssetAtPath(toolPath.VfxToolConfigPath, typeof(VfxToolConfig)) as
                VfxToolConfig;
        }

        public Texture2D GetFrameTexture()
        {
            return frameTexture;
        }

        private interface IToolPath
        {
            string VfxToolConfigPath { get; } // ツールコンフィグ
            string DefaultPresetExportDirectory { get; } // プリセット出力先
            string PresetDatabasePath { get; }
        }
        
        
        private class InternalToolpath : IToolPath
        {
            public string VfxToolConfigPath => "VfxToolBox/Editor/Data/VfxToolConfig.asset";
            public string DefaultPresetExportDirectory => "VfxToolBox/Editor/Data/ShaderPresets";
            public string PresetDatabasePath => "VfxToolBox/Editor/Data/ShaderPresetDatabase.asset";
        }

        private class UnityProjectToolpath : IToolPath
        {
            private InternalToolpath toolPath;
            public string VfxToolConfigPath => "Assets/" + toolPath.VfxToolConfigPath;
            public string DefaultPresetExportDirectory => "Assets/" + toolPath.DefaultPresetExportDirectory;
            public string PresetDatabasePath => "Assets/" + toolPath.PresetDatabasePath;

            public UnityProjectToolpath(InternalToolpath toolpath)
            {
                this.toolPath = toolpath;
            }
        }

        private class PackageToolPath : IToolPath
        {
            private InternalToolpath toolPath;
            public string VfxToolConfigPath => PackageConfig.GetPackagePath(toolPath.VfxToolConfigPath);
            public string DefaultPresetExportDirectory => PackageConfig.GetPackagePath(toolPath.DefaultPresetExportDirectory);
            public string PresetDatabasePath => PackageConfig.GetPackagePath(toolPath.PresetDatabasePath);
            
            public PackageToolPath(InternalToolpath toolPath)
            {
                this.toolPath = toolPath;
            }
        }
    }
}