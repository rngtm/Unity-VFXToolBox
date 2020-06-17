using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using UnityEditor;
using UnityEngine;

namespace VfxTools.ShurikenCopyPaste
{
    public class ParticleSystemCopyPaste
    {
        // モジュール
        private ModuleCopyPasteBase[] moduleDataArray = null;

        // モジュールの個数
        public int ModuleCount => moduleDataArray.Length;

        public string CopyTimeStamp { get; protected set; } = "";


        /** ********************************************************************************
        * @summary すべてのモジュール取得
        ***********************************************************************************/
        public IReadOnlyCollection<ModuleCopyPasteBase> GetModules()
        {
            return moduleDataArray;
        }

        /** ********************************************************************************
        * @summary モジュール取得
        ***********************************************************************************/
        public ModuleCopyPasteBase GetModule(ModuleType type)
        {
            return moduleDataArray[(int) type];
        }

        /** ********************************************************************************
        * @summary コンストラクタ
        ***********************************************************************************/
        public ParticleSystemCopyPaste()
        {
            CreateModuleData();
        }

        private void CreateModuleData()
        {
            moduleDataArray = moduleDataArray ?? new ModuleCopyPasteBase[System.Enum.GetNames(typeof(ModuleType)).Length];
            moduleDataArray = new ModuleCopyPasteBase[]
            {
                ModuleCopyPasteBase.Create(ModuleType.Main),
                ModuleCopyPasteBase.Create(ModuleType.Emission),
                ModuleCopyPasteBase.Create(ModuleType.Shape),
                ModuleCopyPasteBase.Create(ModuleType.VelocityOverLifetime),
                ModuleCopyPasteBase.Create(ModuleType.LimitVelocityOverLifetime),
                ModuleCopyPasteBase.Create(ModuleType.InheritVelocity),
                ModuleCopyPasteBase.Create(ModuleType.ForceOverLifetime),
                ModuleCopyPasteBase.Create(ModuleType.ColorOverLifetime),
                ModuleCopyPasteBase.Create(ModuleType.ColorBySpeed),
                ModuleCopyPasteBase.Create(ModuleType.SizeOverLifetime),
                ModuleCopyPasteBase.Create(ModuleType.SizeBySpeed),
                ModuleCopyPasteBase.Create(ModuleType.RotationOverLifetime),
                ModuleCopyPasteBase.Create(ModuleType.RotationBySpeed),
                ModuleCopyPasteBase.Create(ModuleType.ExternalForces),
                ModuleCopyPasteBase.Create(ModuleType.Noise),
                ModuleCopyPasteBase.Create(ModuleType.Collision),
                ModuleCopyPasteBase.Create(ModuleType.Triggers),
                ModuleCopyPasteBase.Create(ModuleType.SubEmitters),
                ModuleCopyPasteBase.Create(ModuleType.TextureSheetAnimation),
                ModuleCopyPasteBase.Create(ModuleType.Lights),
                ModuleCopyPasteBase.Create(ModuleType.Trails),
                ModuleCopyPasteBase.Create(ModuleType.CustomData),
                ModuleCopyPasteBase.Create(ModuleType.Renderer),
            };
        }

        /** ********************************************************************************
        * @summary モジュールのコピー
        ***********************************************************************************/
        public void Copy(ParticleSystem ps, ModuleType moduleType)
        {
            if (ps == null) return;

            var moduleCopyPaste = GetModule(moduleType);
            moduleCopyPaste.Copy(ps);
            CopyTimeStamp = moduleCopyPaste.CopyTimeStamp;
        }

        /** ********************************************************************************
        * @summary モジュールのペースト
        ***********************************************************************************/
        public void Paste(ParticleSystem ps, ModuleType moduleType)
        {
            if (ps == null) return;
            GetModule(moduleType).Paste(ps);
        }

        /** ********************************************************************************
        * @summary 中身のクリア
        ***********************************************************************************/
        public void Clear()
        {
            CreateModuleData();
        }
    }
}