using UnityEngine;

namespace VfxToolBox
{
    public static class VfxMenuConfig
    {
        // ボタン表示ラベル名
        public const string ShurikenViewerButtonName 
            = "エフェクト表示";
        public const string TimeChangerButtonName 
            = "時間変更";
        public const string LoopButtonName
            = "選択ループ";
        public const string UnloopButtonName
            = "選択ループ解除";
        public const string MaterialPresetAttacherButtonName 
            = "プリセットアタッチ";
        public const string MaterialPresetGeneratorButtonName 
            = "プリセット作成";
        public const string CopyPasteButtonName 
            = "コピー & ペースト";
        public const string ShurikenColorChangeButtonName 
            = "カラー変更";
        public const string RandomizerButtonName 
            = "ランダム化";
        
        // メニュー名
        public const string ToolHubMenuName
            = "Window/VFX ToolBox/Open VFX Tool Hub";
        public const string ShurikenViewerMenuName 
            = "Window/VFX ToolBox/Shuriken Viewer";
        public const string TimeChangerMenuName 
            = "Window/VFX ToolBox/Shuriken Time Changer";
        public const string MaterialPresetAttacherMenuName 
            = "Window/VFX ToolBox/Material Preset Attacher";
        public const string MaterialPresetGeneratorMenuName 
            = "Window/VFX ToolBox/Material Preset Generator";
        public const string CopyPasteMenuName 
            = "Window/VFX ToolBox/Shuriken Copy Paste";
        public const string ShurikenColorChangeMenuName 
            = "Window/VFX ToolBox/Shuriken Color Changer";
        public const string RandomizerMenuName 
            = "Window/VFX ToolBox/Shuriken Randomizer";
        
        // ウィンドウタイトル
        public static readonly GUIContent ToolHubTitleContent
            = new GUIContent("VFXToolHub");
        public static readonly GUIContent ShurikenViewerTitleContent 
            = new GUIContent("Shuriken Viewer");
        public static readonly GUIContent  TimeChangerTitleContent 
            = new GUIContent("Shuriken TimeChanger");
        public static readonly GUIContent MaterialPresetAttacherTitleContent 
            = new GUIContent("Material Preset Attacher");
        public static readonly GUIContent MaterialPresetGeneratorTitleContent 
            = new GUIContent("Material Preset Generator");
        public static readonly GUIContent ShurikenCopyPasteTitleContent 
            = new GUIContent("Shuriken Copy Paste");
        public static readonly GUIContent ShurikenColorChangeTitleContent 
            = new GUIContent("Shuriken ColorChanger");
        public static readonly GUIContent RandomizerTitleContent 
            = new GUIContent("Shuriken Randomizer");

        // メニュー優先度
        public const int ToolHubMenuPriority 
            = 1;
        public const int ShurikenViewerMenuPriority 
            = 201;
        public const int TimeChangerMenuPriority 
            = 202;
        public const int MaterialPresetAttacherMenuPriority 
            = 52;
        public const int MaterialPresetGeneratorMenuPriority 
            = 51;
        public const int CopyPasteMenuPriority 
            = 101;
        public const int RandomizerMenuPriority 
            = 53;

    }
}