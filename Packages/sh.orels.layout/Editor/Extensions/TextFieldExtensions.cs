using UnityEngine.UIElements;

namespace ORL.Layout.Extensions
{
    public static partial class TextFieldExtensions
    {
        public static T Value<T>(this T el, string value) where T : TextInputBaseField<string>
        {
            el.value = value;
            return el;
        }

        public static T Delayed<T, SType>(this T el, bool value) where T : TextInputBaseField<SType>
        {
            el.isDelayed = value;
            return el;
        }

        public static T Delayed<T, SType>(this T el) where T : TextInputBaseField<SType>
        {
            el.isDelayed = true;
            return el;
        }
    }
}