using System;
using ORL.Layout.Elements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ORL.Layout.Extensions
{
    public static partial class VisualElementExtensions
    {
        public static T Class<T>(this T el, params string[] classNames) where T : VisualElement
        {
            foreach (var className in classNames)
            {
                el.AddToClassList(className);
            }
            return el;
        }

        public static T Children<T>(this T el, params VisualElement[] children) where T : VisualElement
        {
            foreach (var child in children)
            {
                el.Add(child);
            }

            return el;
        }

        public static T Ref<T>(this T el, ref T target) where T : VisualElement
        {
            target = el;
            return el;
        }

        public static T UserData<T>(this T el, object userData) where T : VisualElement
        {
            el.userData = userData;
            return el;
        }

        public static T Name<T>(this T el, string name) where T : VisualElement
        {
            el.name = name;
            return el;
        }

        public static T ViewDataKey<T>(this T el, string viewDataKey) where T : VisualElement
        {
            el.viewDataKey = viewDataKey;
            return el;
        }

        public static T Style<T>(this T el, Action<IStyle> setter) where T : VisualElement
        {
            setter(el.style);
            return el;
        }

        public static T Padding<T>(this T el, params float[] padding) where T : VisualElement
        {
            switch (padding.Length)
            {
                case 1:
                    el.style.paddingTop = padding[0];
                    el.style.paddingRight = padding[0];
                    el.style.paddingBottom = padding[0];
                    el.style.paddingLeft = padding[0];
                    break;
                case 2:
                    el.style.paddingTop = padding[0];
                    el.style.paddingBottom = padding[0];
                    el.style.paddingLeft = padding[1];
                    el.style.paddingRight = padding[1];
                    break;
                case 3:
                    el.style.paddingTop = padding[0];
                    el.style.paddingLeft = padding[1];
                    el.style.paddingRight = padding[1];
                    el.style.paddingBottom = padding[2];
                    break;
                case 4:
                    el.style.paddingTop = padding[0];
                    el.style.paddingRight = padding[1];
                    el.style.paddingBottom = padding[2];
                    el.style.paddingLeft = padding[3];
                    break;
            }

            return el;
        }

        public static T Margin<T>(this T el, params float[] margin) where T : VisualElement
        {
            switch (margin.Length)
            {
                case 1:
                    el.style.marginTop = margin[0];
                    el.style.marginRight = margin[0];
                    el.style.marginBottom = margin[0];
                    el.style.marginLeft = margin[0];
                    break;
                case 2:
                    el.style.marginTop = margin[0];
                    el.style.marginBottom = margin[0];
                    el.style.marginLeft = margin[1];
                    el.style.marginRight = margin[1];
                    break;
                case 3:
                    el.style.marginTop = margin[0];
                    el.style.marginLeft = margin[1];
                    el.style.marginRight = margin[1];
                    el.style.marginBottom = margin[2];
                    break;
                case 4:
                    el.style.marginTop = margin[0];
                    el.style.marginRight = margin[1];
                    el.style.marginBottom = margin[2];
                    el.style.marginLeft = margin[3];
                    break;
            }

            return el;
        }

        public static T Relative<T>(this T el) where T : VisualElement
        {
            el.style.position = Position.Relative;
            return el;
        }

        public static T Absolute<T>(this T el) where T : VisualElement
        {
            el.style.position = Position.Absolute;
            return el;
        }

        public static T Top<T>(this T el, float offset) where T : VisualElement
        {
            el.style.top = offset;
            return el;
        }

        public static T Left<T>(this T el, float offset) where T : VisualElement
        {
            el.style.top = offset;
            return el;
        }

        public static T Right<T>(this T el, float offset) where T : VisualElement
        {
            el.style.right = offset;
            return el;
        }

        public static T Bottom<T>(this T el, float offset) where T : VisualElement
        {
            el.style.bottom = offset;
            return el;
        }

        public static T Flex<T>(this T el, float size) where T : VisualElement
        {
            el.style.flexBasis = new StyleLength(new Length(0, LengthUnit.Percent));
            el.style.flexShrink = 1;
            el.style.flexGrow = size;
            return el;
        }

        public static T Wrapped<T>(this T el) where T : VisualElement
        {
            el.style.whiteSpace = WhiteSpace.Normal;
            return el;
        }

        public static T Bold<T>(this T el) where T : VisualElement
        {
            if (el.style.unityFontStyleAndWeight == FontStyle.Italic)
            {
                el.style.unityFontStyleAndWeight = FontStyle.BoldAndItalic;
                return el;
            }

            el.style.unityFontStyleAndWeight = FontStyle.Bold;
            return el;
        }

        public static T Italic<T>(this T el) where T : VisualElement
        {
            if (el.style.unityFontStyleAndWeight == FontStyle.Bold)
            {
                el.style.unityFontStyleAndWeight = FontStyle.BoldAndItalic;
                return el;
            }

            el.style.unityFontStyleAndWeight = FontStyle.Italic;
            return el;
        }

        public static T BoldItalic<T>(this T el) where T : VisualElement
        {
            el.style.unityFontStyleAndWeight = FontStyle.BoldAndItalic;
            return el;
        }

        public static T Color<T>(this T el, Color color) where T : VisualElement
        {
            el.style.color = color;
            return el;
        }

        public static T Width<T>(this T el, float width) where T : VisualElement
        {
            el.style.width = width;
            return el;
        }

        public static T BindingPath<T>(this T el, string bindingPath) where T : IBindable
        {
            el.bindingPath = bindingPath;
            return el;
        }

        public static T BindSerializedObject<T>(this T el, SerializedObject serializedObject) where T : VisualElement
        {
            el.Bind(serializedObject);
            return el;
        }

        public static T AlignItems<T>(this T el, Align align) where T : VisualElement
        {
            el.style.alignItems = align;
            return el;
        }

        public static T AlignContent<T>(this T el, Align align) where T : VisualElement
        {
            el.style.alignContent = align;
            return el;
        }

        public static T JustifyContent<T>(this T el, Justify justify) where T : VisualElement
        {
            el.style.justifyContent = justify;
            return el;
        }

        public static T Direction<T>(this T el, FlexDirection direction) where T : VisualElement
        {
            el.style.flexDirection = direction;
            return el;
        }

        public static T ShowIf<T>(this T el, Func<bool> condition) where T : VisualElement
        {
            el.style.display = condition() ? DisplayStyle.Flex : DisplayStyle.None;
            el.schedule.Execute(() => el.style.display = condition() ? DisplayStyle.Flex : DisplayStyle.None).Every(300);
            return el;
        }

        public static T HideIf<T>(this T el, Func<bool> condition) where T : VisualElement
        {
            el.style.display = condition() ? DisplayStyle.None : DisplayStyle.Flex;
            el.schedule.Execute(() => el.style.display = condition() ? DisplayStyle.None : DisplayStyle.Flex).Every(300);
            return el;
        }

        public static T BindVisibleState<T>(this T el, ReactiveProperty<bool> prop) where T : VisualElement
        {
            el.style.display = prop.Value ? DisplayStyle.Flex : DisplayStyle.None;
            prop.OnValueChanged += value => el.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            return el;
        }

        public static T OnMount<T>(this T el, Action<T> onMount) where T : VisualElement
        {
            el.RegisterCallback<AttachToPanelEvent>(e => onMount(el));
            return el;
        }

        public static T OnUnmount<T>(this T el, Action<T> onUnmount) where T : VisualElement
        {
            el.RegisterCallback<DetachFromPanelEvent>(e => onUnmount(el));
            return el;
        }
    }
}