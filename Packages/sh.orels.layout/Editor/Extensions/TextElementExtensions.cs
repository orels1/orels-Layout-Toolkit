using UnityEngine.UIElements;
using System;

namespace ORL.Layout.Extensions
{
    public static partial class TextElementExtensions
    {
        /// <summary>
        /// Sets the text of a TextElement
        /// </summary>
        /// <example>
        /// <code>
        /// Button().Text("Click me!");
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static T Text<T>(this T el, string text) where T : TextElement
        {
            el.text = text;
            return el;
        }

        /// <summary>
        /// Binds a `ReactiveProperty` to the text of a TextElement.
        /// This will automatically update the text when the property changes via its `.Set()` method
        /// </summary>
        /// <example>
        /// <code>
        /// private ReactiveProperty<string> _text = new("Waiting");
        /// 
        /// protected override VisualElement Render()
        /// {
        ///     return VStack(
        ///         Label().BoundPropValue(_text),
        ///         Button(() => _text.Set("Thank you!")).Text("Click me!")
        ///     );
        /// }
        /// </code>
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <param name="el"></param>
        /// <param name="prop">`ReactiveProperty<string>` to bind to</param>
        /// <returns></returns>
        public static T BoundPropText<T>(this T el, ReactiveProperty<string> prop) where T : TextElement
        {
            el.text = prop.Value;
            prop.OnValueChanged += s => el.text = s;
            return el;
        }

        /// <summary>
        /// Binds a `ReactiveProperty` to the text of a TextElement via a custom compute function.
        /// This will automatically update the text when the property changes via its `.Set()` method
        /// </summary>
        /// <example>
        /// <code>
        /// private ReactiveProperty<int> _counter = new(0);
        /// 
        /// protected override VisualElement Render()
        /// {
        ///     return VStack(
        ///         Label().BoundPropValue(_counter, c => $"Counter: {c}"),
        ///         Button(() => _counter.Set(_counter + 1)).Text("Click me!")
        ///     );
        /// }
        /// </code>
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <param name="el"></param>
        /// <param name="prop">`ReactiveProperty<T>` to bind to</param>
        /// <param name="compute">Function to compute the text from the property value</param>
        /// <returns></returns>
        public static T BoundPropText<T, SType>(this T el, ReactiveProperty<SType> prop, Func<SType, string> compute) where T : TextElement
        {
            el.text = compute(prop.Value);
            prop.OnValueChanged += s => el.text = compute(s);
            return el;
        }
    }
}