using System;
using UnityEditor;
using UnityEngine;

namespace VfxToolBox.Shuriken.TimeChanger
{
    public class TimeChangeToolConfig
    {
        // Float値のラベルのオプション
        public readonly GUILayoutOption[] FloatLabelOptions = new GUILayoutOption[]
        {
            GUILayout.Width(125f),
        };
        public readonly GUILayoutOption[] BeforeAfterNameOptions = new GUILayoutOption[]
        {
            GUILayout.Width(165f),
        };
        public readonly float BeforeAfterNameRightSpace = 10f;

        public readonly GUILayoutOption[] BeforeAfterValueOptions = new GUILayoutOption[]
        {
            GUILayout.Width(50f),
        };
        public readonly GUILayoutOption[] BeforeAfterArrowOptions = new GUILayoutOption[]
        {
            GUILayout.Width(48f),
        };
        
        // FloatField
        public readonly GUILayoutOption[] FloatFieldOptions = new GUILayoutOption[]
        {
            GUILayout.Width(150f),
        };
        // ObjectField
        public readonly GUILayoutOption[] ObjectNameOptions = new GUILayoutOption[]
        {
            // GUILayout.Width(170f),
        };
        public readonly float ObjectNameRightSpace = 20f;
        
        public readonly GUILayoutOption[] ObjectNameLabelOptions = new GUILayoutOption[]
        {
            GUILayout.Width(270f),
        };
        // StartDelay操作ボックス
        public readonly GUILayoutOption[] StartDelayBoxOptions = new GUILayoutOption[]
        {
            // GUILayout.Width(128f),
            GUILayout.Height(68f),
            // GUILayout.ExpandHeight(true),
        };        
        // StartDelay操作ボックス
        public readonly GUILayoutOption[] EditTargetBoxOptions = new GUILayoutOption[]
        {
            // GUILayout.Width(128f),
            // GUILayout.ExpandHeight(true),
        };
        // 中段のボックス
        public readonly GUILayoutOption[] MiddleBoxOptions = new GUILayoutOption[]
        {
            // GUILayout.Height(180f),
        };
        // Before/Afterのボックス
        public readonly GUILayoutOption[] BeforeAfterBoxOptions = new GUILayoutOption[]
        {
            // GUILayout.ExpandHeight(true),
        };
        
        // ボタンの定義(Lifetime)
        public readonly ButtonData[][] LifetimeButtonDataTableArray = new ButtonData[][]
        {
            new ButtonData[] 
            {
                new AddButtonData(-0.005f),
                new AddButtonData(0.005f),
            },
            new ButtonData[] 
            {
                new AddButtonData(-0.015f),
                new AddButtonData(0.015f),
            },
            new ButtonData[] 
            {
                new AddButtonData(-0.1f),
                new AddButtonData(0.1f),
            },
            new ButtonData[] 
            {
                new AddButtonData(-0.25f),
                new AddButtonData(0.25f),
            },
            new ButtonData[] 
            {
                new AddButtonData(-1f),
                new AddButtonData(1f),
            },   
            new ButtonData[] 
            {
                new MultiplyButtonData(0.5f),
                new MultiplyButtonData(2.0f),
            },  
            new ButtonData[] 
            {
                // new DelayButtonData("= 0.0",  (delay) => 0.0f), 
                new ButtonData("Remove 0.000xxx",  (delay) =>
                {
                    delay = Mathf.Round(delay * 10000f) / 10000f;
                    return delay;
                }), 
            },
        };
        
        // ボタンの定義(StartDelay)
        public readonly ButtonData[][] DelayButtonDataTableArray = new ButtonData[][]
        {
            new ButtonData[] 
            {
                new AddButtonData(-0.005f),
                new AddButtonData(0.005f),
            },
            new ButtonData[] 
            {
                new AddButtonData(-0.015f),
                new AddButtonData(0.015f),
            },
            new ButtonData[] 
            {
                new AddButtonData(-0.1f),
                new AddButtonData(0.1f),
            },
            new ButtonData[] 
            {
                new AddButtonData(-0.25f),
                new AddButtonData(0.25f),
            },
            new ButtonData[] 
            {
                new AddButtonData(-1f),
                new AddButtonData(1f),
            },   
            new ButtonData[] 
            {
                new MultiplyButtonData(0.5f),
                new MultiplyButtonData(2.0f),
            },  
            new ButtonData[] 
            {
                new ButtonData("= 0.0",  (delay) => 0.0f), 
                new ButtonData("Remove 0.000xxx",  (delay) =>
                {
                    delay = Mathf.Round(delay * 10000f) / 10000f;
                    return delay;
                }), 
            },
        };
        
        //
        // public DelayButtonData[] PositiveAddButtonTable =
        // {
        //     new DelayAddButtonData(0.01f),
        //     new DelayAddButtonData(0.05f),
        //     new DelayAddButtonData(0.1f),
        //     new DelayAddButtonData(0.5f),
        // };
        //
        // public DelayButtonData[] NegativeAddButtonTable =
        // {
        //     new DelayAddButtonData(-0.01f),
        //     new DelayAddButtonData(-0.05f),
        //     new DelayAddButtonData(-0.1f),
        //     new DelayAddButtonData(-0.5f),
        // };
        //
        // public DelayButtonData[] MultiplyButtonTable =
        // {
        //     new DelayMultiplyButtonData(0.5f),
        //     new DelayMultiplyButtonData(2.0f),
        // };
        
        
    }
}