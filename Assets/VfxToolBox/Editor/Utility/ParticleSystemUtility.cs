using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VfxToolBox.Utility
{
    /// <summary>
    /// ParticlySystem汎用メソッド定義
    /// </summary>
    public static class ParticleSystemUtility
    {
        /// <summary>
        /// 空のParticleSystemを作成する
        /// </summary>
        public static ParticleSystem CreateEmptyParticleSystem(string name)
        {
            var go = new GameObject(name);
            Undo.RegisterCreatedObjectUndo(go, "Create Object");

            var ps = go.AddComponent<ParticleSystem>();
            var emission = ps.emission;
            emission.enabled = false;

            var shape = ps.shape;
            shape.enabled = false;

            var renderer = go.GetComponent<ParticleSystemRenderer>();
            renderer.enabled = false;
            return ps;
        }

        /// <summary>
        /// ParticleSystemを作成する
        /// </summary>
        public static ParticleSystem CreateParticleSystem(string name)
        {
            var go = new GameObject(name);
            Undo.RegisterCreatedObjectUndo(go, "Create Object");

            var ps = go.AddComponent<ParticleSystem>();
            return ps;
        }
        
        /// <summary>
        /// 選択しているParticleSystemを取得
        /// </summary>
        public static IEnumerable<ParticleSystem> GetSelectionParticleSystem()
        {
            foreach (var gameObject in Selection.gameObjects)
            {
                var particleSystem = gameObject.GetComponent<ParticleSystem>();
                if (particleSystem == null) continue;

                yield return particleSystem;
            }
        }
        
        /// <summary>
        /// 選択しているParticleSystemを子を含めて取得
        /// </summary>
        public static IEnumerable<ParticleSystem> GetSelectionParticleSystemRecursive()
        {
            foreach (var ps in GetSelectionParticleSystem())
            {
                foreach (var childPs in ps.GetComponentsInChildren<ParticleSystem>())
                {
                    yield return childPs;
                }
            }
        }
    }
}