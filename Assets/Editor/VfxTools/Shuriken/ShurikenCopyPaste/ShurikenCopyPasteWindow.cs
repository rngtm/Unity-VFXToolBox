namespace VfxTools.ShurikenCopyPaste
{
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEditor;

    /** ********************************************************************************
    * @summary ParticleSystemのコピーペーストウィンドウ
    ***********************************************************************************/
    public class ShurikenCopyPasteWindow : VfxWindowBase
    {
        [SerializeField] private ParticleSystemCopyPaste copyPaste = new ParticleSystemCopyPaste();
        private readonly DelayCallSystem delayCall = new DelayCallSystem();
        private Vector2 buttonScrollPos = Vector2.zero;
        private Vector2 clipboardScrollPos = Vector2.zero;
        private ToolIcon icon = null;

        [MenuItem(VfxMenuConfig.CopyPasteMenuName, false, VfxMenuConfig.CopyPasteMenuPriority)]
        public static void Open()
        {
            var window = GetWindow<ShurikenCopyPasteWindow>();
            window.titleContent = VfxMenuConfig.ShurikenCopyPasteTitleContent;
            window.minSize = Config.WindowMinimumSize;
        }

        /// <summary>
        /// ウィンドウ描画
        /// </summary>
        protected override void OnGUI()
        {
            base.OnGUI();

            DrawSelectionBox();

            using (new EditorGUILayout.HorizontalScope())
            {
                DrawCopyPasteBox();
                DrawClipboardBox();
            }
        }

        protected override void Update()
        {
            base.Update();

            delayCall.DoUpdate();
        }

        /** ********************************************************************************
        * @summary 選択情報の表示
        ***********************************************************************************/
        private void DrawSelectionBox()
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("■ Selection", Config.BoxTitleLabelOption);
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField(Selection.activeGameObject, typeof(GameObject), true);
                EditorGUI.EndDisabledGroup();
            }
        }

        /** ********************************************************************************
        * @summary 情報の表示
        ***********************************************************************************/
        private void DrawClipboardBox()
        {
            using (new EditorGUILayout.VerticalScope(Config.BoxClipboardOption))
            {
                EditorGUILayout.LabelField("■ Clipboard");
                clipboardScrollPos = EditorGUILayout.BeginScrollView((clipboardScrollPos));
                
                foreach (var moduleType in Config.TargetModules)
                {
                    ModuleCopyPasteBase moduleData = copyPaste.GetModule(moduleType);
                    CustomUI.DrawModule(moduleData);
                }
                
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndScrollView();
            }
        }

        /** ********************************************************************************
        * @summary Copyボタンの描画
        ***********************************************************************************/
        private void DrawCopyPasteBox()
        {
            using (new EditorGUILayout.VerticalScope(Config.BoxCopyPasteStyle, Config.BoxCopyPasteOption))
            {
                EditorGUILayout.LabelField("■ Copy/Paste", Config.BoxTitleLabelOption); // タイトル表示
                EditorGUI.BeginDisabledGroup(Selection.activeGameObject?.GetComponent<ParticleSystem>() == null);

                using (new EditorGUILayout.HorizontalScope(Config.BoxCopyPasteStyle, Config.BoxCopyPasteOption))
                {
                    if (GUILayout.Button("Copy ALL"))
                    {
                        CopyAll();
                    }
                }

                buttonScrollPos = EditorGUILayout.BeginScrollView(buttonScrollPos);
                foreach (ModuleType moduleType in Config.TargetModules)
                {
                    DrawModuleCopyPaste(moduleType);
                }

                EditorGUI.EndDisabledGroup();

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndScrollView();
            }
        }

        /** ********************************************************************************
        * @summary モジュール別 Copy/Pasteボタンの描画
        ***********************************************************************************/
        private void DrawModuleCopyPaste(ModuleType moduleType)
        {
            using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
            {
                ModuleCopyPasteBase module = copyPaste.GetModule(moduleType);
                module.IsToggle = EditorGUILayout.ToggleLeft($"■ {moduleType}", module.IsToggle);　// チェックボックス
                // EditorGUILayout.LabelField($"■ {moduleType}", Config.LabelCopyPasteOption); // モジュール名
                EditorGUI.BeginDisabledGroup(true);
                // EditorGUILayout.TextField($"{module.CopyNameStamp}", GUILayout.ExpandWidth(true));
                EditorGUILayout.TextField($"{module.CopyNameStamp}", Config.TextCopyNameOption); // コピー情報の表示
                EditorGUI.EndDisabledGroup();

                // ペーストボタン
                if (GUILayout.Button("Paste", Config.ButtonStyle, Config.ButtonOption))
                {
                    var ps = Selection.activeGameObject.GetComponent<ParticleSystem>();

                    // Undo登録
                    Undo.RegisterCompleteObjectUndo(ps.gameObject, "Paste ParticleSystem Module");

                    if (ps == null) return;

                    bool needRestart = ps.gameObject.activeSelf;
                    if (needRestart)
                    {
                        ps.gameObject.SetActive(false);
                    }

                    copyPaste.Paste(ps, moduleType);
                    EditorSceneManager.MarkSceneDirty(ps.gameObject.scene);

                    if (needRestart)
                    {
                        ps.gameObject.SetActive(true);
                        ps.Play();
                    }
                    
                    Debug.Log($"Paste : {moduleType}");
                }
            }
        }

        /** ********************************************************************************
        * @summary 全部クリア
        ***********************************************************************************/
        private void ClearAll()
        {
            copyPaste.Clear();
        }

        /** ********************************************************************************
        * @summary 全部コピー
        ***********************************************************************************/
        private void CopyAll()
        {
            var ps = Selection.activeGameObject.GetComponent<ParticleSystem>();

            foreach (ModuleType moduleType in Config.TargetModules)
            {
                using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                {
                    copyPaste.Copy(ps, moduleType);
                }
            }
        }
    }
}