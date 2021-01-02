using System;
using System.Collections.Generic;
using UnityEngine;
using VfxToolBox.Shuriken.ShaderPresetGenerator;

namespace VfxToolBox.ShurikenCopyPaste
{
    /** ********************************************************************************
    * @summary ParticleSystemモジュールのコピーペーストの基底クラス
    ***********************************************************************************/
    public abstract class ModuleCopyPasteBase
    {
        private MemberValue[] memberValues = new MemberValue[0];
        public bool IsOpen { get; set; } // 折りたたみを開いているかどうか
        public bool IsToggle { get; set; } // チェックボックスが入っているか
        public string CopyNameStamp { get; protected set; } = "";
        public string CopyTimeStamp { get; protected set; } = "";
        public ModuleType ModuleType { get; protected set; } // モジュールのタイプ
        public int MemberCount => memberValues.Length;

        public static ModuleCopyPasteBase Create(ModuleType moduleType)
        {
            ModuleCopyPasteBase instance = null;

            switch (moduleType)
            {
                case ModuleType.Main:
                    instance = new ModuleCopyPasteBaseMain();
                    break;
                case ModuleType.Emission:
                    instance = new ModuleCopyPasteBaseEmission();
                    break;
                case ModuleType.Shape:
                    instance = new ModuleCopyPasteBaseShape();
                    break;
                case ModuleType.VelocityOverLifetime:
                    instance = new ModuleCopyPasteBaseVelocityOverLifetime();
                    break;
                case ModuleType.LimitVelocityOverLifetime:
                    instance = new ModuleCopyPasteBaseLimitVelocityOverLifetime();
                    break;
                case ModuleType.InheritVelocity:
                    instance = new ModuleCopyPasteBaseInheritVelocity();
                    break;
                case ModuleType.ForceOverLifetime:
                    instance = new ModuleCopyPasteBaseForceOverLifetime();
                    break;
                case ModuleType.ColorOverLifetime:
                    instance = new ModuleCopyPasteBaseColorOverLifetime();
                    break;
                case ModuleType.ColorBySpeed:
                    instance = new ModuleCopyPasteBaseColorBySpeed();
                    break;
                case ModuleType.SizeOverLifetime:
                    instance = new ModuleCopyPasteBaseSizeOverLifetime();
                    break;
                case ModuleType.SizeBySpeed:
                    instance = new ModuleCopyPasteBaseSizeBySpeed();
                    break;
                case ModuleType.RotationOverLifetime:
                    instance = new ModuleCopyPasteBaseRotationOverLifetime();
                    break;
                case ModuleType.RotationBySpeed:
                    instance = new ModuleCopyPasteBaseRotationBySpeed();
                    break;
                case ModuleType.ExternalForces:
                    instance = new ModuleCopyPasteBaseExternalForces();
                    break;
                case ModuleType.Noise:
                    instance = new ModuleCopyPasteBaseNoise();
                    break;
                case ModuleType.Collision:
                    instance = new ModuleCopyPasteBaseCollision();
                    break;
                case ModuleType.Triggers:
                    instance = new ModuleCopyPasteBaseTriggers();
                    break;
                case ModuleType.SubEmitters:
                    instance = new ModuleCopyPasteBaseSubEmitters();
                    break;
                case ModuleType.TextureSheetAnimation:
                    instance = new ModuleCopyPasteBaseTextureSheetAnimation();
                    break;
                case ModuleType.Lights:
                    instance = new ModuleCopyPasteBaseLights();
                    break;
                case ModuleType.Trails:
                    instance = new ModuleCopyPasteBaseTrails();
                    break;
                case ModuleType.CustomData:
                    instance = new ModuleCopyPasteBaseCustomData();
                    break;
                case ModuleType.Renderer:
                    instance = new ModuleCopyPasteBaseRenderer();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(moduleType), moduleType, null);
            }

            instance.ModuleType = moduleType;
            return instance;
        }

        /** ********************************************************************************
        * @summary コンストラクタ
        ***********************************************************************************/
        protected ModuleCopyPasteBase(ModuleType moduleType)
        {
            ModuleType = moduleType;
        }

        /** ********************************************************************************
        * @summary 値を持っているかどうか
        ***********************************************************************************/
        public bool HasValue => memberValues.Length > 0;

        /** ********************************************************************************
        * @summary ParticleSystem内のモジュールをコピーする
        ***********************************************************************************/
        public void Copy(ParticleSystem ps)
        {
            if (ps == null) return;

            var particleSystemModule = ParticleSystemModuleUtility.GetModule(ps, ModuleType); // ParticleSystem内のモジュール選択
            CopyTimeStamp = $"{DateTime.Now:hh:mm:ss}";
            CopyNameStamp = ps.gameObject.name;
            memberValues = Exctractor.GetModuleValues(particleSystemModule); // メンバの数値取得

            CopyInternal(ps);  // リフレクションでアクセスできないデータのコピー
        }

        /** ********************************************************************************
        * @summary ParticleSystem内のモジュールをコピーする
        ***********************************************************************************/
        public void Paste(ParticleSystem ps)
        {
            if (ps == null) return;

            var particleSystemModule = ParticleSystemModuleUtility.GetModule(ps, ModuleType); // ParticleSystem内のモジュール選択
            Exctractor.SetModuleValue(particleSystemModule, this);

            PasteInternal(ps); // リフレクションでアクセスできないデータのペースト
        }

        /** ********************************************************************************
        * @summary 抽出したメンバ値の取得
        ***********************************************************************************/
        public IEnumerable<MemberValue> GetMemberValues()
        {
            return memberValues;
        }

        /** ********************************************************************************
        * @summary 値の取得(リフレクションで取得できない値の取得)
        ***********************************************************************************/
        protected abstract void CopyInternal(ParticleSystem ps);

        /** ********************************************************************************
        * @summary 値の上書き(リフレクションで設定できない値の設定)
        ***********************************************************************************/
        protected abstract void PasteInternal(ParticleSystem ps);

        /** ********************************************************************************
        * @summary リフレクションで設定できない値の取得
        ***********************************************************************************/
        public virtual IEnumerable<string> GetInternalValues()
        {
            return null;
        }
    }


    public class ModuleCopyPasteBaseMain : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseMain() : base(ModuleType.Main)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseEmission : ModuleCopyPasteBase
    {
        private ModuleData data = new ModuleData();
        private class ModuleData
        {
            public ParticleSystem.Burst[] Bursts;
        }
        
        public ModuleCopyPasteBaseEmission() : base(ModuleType.Emission)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
            var emission = ps.emission;
            data.Bursts = new ParticleSystem.Burst[emission.burstCount];
            for (int i = 0; i < emission.burstCount; i++)
            {
                data.Bursts[i] = emission.GetBurst(i);
            }
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
            for (int i = 0; i < data.Bursts.Length; i++)
            {
                ps.emission.SetBurst(i, data.Bursts[i]);
            }
        }
    }

    public class ModuleCopyPasteBaseShape : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseShape() : base(ModuleType.Shape)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }


        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseVelocityOverLifetime : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseVelocityOverLifetime() : base(ModuleType.VelocityOverLifetime)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseLimitVelocityOverLifetime : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseLimitVelocityOverLifetime() : base(ModuleType.LimitVelocityOverLifetime)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseInheritVelocity : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseInheritVelocity() : base(ModuleType.InheritVelocity)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseForceOverLifetime : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseForceOverLifetime() : base(ModuleType.ForceOverLifetime)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseColorOverLifetime : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseColorOverLifetime() : base(ModuleType.ColorOverLifetime)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseColorBySpeed : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseColorBySpeed() : base(ModuleType.ColorBySpeed)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseSizeOverLifetime : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseSizeOverLifetime() : base(ModuleType.SizeOverLifetime)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseSizeBySpeed : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseSizeBySpeed() : base(ModuleType.SizeBySpeed)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseRotationOverLifetime : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseRotationOverLifetime() : base(ModuleType.RotationOverLifetime)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseRotationBySpeed : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseRotationBySpeed() : base(ModuleType.RotationBySpeed)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseExternalForces : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseExternalForces() : base(ModuleType.ExternalForces)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseNoise : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseNoise() : base(ModuleType.Noise)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseCollision : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseCollision() : base(ModuleType.Collision)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseTriggers : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseTriggers() : base(ModuleType.Triggers)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseSubEmitters : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseSubEmitters() : base(ModuleType.SubEmitters)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseTextureSheetAnimation : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseTextureSheetAnimation() : base(ModuleType.TextureSheetAnimation)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseLights : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseLights() : base(ModuleType.Lights)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseTrails : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseTrails() : base(ModuleType.Trails)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }

    public class ModuleCopyPasteBaseCustomData : ModuleCopyPasteBase
    {
        private ModuleData data = new ModuleData();
        
        public ModuleCopyPasteBaseCustomData() : base(ModuleType.CustomData)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
            string[] name1, name2;
            ParticleSystem.MinMaxCurve[] vector1, vector2;
            CustomDataUtility.GetCustomDataVector(ps, 0, out name1, out vector1);
            CustomDataUtility.GetCustomDataVector(ps, 1, out name2, out vector2);

            // CustomDataのコピー
            data.CustomData1_Name = name1;
            data.CustomData1_Vector = vector1;
            data.CustomData2_Name = name2;
            data.CustomData2_Vector = vector2;
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
            // CustomDataのペースト
            CustomDataUtility.SetCustomDataVector(ps, 0, data.CustomData1_Name, data.CustomData1_Vector);
            CustomDataUtility.SetCustomDataVector(ps, 1, data.CustomData2_Name, data.CustomData2_Vector);
        }
        
        private class ModuleData
        {
            public string[] CustomData1_Name { get; set; }
            public string[] CustomData2_Name { get; set; }
            public ParticleSystem.MinMaxCurve[] CustomData1_Vector { get; set; }
            public ParticleSystem.MinMaxCurve[] CustomData2_Vector { get; set; }

            public ModuleData()
            {
                CustomData1_Name = new string[4];
                CustomData2_Name = new string[4];
                CustomData1_Vector = new ParticleSystem.MinMaxCurve[4];
                CustomData2_Vector = new ParticleSystem.MinMaxCurve[4];
            }
        }
    }

    public class ModuleCopyPasteBaseRenderer : ModuleCopyPasteBase
    {
        public ModuleCopyPasteBaseRenderer() : base(ModuleType.Renderer)
        {
        }

        protected override void CopyInternal(ParticleSystem ps)
        {
        }

        protected override void PasteInternal(ParticleSystem ps)
        {
        }
    }
}