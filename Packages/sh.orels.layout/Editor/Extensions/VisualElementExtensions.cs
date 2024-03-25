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
        /// <summary>
        /// Assigns a list of USS classes to a VisualElement
        /// </summary>
        /// <example>
        /// <code>
        /// Button().Class("btn", "btn-primary");
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="classNames">List ot class names to add</param>
        /// <returns></returns>
        public static T Class<T>(this T el, params string[] classNames) where T : VisualElement
        {
            foreach (var className in classNames)
            {
                el.AddToClassList(className);
            }
            return el;
        }

        /// <summary>
        /// Adds child elements to a VisualElement
        /// </summary>
        /// <example>
        /// <code>
        /// Foldout().Children(
        ///    Label().Text("Hello, world!"),
        ///    Button().Text("Click me!")
        /// );
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="children"></param>
        /// <returns></returns>
        public static T Children<T>(this T el, params VisualElement[] children) where T : VisualElement
        {
            foreach (var child in children)
            {
                el.Add(child);
            }

            return el;
        }

        /// <summary>
        /// Saves a reference to the created VisualElement in a provided variable reference for future use
        /// </summary>
        /// <example>
        /// <code>
        /// Label label = null;
        /// return VStack(
        ///   Label().Text("Hello, world!").Ref(ref label),
        ///   Button(() => label.text = "Wow, you clicked me!").Text("Click me!")
        /// )
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T Ref<T>(this T el, ref T target) where T : VisualElement
        {
            target = el;
            return el;
        }

        /// <summary>
        /// Stores an arbitary object in the `userData` property of a VisualElement
        /// </summary>
        /// <example>
        /// <code>
        /// Label label = null;
        /// return VStack(
        ///     Label("Heyo!").UserData("Secret message!").Ref(ref label),
        ///     Button(() => label.text = label.userData).Text("Reveal secrets!")
        /// );
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        public static T UserData<T>(this T el, object userData) where T : VisualElement
        {
            el.userData = userData;
            return el;
        }

        /// <summary>
        /// Assigns a name to a VisualElement
        /// </summary>
        /// <example>
        /// <code>
        /// Label().Name("my-label");
        /// </code>
        /// </example>
        /// <list>
        /// <item>This can be used to target elements via USS #selectors, e.g. #my-label</item>
        /// </list>
        /// <param name="el"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T Name<T>(this T el, string name) where T : VisualElement
        {
            el.name = name;
            return el;
        }

        /// <summary>
        /// Assigns a view data key to a VisualElement
        /// </summary>
        /// <example>
        /// <code>
        /// Foldout().ViewDataKey("my-foldout").Children(
        ///    Label().Text("Hello, world!")
        /// );
        /// </code>
        /// </example>
        /// <list>
        /// <item>This enables view state persistence on supported elements, e.g. Foldout or Scrollview. More info on Unity docs</item>
        /// </list>
        /// <param name="el"></param>
        /// <param name="viewDataKey"></param>
        /// <returns></returns>
        public static T ViewDataKey<T>(this T el, string viewDataKey) where T : VisualElement
        {
            el.viewDataKey = viewDataKey;
            return el;
        }

        /// <summary>
        /// Enables arbitrary styles manipulation via an `Action<IStyle>` delegate
        /// </summary>
        /// <example>
        /// <code>
        /// Label("Text with background color").Style(style => style.backgroundColor = Color.red);
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="setter">`Action<IStyle>` that manipulates the style object of the target element</param>
        /// <returns></returns>
        public static T Style<T>(this T el, Action<IStyle> setter) where T : VisualElement
        {
            setter(el.style);
            return el;
        }

        /// <summary>
        /// Sets the edge padding of a VisualElement
        /// </summary>
        /// <example>
        /// <code>
        /// VStack(
        ///    Label("Padded label").Padding(10),
        ///    Label("Another padded label").Padding(5, 10),
        ///    Label("Yet another padded label").Padding(5, 10, 15),
        ///    Label("The last padded label").Padding(5, 10, 15, 20)
        /// );
        /// </code>
        /// </example>
        /// <list>
        /// <item>1 value: sets all paddings to the same value</item>
        /// <item>2 values: sets top and bottom paddings to the first value, left and right paddings to the second value</item>
        /// <item>3 values: sets top padding to the first value, left and right paddings to the second value, bottom padding to the third value</item>
        /// <item>4 values: sets top, right, bottom and left paddings to the respective values</item>
        /// </list>
        /// <param name="el"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Sets the edge margin of a VisualElement
        /// </summary>
        /// <example>
        /// <code>
        /// VStack(
        ///    Label("Offset label").Margin(10),
        ///    Label("Another Offset label").Margin(5, 10),
        ///    Label("Yet another Offset label").Margin(5, 10, 15),
        ///    Label("The last Offset label").Margin(5, 10, 15, 20)
        /// );
        /// </code>
        /// </example>
        /// <list>
        /// <item>1 value: sets all margins to the same value</item>
        /// <item>2 values: sets top and bottom margins to the first value, left and right margins to the second value</item>
        /// <item>3 values: sets top margin to the first value, left and right margins to the second value, bottom margin to the third value</item>
        /// <item>4 values: sets top, right, bottom and left margins to the respective values</item>
        /// </list>
        /// <param name="el"></param>
        /// <param name="margin"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Sets the VisualElement's positioning to be parent-relative
        /// </summary>
        /// <example>
        /// <code>
        /// VStack(
        ///     Label("I'm relative!").Relative().Top(10)
        /// );
        /// </code>
        /// </example>
        /// <list>
        /// <item>Useful for positioning elements inside a container</item>
        /// </list>
        /// <param name="el"></param>
        /// <returns></returns>
        public static T Relative<T>(this T el) where T : VisualElement
        {
            el.style.position = Position.Relative;
            return el;
        }

        /// <summary>
        /// Sets the VisualElement's positioning to be absolute
        /// </summary>
        /// <example>
        /// <code>
        /// VStack(
        ///    Label("I can overlap other elements").Absolute().Top(10).Left(30)
        /// );
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <returns></returns>
        public static T Absolute<T>(this T el) where T : VisualElement
        {
            el.style.position = Position.Absolute;
            return el;
        }

        /// <summary>
        /// Adds top offset to a VisualElement
        /// </summary>
        /// <example>
        /// <code>
        /// VStack(
        ///   Label("I'm 10px offset from the top").Top(10)
        /// );
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="offset">Amount to offset by</param>
        /// <returns></returns>
        public static T Top<T>(this T el, float offset) where T : VisualElement
        {
            el.style.top = offset;
            return el;
        }

        /// <summary>
        /// Adds left offset to a VisualElement
        /// </summary>
        /// <example>
        /// <code>
        /// VStack(
        ///   Label("I'm 10px offset from the left").Left(10)
        /// );
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="offset">Amount to offset by</param>
        /// <returns></returns>
        public static T Left<T>(this T el, float offset) where T : VisualElement
        {
            el.style.top = offset;
            return el;
        }

        /// <summary>
        /// Adds right offset to a VisualElement
        /// </summary>
        /// <example>
        /// <code>
        /// VStack(
        ///   Label("I'm 10px offset from the right").Right(10)
        /// );
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="offset">Amount to offset by</param>
        /// <returns></returns>
        public static T Right<T>(this T el, float offset) where T : VisualElement
        {
            el.style.right = offset;
            return el;
        }

        /// <summary>
        /// Adds bottom offset to a VisualElement
        /// </summary>
        /// <example>
        /// <code>
        /// VStack(
        ///   Label("I'm 10px offset from the bottom").Bottom(10)
        /// );
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="offset">Amount to offset by</param>
        /// <returns></returns>
        public static T Bottom<T>(this T el, float offset) where T : VisualElement
        {
            el.style.bottom = offset;
            return el;
        }

        /// <summary>
        /// Sets the VisualElement's flex properties
        /// </summary>
        /// <example>
        /// <code>
        /// HStack(
        ///   TextField("I will grow").Flex(1),
        ///   Button("I will not", () => {})
        /// );
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="size">Flex size to use for ratio-based scaling. See Unity docs for more details</param>
        /// <returns></returns>
        public static T Flex<T>(this T el, float size) where T : VisualElement
        {
            el.style.flexBasis = new StyleLength(new Length(0, LengthUnit.Percent));
            el.style.flexShrink = 1;
            el.style.flexGrow = size;
            return el;
        }

        /// <summary>
        /// Enables word-wrapping on the VisualElement
        /// </summary>
        /// <example>
        /// <code>
        /// Label("This text will wrap around if it's too long").Wrapped();
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <returns></returns>
        public static T Wrapped<T>(this T el) where T : VisualElement
        {
            el.style.whiteSpace = WhiteSpace.Normal;
            return el;
        }

        /// <summary>
        /// Makes the VisualElement's text bold
        /// </summary>
        /// <example>
        /// <code>
        /// Label("I'm bold!").Bold();
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Makes the VisualElement's text italicized
        /// </summary>
        /// <example>
        /// <code>
        /// Label("I'm italic!").Italic();
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Makes the VisualElement's text bold and italicized
        /// </summary>
        /// <example>
        /// <code>
        /// Label("I'm bold and italic!").BoldItalic();
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <returns></returns>
        public static T BoldItalic<T>(this T el) where T : VisualElement
        {
            el.style.unityFontStyleAndWeight = FontStyle.BoldAndItalic;
            return el;
        }

        /// <summary>
        /// Sets the VisualElement's text color
        /// </summary>
        /// <example>
        /// <code>
        /// Label("I'm red!").Color(Color.red);
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static T Color<T>(this T el, Color color) where T : VisualElement
        {
            el.style.color = color;
            return el;
        }

        /// <summary>
        /// Sets the width of a VisualElement
        /// </summary>
        /// <example>
        /// <code>
        /// Label("I'm 100px wide").Width(100);
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static T Width<T>(this T el, float width) where T : VisualElement
        {
            el.style.width = width;
            return el;
        }

        /// <summary>
        /// Sets the VisualElemenet's SerializedObject binding path
        /// </summary>
        /// <example>
        /// <code>
        /// VStack(
        ///     TextField("Object's name").BindingPath("name");
        /// ).BindSerializedObject(new SerializedObject(myObject));
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="bindingPath">Variable's binding path. Check Unity docs for more information on IBindable fields</param>
        /// <returns></returns>
        public static T BindingPath<T>(this T el, string bindingPath) where T : IBindable
        {
            el.bindingPath = bindingPath;
            return el;
        }

        /// <summary>
        /// Binds a VisualElement to a SerializedObject
        /// </summary>
        /// <example>
        /// <code>
        /// VStack(
        ///    TextField("Object's name").BindingPath("name");
        /// ).BindSerializedObject(new SerializedObject(myObject));
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="serializedObject">SerializedObject to bind to</param>
        /// <returns></returns>
        public static T BindSerializedObject<T>(this T el, SerializedObject serializedObject) where T : VisualElement
        {
            el.Bind(serializedObject);
            return el;
        }

        /// <summary>
        /// Aligns the children of a VisualElement
        /// </summary>
        /// <example>
        /// <code>
        /// VStack(
        ///   Label("I'm centered"),
        ///   Label("I'm also centered")
        /// ).AlignItems(Align.Center);
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="align">Align mode. Check Unity docs for more information</param>
        /// <returns></returns>
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

        /// <summary>
        /// Justifies the content of a VisualElement
        /// </summary>
        /// <example>
        /// <code>
        /// VStack(
        ///     Label("I'm all the way to the left"),
        ///     Label("I'm all the way to the right")
        /// ).JustifyContent(Justify.SpaceBetween);
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="justify">Justify Content mode. Check Unity docs for more information</param>
        /// <returns></returns>
        public static T JustifyContent<T>(this T el, Justify justify) where T : VisualElement
        {
            el.style.justifyContent = justify;
            return el;
        }

        /// <summary>
        /// Controls the direction of VisualElement's child alignment
        /// </summary>
        /// <example>
        /// <code>
        /// VisualElement(
        ///    Label("I'm first"),
        ///    Label("I'm second")
        /// ).Direction(FlexDirection.Row);
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="direction">Flex direction for layout</param>
        /// <returns></returns>
        public static T Direction<T>(this T el, FlexDirection direction) where T : VisualElement
        {
            el.style.flexDirection = direction;
            return el;
        }

        /// <summary>
        /// Conditionally shows a VisualElement based on a provided condition function
        /// </summary>
        /// <example>
        /// <code>
        /// var showHelp = false;
        /// VStack(
        ///     Label("Help info here").ShowIf(() => showHelp),
        ///     Button("Toggle Help", () => showHelp = !showHelp)
        /// );
        /// </code>
        /// </example>
        /// <list>
        /// <item>This spools up a timer that evaluates the condition function every 300ms. Use `ReactiveProperty` for more efficient event-based updates</item>
        /// </list>
        /// <param name="el"></param>
        /// <param name="condition">`Func<bool>` that returns the visibility state</param>
        /// <returns></returns>
        public static T ShowIf<T>(this T el, Func<bool> condition) where T : VisualElement
        {
            el.style.display = condition() ? DisplayStyle.Flex : DisplayStyle.None;
            el.schedule.Execute(() => el.style.display = condition() ? DisplayStyle.Flex : DisplayStyle.None).Every(300);
            return el;
        }

        /// <summary>
        /// Conditionally hides a VisualElement based on a provided condition function
        /// </summary>
        /// <example>
        /// <code>
        /// var simpleMode = false;
        /// VStack(
        ///    Label("Advanced information").HideIf(() => simpleMode),
        ///    Button("Simple Mode", () => simpleMode = !simpleMode)
        /// );
        /// </code>
        /// </example>
        /// <list>
        /// <item>This spools up a timer that evaluates the condition function every 300ms. Use `ReactiveProperty` for more efficient event-based updates</item>
        /// </list>
        /// <param name="el"></param>
        /// <param name="condition">`Func<bool>` that returns the hidden state</param>
        /// <returns></returns>
        public static T HideIf<T>(this T el, Func<bool> condition) where T : VisualElement
        {
            el.style.display = condition() ? DisplayStyle.None : DisplayStyle.Flex;
            el.schedule.Execute(() => el.style.display = condition() ? DisplayStyle.None : DisplayStyle.Flex).Every(300);
            return el;
        }

        /// <summary>
        /// Binds visbility state of a VisualElement
        /// </summary>
        /// <example>
        /// <code>
        /// var isVisible = new ReactiveProperty<bool>(true);
        /// VStack(
        ///     Label("Toggle Me").BindVisibleState(isVisible),
        ///     Button("Toggle visibility", () => isVisible.Set(!isVisible.Value))
        /// );
        /// </code>
        /// </example>
        /// <list>
        /// <item>Compared to `HideIf` and `ShowIf` this only updates the visibility state when the value changes</item>
        /// </list>
        /// <param name="el"></param>
        /// <param name="prop">`ReactiveProperty<bool>` to bind to</param>
        /// <returns></returns>
        public static T BindVisibleState<T>(this T el, ReactiveProperty<bool> prop) where T : VisualElement
        {
            el.style.display = prop.Value ? DisplayStyle.Flex : DisplayStyle.None;
            prop.OnValueChanged += value => el.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            return el;
        }

        /// <summary>
        /// Calls a provided callback on mount
        /// </summary>
        /// <example>
        /// <code>
        /// VisualElement().OnMount(label => Debug.Log("Label mounted!"));
        /// </code>
        /// </example>
        /// <list>
        /// <item>This is internally called on `AttachToPanelEvent`</item>
        /// </list>
        /// <param name="el"></param>
        /// <param name="onMount">An `Action<T>` function that receives the element reference on mount</param>
        /// <returns></returns>
        public static T OnMount<T>(this T el, Action<T> onMount) where T : VisualElement
        {
            el.RegisterCallback<AttachToPanelEvent>(e => onMount(el));
            return el;
        }

        /// <summary>
        /// Calls a provided callback on unmount
        /// </summary>
        /// <example>
        /// <code>
        /// VisualElement().OnUnmount(label => Debug.Log("Label unmounted!"));
        /// </code>
        /// </example>
        /// <list>
        /// <item>This is internally called on `DetachFromPanelEvent`</item>
        /// </list>
        /// <param name="el"></param>
        /// <param name="onUnmount">An `Action<T>` function that receives the element reference on unmount</param>
        /// <returns></returns>
        public static T OnUnmount<T>(this T el, Action<T> onUnmount) where T : VisualElement
        {
            el.RegisterCallback<DetachFromPanelEvent>(e => onUnmount(el));
            return el;
        }
    }
}