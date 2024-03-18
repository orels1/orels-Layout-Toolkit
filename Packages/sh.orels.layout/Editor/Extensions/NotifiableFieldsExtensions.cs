using UnityEngine.UIElements;

namespace ORL.Layout.Extensions
{
    public static partial class NotifiableFieldExtensions
    {
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

        public static T OnChange<T, SType>(this T el, EventCallback<ChangeEvent<SType>> onChange) where T : INotifyValueChanged<SType>
        {
            el.RegisterValueChangedCallback(onChange);
            return el;
        }

    }
}