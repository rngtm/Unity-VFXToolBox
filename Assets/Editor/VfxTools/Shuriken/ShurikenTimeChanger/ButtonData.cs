using System;

namespace VfxTools.Shuriken.TimeChanger
{
    public class ButtonData
    {
        public string ButtonLabel { get; protected set; }
        public Func<float, float> Callback { get; protected set; }

        public ButtonData(string label, Func<float, float> callback)
        {
            ButtonLabel = label;
            Callback = callback;
        }
    }

    public class AddButtonData : ButtonData
    {
        public AddButtonData(float add) 
            : base(add > 0 ? $"+{add}" : $"{add}", startDelay => startDelay + add)
        {
        }
    }

    public class MultiplyButtonData : ButtonData
    {
        public MultiplyButtonData(float multiply) 
            : base($"x{multiply:0.0}", startDelay => startDelay * multiply)
        {
        }
    }
}