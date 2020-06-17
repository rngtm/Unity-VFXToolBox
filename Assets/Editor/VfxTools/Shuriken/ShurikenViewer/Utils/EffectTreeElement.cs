namespace VfxTools.ShurikenViewer
{
    using System.Collections.Generic;
    using UnityEngine;

    // 親子構造を表現するためのモデルを定義しておく
    // これがTreeViewに渡すモデルになる
    public class EffectTreeElement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EffectTreeElement Parent { get; private set; }
        private List<EffectTreeElement> _children = new List<EffectTreeElement>();
        public List<EffectTreeElement> Children { get { return _children; } }
        public EffectData EffectData { get; set; }

        /// <summary>
        /// 子を追加する
        /// </summary>
        public void AddChild(EffectTreeElement child)
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
        public void RemoveChild(EffectTreeElement child)
        {
            if (Children.Contains(child))
            {
                Children.Remove(child);
                child.Parent = null;
            }
        }
    }

}