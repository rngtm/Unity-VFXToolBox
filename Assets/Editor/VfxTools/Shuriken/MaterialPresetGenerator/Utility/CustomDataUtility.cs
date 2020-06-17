using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VfxTools.Shuriken.ShaderPresetGenerator
{
    public static class CustomDataUtility
    {
        private static readonly string[] propertyName = new string[]
        {
            "CustomDataModule.vectorLabel0_0",
            "CustomDataModule.vectorLabel0_1",
            "CustomDataModule.vectorLabel0_2",
            "CustomDataModule.vectorLabel0_3",
            "CustomDataModule.vectorLabel1_0",
            "CustomDataModule.vectorLabel1_1",
            "CustomDataModule.vectorLabel1_2",
            "CustomDataModule.vectorLabel1_3",
        };

        /// <summary>
        /// CustomDataのVector値を取得
        /// </summary>
        public static void GetCustomDataVector(ParticleSystem ps, int customDataIndex, out string[] names, out ParticleSystem.MinMaxCurve[] values)
        {
            names = new string[4];
            values = new ParticleSystem.MinMaxCurve[4];

            var custom = ps.customData;
            var serializedObject = new SerializedObject(ps);
            var stream = (customDataIndex == 0) ? ParticleSystemCustomData.Custom1 : ParticleSystemCustomData.Custom2;
            for (int i = 0; i < names.Length; i++)
            {
                int propertyIndex = (customDataIndex == 0) ? i : i + 4;
                var customData = serializedObject.FindProperty(propertyName[propertyIndex]);
                names[i] = customData.stringValue;
                values[i] = custom.GetVector(stream, i);
            }

        }
        
        /// <summary>
        /// CustomDataのVector値を設定
        /// </summary>
        public static void SetCustomDataVector(ParticleSystem ps, int customDataIndex, string[] names, ParticleSystem.MinMaxCurve[] values)
        {
            var custom = ps.customData;
            custom.enabled = true;

            // mode
            var stream = (customDataIndex == 0) ? ParticleSystemCustomData.Custom1 : ParticleSystemCustomData.Custom2;
            custom.SetMode(stream, ParticleSystemCustomDataMode.Vector);
            custom.SetVectorComponentCount(stream, 4);

            var serializedObject = new SerializedObject(ps);
            for (int i = 0; i < names.Length; i++)
            {
                int propertyIndex = (customDataIndex == 0) ? i : i + 4;
                var customData = serializedObject.FindProperty(propertyName[propertyIndex]);
                customData.stringValue = names[i];
                custom.SetVector(stream, i, values[i]);
            }
            
            // 変更の適用
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// CustomDataの設定
        /// </summary>
        public static void SetCustomData(ParticleSystem ps, ShaderPresetData param)
        {
            var custom = ps.customData;
            custom.enabled = true;

            if (param.Custom1Count > 0)
            {
                custom.SetMode(ParticleSystemCustomData.Custom1, ParticleSystemCustomDataMode.Vector);
                custom.SetVectorComponentCount(ParticleSystemCustomData.Custom1, param.Custom1Count);
            }
            else
            {
                custom.SetMode(ParticleSystemCustomData.Custom2, ParticleSystemCustomDataMode.Disabled);
            }

            if (param.Custom2Count > 0)
            {
                custom.SetMode(ParticleSystemCustomData.Custom2, ParticleSystemCustomDataMode.Vector);
                custom.SetVectorComponentCount(ParticleSystemCustomData.Custom2, param.Custom2Count);
            }
            else
            {
                custom.SetMode(ParticleSystemCustomData.Custom2, ParticleSystemCustomDataMode.Disabled);
            }

            var serializedObject = new SerializedObject(ps);
            for (int i = 0; i < param.Custom1Count; i++)
            {
                int floatIndex = i;
                // custom.SetVector(ParticleSystemCustomData.Custom1, i, param.GetFloatValue(floatIndex).MinMaxCurve);
                var customData = serializedObject.FindProperty(propertyName[i]);
                customData.stringValue = param.GetFloatValue(floatIndex).Name;
            }
            
            for (int i = 0; i < param.Custom2Count; i++)
            {
                int floatIndex = i + 4;
                // custom.SetVector(ParticleSystemCustomData.Custom2, i, param.GetFloatValue(floatIndex).MinMaxCurve);
                var customData = serializedObject.FindProperty(propertyName[floatIndex]);
                customData.stringValue = param.GetFloatValue(floatIndex).Name;
            }

            // 変更の適用
            serializedObject.ApplyModifiedProperties();
        }


        /// <summary>
        /// VertexStreamの設定
        /// </summary>
        public static void SetVertexStream(ParticleSystemRenderer psr, ShaderPresetData param)
        {
            psr.SetActiveVertexStreams(param.VertexStreams.ToList());
        }
    }
}