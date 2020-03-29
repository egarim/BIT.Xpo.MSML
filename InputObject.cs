using System;

namespace BIT.Xpo.MSML
{
    public class InputObject
    {
        public bool Label { get; }
        public string Text { get; }

        public InputObject(bool label, string text)
        {
            Label = label;
            Text = text;
        }
    }
}