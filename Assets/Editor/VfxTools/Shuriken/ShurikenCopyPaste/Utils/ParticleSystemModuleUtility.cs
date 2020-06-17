using UnityEngine;

namespace VfxTools.ShurikenCopyPaste
{   
    public static class ParticleSystemModuleUtility
    {
        /** ********************************************************************************
        * @summary モジュールの取得
        ***********************************************************************************/
        public static object GetModule(ParticleSystem ps, ModuleType moduleType)
        {
            object module;
            switch (moduleType)
            {
                case ModuleType.Main:
                    module = ps.main;
                    break;
                case ModuleType.Emission:
                    module = ps.emission;
                    break;
                case ModuleType.Shape:
                    module = ps.shape;
                    break;
                case ModuleType.VelocityOverLifetime:
                    module = ps.velocityOverLifetime;
                    break;
                case ModuleType.LimitVelocityOverLifetime:
                    module = ps.limitVelocityOverLifetime;
                    break;
                case ModuleType.InheritVelocity:
                    module = ps.inheritVelocity;
                    break;
                case ModuleType.ForceOverLifetime:
                    module = ps.forceOverLifetime;
                    break;
                case ModuleType.ColorOverLifetime:
                    module = ps.colorOverLifetime;
                    break;
                case ModuleType.ColorBySpeed:
                    module = ps.colorBySpeed;
                    break;
                case ModuleType.SizeOverLifetime:
                    module = ps.sizeOverLifetime;
                    break;
                case ModuleType.SizeBySpeed:
                    module = ps.sizeBySpeed;
                    break;
                case ModuleType.RotationOverLifetime:
                    module = ps.rotationOverLifetime;
                    break;
                case ModuleType.RotationBySpeed:
                    module = ps.rotationBySpeed;
                    break;
                case ModuleType.ExternalForces:
                    module = ps.externalForces;
                    break;
                case ModuleType.Noise:
                    module = ps.noise;
                    break;
                case ModuleType.Collision:
                    module = ps.collision;
                    break;
                case ModuleType.Triggers:
                    module = ps.trigger;
                    break;
                case ModuleType.SubEmitters:
                    module = ps.subEmitters;
                    break;
                case ModuleType.TextureSheetAnimation:
                    module = ps.textureSheetAnimation;
                    break;
                case ModuleType.Lights:
                    module = ps.lights;
                    break;
                case ModuleType.Trails:
                    module = ps.trails;
                    break;
                case ModuleType.CustomData:
                    module = ps.customData;
                    break;
                case ModuleType.Renderer:
                    module = ps.gameObject.GetComponent<ParticleSystemRenderer>();
                    break;
                default:
                    module = null;
                    break;
            }

            return module;
        }
    }
}