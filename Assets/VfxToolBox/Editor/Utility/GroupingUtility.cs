using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Principal;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine.UIElements;
using VfxToolBox.ShurikenCopyPaste;

namespace VfxToolBox.Utility
{
    using UnityEngine;
    using UnityEditor;
    using System.Linq;

    public static class GroupingUtility
    {
        static bool called = false;

        // 選択しているオブジェクトをグループ化する
        // [MenuItem("GameObject/Custom/Group Selected %g", false, 100)]
        public static void GroupSelected()
        {
            if (!Selection.activeTransform) return;

            int firstIndex = Selection.transforms.Min(t => t.GetSiblingIndex());
            bool isParticleSystem = Selection.transforms
                .Any(t => t.GetComponent<ParticleSystem>());

            // 親オブジェクト
            var go = new GameObject(Selection.activeTransform.name + " Group");
            if (isParticleSystem)
            {
                var ps = go.AddComponent<ParticleSystem>();
                var emission = ps.emission;
                emission.enabled = false;

                var shape = ps.shape;
                shape.enabled = false;

                var renderer = go.GetComponent<ParticleSystemRenderer>();
                renderer.enabled = false;
            }

            Undo.RegisterCreatedObjectUndo(go, "Group Selected");
            go.transform.SetSiblingIndex(firstIndex);

            // 選択オブジェクトを入れ子にする
            go.transform.SetParent(Selection.activeTransform.parent, false);
            foreach (var transform in Selection.transforms.OrderBy(t => t.GetSiblingIndex()))
            {
                Undo.SetTransformParent(transform, go.transform, "Group Selected");
            }

            Selection.activeGameObject = go;
        }

        // 選択オブジェクトの子オブジェクトを外に出す
        // [MenuItem("GameObject/Custom/Ungroup Selected #%g", false, 101)]
        public static void UngroupSelected()
        {
            if (Selection.activeTransform == null) return;

            var transform = Selection.activeTransform;
            {
                if (transform.childCount == 0) return;
                
                var newParent = transform.parent;
                EditorSceneManager.MarkSceneDirty(transform.gameObject.scene);

                Transform[] children = new Transform[transform.childCount];
                for (int i = 0; i < children.Length; i++)
                {
                    children[i] = transform.GetChild(i);
                }

                int si = transform.GetSiblingIndex();
                for (var index = 0; index < children.Length; index++)
                {
                    Transform child = children[index];
                    called = true;

                    // 変更前の状態を記録
                    Undo.SetTransformParent(child, newParent, "Change Parent");

                    child.SetSiblingIndex(si + index + 1);
                }
            }

            EditorApplication.delayCall += () => { called = false; };
        }
    }
}