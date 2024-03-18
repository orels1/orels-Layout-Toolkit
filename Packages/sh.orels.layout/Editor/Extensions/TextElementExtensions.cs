using UnityEngine.UIElements;
using System;

namespace ORL.Layout.Extensions
{
    public static partial class TextElementExtensions
    {
        public static T Text<T>(this T el, string text) where T : TextElement
        {
            el.text = text;
            return el;
        }

        public static T BoundPropText<T>(this T el, ReactiveProperty<string> prop) where T : TextElement
        {
            el.text = prop.Value;
            prop.OnValueChanged += s => el.text = s;
            return el;
        }

        public static T BoundPropText<T, SType>(this T el, ReactiveProperty<SType> prop, Func<SType, string> compute) where T : TextElement
        {
            el.text = compute(prop.Value);
            prop.OnValueChanged += s => el.text = compute(s);
            return el;
        }
    }
}