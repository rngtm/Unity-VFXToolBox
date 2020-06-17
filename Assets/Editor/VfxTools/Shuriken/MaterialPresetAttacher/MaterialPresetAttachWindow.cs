using System.Runtime.CompilerServices;
using VfxTools.Shuriken.ShaderPresetGenerator;

namespace VfxTools.Shuriken.MaterialPresetAttacher
{
    using UnityEngine;
    using UnityEditor.IMGUI.Controls;
    using UnityEditor;
    using System.Collections.Generic;

    public class MaterialPresetAttachWindow : UnityEditor.EditorWindow
    {
        [SerializeField] private ParticleSystem particleSystem = null;
        [SerializeField] private ParticleSystemRenderer particleSystemRenderer = null;

        [UnityEditor.MenuItem(VfxMenuConfig.MaterialPresetAttacherMenuName, false,
            VfxMenuConfig.MaterialPresetAttacherMenuPriority)]
        public static void Open()
        {
            var window = GetWindow<MaterialPresetAttachWindow>();
            window.titleContent = new UnityEngine.GUIContent("Preset Attach");
            window.Show();
        }

        private void OnGUI()
        {
            DrawHeader();
            
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("■ ParticleSystem");

                EditorGUI.BeginChangeCheck();
                particleSystem = EditorGUILayout.ObjectField(particleSystem, typeof(ParticleSystem)) as ParticleSystem;
                if (EditorGUI.EndChangeCheck())
                {
                    if (particleSystem != null)
                    {
                        particleSystemRenderer = particleSystem.GetComponent<ParticleSystemRenderer>();
                    }
                    else
                    {
                        particleSystemRenderer = null;
                    }
                }

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.LabelField("■ Shader");
                EditorGUI.indentLevel++;
                EditorGUILayout.ObjectField("Shader", particleSystemRenderer?.sharedMaterial?.shader, typeof(Shader), false);
                EditorGUI.indentLevel--;
                EditorGUI.EndDisabledGroup();
            }

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUI.BeginDisabledGroup(particleSystemRenderer == null);
                
                if (GUILayout.Button("Apply"))
                {
                    var shader = particleSystemRenderer.sharedMaterial.shader;
                    if (shader != null)
                    {
                        var preset = ShaderPresetDatabase.Get().FindPreset(shader);
                        PresetUtility.ApplyPreset(particleSystem, preset);
                    }
                }

                EditorGUI.EndDisabledGroup();
            }
        }
        
        /// <summary>
        /// ヘッダ描画
        /// </summary>
        private void DrawHeader()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                if (GUILayout.Button("Open Generator", EditorStyles.toolbarButton))
                {
                    MaterialPresetGeneratorWindow.Open();
                }
                
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Select Preset", EditorStyles.toolbarButton))
                {
                    var shader = particleSystemRenderer?.sharedMaterial?.shader;
                    if (shader != null)
                    {
                        var preset = ShaderPresetDatabase.Get()?.FindPreset(shader);
                        if (preset != null)
                        {
                            Selection.activeObject = preset;
                        }
                        // EditorGUIUtility.PingObject(preset);   
                    }
                }
            }
        }
    }

// 親子構造を表現するためのモデルを定義しておく
// これがTreeViewに渡すモデルになる
    public class PresetTreeElement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PresetTreeElement Parent { get; private set; }
        private List<PresetTreeElement> _children = new List<PresetTreeElement>();

        public List<PresetTreeElement> Children
        {
            get { return _children; }
        }

        /// <summary>
        /// 子を追加する
        /// </summary>
        public void AddChild(PresetTreeElement child)
        {
            // 既に親がいたら削除
            if (child.Parent != null)
            {
                child.Parent.RemoveChild(child);
            }

            // 親子関係を設定
            Children.Add(child);
            child.Parent = this;
        }

        /// <summary>
        /// 子を削除する
        /// </summary>
        public void RemoveChild(PresetTreeElement child)
        {
            if (Children.Contains(child))
            {
                Children.Remove(child);
                child.Parent = null;
            }
        }
    }

    // TreeViewを表示するWindow
    class TreeViewExampleWindow : VfxWindowBase
    {
        // Stateはシ リアライズする（Unity再起動しても状態を保持するため）
        [SerializeField] private TreeViewState _treeViewState;

        private ExampleTreeView _treeView;
        private SearchField _searchField;

        // [MenuItem("Window/Tree View Example")]
        private static void Open()
        {
            GetWindow<TreeViewExampleWindow>(ObjectNames.NicifyVariableName(typeof(TreeViewExampleWindow).Name));
        }

        private void OnEnable()
        {
            // Stateは生成されていたらそれを使う
            if (_treeViewState == null)
            {
                _treeViewState = new TreeViewState();
            }

            // TreeViewを作成
            _treeView = new ExampleTreeView(_treeViewState);
            // 親子関係を適当に構築したモデルを作成
            // IDは任意だが被らないように
            var currentId = 0;
            var root = new PresetTreeElement {Id = ++currentId, Name = "1"};
            for (int i = 0; i < 2; i++)
            {
                var element = new PresetTreeElement {Id = ++currentId, Name = "1-" + (i + 1)};
                for (int j = 0; j < 2; j++)
                {
                    element.AddChild(new PresetTreeElement {Id = ++currentId, Name = "1-" + (i + 1) + "-" + (j + 1)});
                }

                root.AddChild(element);
            }

            // TreeViewを初期化
            _treeView.Setup(new List<PresetTreeElement> {root}.ToArray());

            // SearchFieldを初期化
            _searchField = new SearchField();
            _searchField.downOrUpArrowKeyPressed += _treeView.SetFocusAndEnsureSelectedItem;
        }

        /// <summary>
        /// ウィンドウ描画
        /// </summary>
        protected override void OnGUI()
        {
            base.OnGUI();

            // 検索窓を描画
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Space(100);
                GUILayout.FlexibleSpace();
                // TreeView.searchStringに検索文字列を入れると勝手に表示するItemを絞ってくれる
                _treeView.searchString = _searchField.OnToolbarGUI(_treeView.searchString);
            }

            // TreeViewを描画
            var rect = EditorGUILayout.GetControlRect(false, 200);
            _treeView.OnGUI(rect);
        }
    }

// 抽象クラスTreeViewを継承したクラスを作る
    public class ExampleTreeView : TreeView
    {
        private PresetTreeElement[] _baseElements;

        public ExampleTreeView(TreeViewState treeViewState) : base(treeViewState)
        {
        }

        public void Setup(PresetTreeElement[] baseElements)
        {
            // モデルを入れて
            _baseElements = baseElements;
            // Reload()で更新（BuildRootが呼ばれる）
            Reload();
        }

        // ルートとなるTreeViewItemを作って返す
        // Reloadが呼ばれるたびに呼ばれる
        protected override TreeViewItem BuildRoot()
        {
            // RootのItemはdepth = -1として定義する
            var root = new TreeViewItem {id = 0, depth = -1, displayName = "Root"};

            //　モデルからTreeViewItemの親子関係を構築
            var elements = new List<TreeViewItem>();
            foreach (var baseElement in _baseElements)
            {
                var baseItem = CreateTreeViewItem(baseElement);
                root.AddChild(baseItem);
                AddChildrenRecursive(baseElement, baseItem);
            }

            // 親子関係に基づいてDepthを自動設定するメソッド
            SetupDepthsFromParentsAndChildren(root);

            return root;
        }

        /// <summary>
        /// モデルとItemから再帰的に子Itemを作成・追加する
        /// </summary>
        private void AddChildrenRecursive(PresetTreeElement model, TreeViewItem item)
        {
            foreach (var childModel in model.Children)
            {
                var childItem = CreateTreeViewItem(childModel);
                item.AddChild(childItem);
                AddChildrenRecursive(childModel, childItem);
            }
        }

        /// <summary>
        /// ExampleTreeElementからTreeViewItemを作成する
        /// </summary>
        private TreeViewItem CreateTreeViewItem(PresetTreeElement model)
        {
            return new TreeViewItem {id = model.Id, displayName = model.Name};
        }
    }
}