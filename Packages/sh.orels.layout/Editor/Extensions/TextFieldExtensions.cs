using UnityEngine.UIElements;

namespace ORL.Layout.Extensions
{
    public static partial class TextFieldExtensions
    {
        /// <summary>
        /// Sets the value of a string input field
        /// </summary>
        /// <example>
        /// <code>
        /// TextField().Value("Hello, world!");
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Value<T>(this T el, string value) where T : TextInputBaseField<string>
        {
            el.value = value;
            return el;
        }

        /// <summary>
        /// Sets any input field to be delayed or not
        /// </summary>
        /// <example>
        /// <code>
        /// TextField().Delayed(true);
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Delayed<T, SType>(this T el, bool value) where T : TextInputBaseField<SType>
        {
            el.isDelayed = value;
            return el;
        }

        /// <summary>
        /// Sets any input field to be delayed
        /// </summary>
        /// <example>
        /// <code>
        /// TextField().Delayed();
        /// </code>
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="SType"></typeparam>
        /// <param name="el"></param>
        /// <returns></returns>
        public static T Delayed<T, SType>(this T el) where T : TextInputBaseField<SType>
        {
            el.isDelayed = true;
            return el;
        }
    }
}