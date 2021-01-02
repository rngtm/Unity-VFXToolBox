using System;
using UnityEditor;
using UnityEngine;
using VfxToolBox;
using VfxToolBox.ShurikenCopyPaste;

namespace VfxToolBox.Shuriken.ShurikenRandomizer
{
    public class ShurikenRandomizeWindow : VfxWindowBase
    {
        [MenuItem(VfxMenuConfig.RandomizerMenuName, false, VfxMenuConfig.RandomizerMenuPriority)]
        public static void Open()
        {
            var window = GetWindow<ShurikenRandomizeWindow>();
            window.titleContent = VfxMenuConfig.RandomizerTitleContent;
        }

        protected override void OnGUI()
        {
            base.OnGUI();
            
            DrawSelectionBox();
        }

        /// <summary>
        /// 選択オブジェクトの表示ボックス
        /// </summary>
        private void DrawSelectionBox()
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("■ Selection");
                foreach (var gameObject in Selection.gameObjects)
                {
                    EditorGUILayout.ObjectField(gameObject, typeof(GameObject), true);
                }
            }
        }
        
    }
}