﻿using UnityEditor;
using UnityEngine;

namespace VfxTools.ShurikenCopyPaste
{
    public static class Config
    {
        public static readonly Vector2 WindowMinimumSize = new Vector2(120f,  120f);
        public static readonly float IconSpace = -5f;
        public static int RepaintInterval = 4;
        
        // GUIStyle
        public static GUIStyle BoxCopyPasteStyle => GUI.skin.box;
        public static GUIStyle ButtonStyle => EditorStyles.miniButton;

        // GUILayoutOption
        public static readonly GUILayoutOption[] BoxTitleLabelOption = new GUILayoutOption[]
        {
        };

        public static readonly GUILayoutOption[] LabelCopyPasteOption = new GUILayoutOption[] // モジュールのラベル
        {
            // GUILayout.ExpandWidth(true),
            GUILayout.Width(174f),
        };
        
        public static readonly GUILayoutOption[] LabelClipboardOption = new GUILayoutOption[] // クリップボードのラベル
        {
            // GUILayout.ExpandWidth(true),
            // GUILayout.Width(204f),
            GUILayout.Width(210f),
        };        
        public static readonly GUILayoutOption[] TextCopyNameOption = new GUILayoutOption[] // コピー名のテキスト
        {
            // GUILayout.ExpandWidth(true),
            GUILayout.MinWidth(48f),
        };
        
        public static readonly GUILayoutOption[] ButtonOption = new GUILayoutOption[] // ボタン
        {
            GUILayout.MinWidth(24),
            GUILayout.Width(48),
        };
        public static readonly GUILayoutOption[] BoxCopyPasteOption = new GUILayoutOption[] // コピペフィールドボックス
        {
            GUILayout.ExpandWidth(true),
            GUILayout.ExpandHeight(true),
            GUILayout.MinWidth(340f),
        };
        public static readonly GUILayoutOption[] BoxClipboardOption = new GUILayoutOption[] // クリップボード全体
        {
            GUILayout.ExpandWidth(true),
            GUILayout.ExpandHeight(true),
        };
        public static readonly GUILayoutOption[] BoxClipboardModuleOption = new GUILayoutOption[] // モジュール別　クリップボード
        {
            GUILayout.ExpandWidth(true),
        };
        public static readonly GUILayoutOption[] IconOption = new GUILayoutOption[]
        {
            GUILayout.Width(16f),
        };
        
        // コピー対象のモジュール
        public static  readonly ModuleType[] TargetModules = new ModuleType[]
        {
            ModuleType.Main,
            ModuleType.Emission,
            ModuleType.Shape,
            ModuleType.VelocityOverLifetime,
            ModuleType.LimitVelocityOverLifetime,
            ModuleType.InheritVelocity,
            ModuleType.ForceOverLifetime,
            ModuleType.ColorOverLifetime,
            ModuleType.ColorBySpeed,
            ModuleType.SizeOverLifetime,
            ModuleType.SizeBySpeed,
            ModuleType.RotationOverLifetime,
            ModuleType.RotationBySpeed,
            ModuleType.ExternalForces,
            ModuleType.Noise,
            // ModuleType.Collision,
            ModuleType.Triggers,
            // ModuleType.SubEmitters,
            ModuleType.TextureSheetAnimation,
            ModuleType.Lights,
            ModuleType.Trails,
            ModuleType.CustomData,
            ModuleType.Renderer,
        };

    }
}