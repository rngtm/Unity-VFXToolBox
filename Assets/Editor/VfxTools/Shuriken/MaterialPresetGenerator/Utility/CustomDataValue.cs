using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VfxTools.Shuriken.ShaderPresetGenerator
{
    [Serializable]
    public class CustomDataValue
    {
        [SerializeField] private string name = "Value";
        [SerializeField] private ParticleSystem.MinMaxCurve minMaxCurve = new ParticleSystem.MinMaxCurve();
            
        public string Name => name;
        public ParticleSystem.MinMaxCurve MinMaxCurve => minMaxCurve;

        public CustomDataValue(string name, ParticleSystem.MinMaxCurve value)
        {
            this.name = name;
            this.minMaxCurve = value;
        }
    }
}