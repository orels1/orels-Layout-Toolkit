using UnityEngine.UIElements;

namespace ORL.Layout.Extensions
{
    public static partial class NotifiableFieldExtensions
    {
        /// <summary>
        /// Binds a property to the value of the field
        /// </summary>
        /// <example>
        /// <code>
        /// private ReactiveProperty<string> _text = new("Hello");
        /// 
        /// protected override VisualElement Render()
        /// {
        ///     return VStack(
        ///         TextField("Type some text!").BoundPropValue(_text),
        ///         Label().BoundPropText(_text)
        ///     );
        /// }
        /// </code>
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="SType"></typeparam>
        /// <param name="el"></param>
        /// <param name="prop">`ReactiveProperty<T>` to bind to, must match the field type</param>
        /// <returns></returns>
        public static T BoundPropValue<T, SType>(this T el, ReactiveProperty<SType> prop) where T : INotifyValueChanged<SType>
        {
            el.value = prop.Value;
            el.RegisterValueChangedCallback(s =>
            {
                prop.Set(s.newValue);
            });
            // Ensure two-way communication
            prop.OnValueChanged += s => el.value = s;
            return el;
        }

        /// <summary>
        /// Binds a generic callback to the value change event
        /// </summary>
        /// <example>
        /// <code>
        /// TextField("Type some text").OnChange(e => Debug.Log(e.newValue));
        /// </code>
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="SType"></typeparam>
        /// <param name="el"></param>
        /// <param name="onChange">`ChangeEvent` callback to bind</param>
        /// <returns></returns>
        public static T OnChange<T, SType>(this T el, EventCallback<ChangeEvent<SType>> onChange) where T : INotifyValueChanged<SType>
        {
            el.RegisterValueChangedCallback(onChange);
            return el;
        }

    }
}