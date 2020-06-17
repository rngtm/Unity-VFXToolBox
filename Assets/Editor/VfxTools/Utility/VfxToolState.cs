using UnityEditor;
using UnityEngine;
using VfxTools.Shuriken.ShaderPresetGenerator;

namespace VfxTools
{
    public static class VfxToolState
    {
        public static GameObject LastSelectionGameObject { get; private set; }
        public static ShaderPresetData LastSelectionPreset { get; private set; }
        public static string SelectionGameObjectName => LastSelectionGameObject ? LastSelectionGameObject.name : "";
        
        [InitializeOnLoadMethod]
        static void OnLoad()
        {
            Selection.selectionChanged += () =>
            {
                LastSelectionGameObject = Selection.activeGameObject ?? LastSelectionGameObject;

                var selectionPresetData = Selection.activeObject as ShaderPresetData;
                LastSelectionPreset = selectionPresetData ?? LastSelectionPreset;
            };
        }
    }
}