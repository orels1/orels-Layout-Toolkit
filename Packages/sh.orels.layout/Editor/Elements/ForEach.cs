using System;
using System.Collections.Generic;
using ORL.Layout.Extensions;
using UnityEngine.UIElements;

namespace ORL.Layout.Elements
{
    /// <summary>
    /// Iterator element that creates a visual element for each item in the provided collection using provided template function
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public class ForEach<T> : VisualElement
    {
        private List<VisualElement> _elements = new List<VisualElement>();
        private Func<T, VisualElement> _templateFunc;
        private Func<T, int, VisualElement> _templateFuncWithIndex;
        private bool _attached;

        /// <summary>
        /// Creates a new instance of ForEach element
        /// </summary>
        /// <example>
        /// <code>
        /// var items = new List<string> { "Item 1", "Item 2", "Item 3" };
        /// return VStack(
        ///     ForEach(items, item => new Label(item))
        /// );
        /// </code>
        /// </example>
        /// <param name="items">An `IEnumerable<T>` of items to use</param>
        /// <param name="template">A `Func<T, VisualElement>` template function that renders an individual element</param>
        public ForEach(IEnumerable<T> items, Func<T, VisualElement> template)
        {
            _templateFunc = template;
            UpdateElements(items);
        }

        /// <summary>
        /// Creates a new instance of ForEach element passing element index to the template function
        /// </summary>
        /// <example>
        /// <code>
        /// var items = new List<string> { "Item 1", "Item 2", "Item 3" };
        /// return VStack(
        ///     ForEach(items, (item, index) => new Label($"[{index}] {item}"))
        /// );
        /// </code>
        /// </example>
        /// <param name="items">An `IEnumerable<T>` of items to use</param>
        /// <param name="template">A `Func<T, int, VisualElement>` template function that renders an individual element, has access to the element's index in the original collection</param>
        public ForEach(IEnumerable<T> items, Func<T, int, VisualElement> template)
        {
            _templateFuncWithIndex = template;
            UpdateElements(items);
        }

        private void InsertElements()
        {
            var insertAt = hierarchy.parent.hierarchy.IndexOf(this);
            foreach (var item in _elements)
            {
                hierarchy.parent.Insert(insertAt, item);
            }
        }

        private void UpdateElements(IEnumerable<T> items)
        {
            _elements.ForEach(e => e.RemoveFromHierarchy());
            _elements.Clear();

            if (_templateFunc != null)
            {
                foreach (var item in items)
                {
                    _elements.Add(_templateFunc(item));
                }
            }
            else
            {
                var i = 0;
                foreach (var item in items)
                {
                    _elements.Add(_templateFuncWithIndex(item, i));
                    i++;
                }
            }

            _elements.Reverse();

            if (!_attached)
            {
                RegisterCallback<AttachToPanelEvent>(_ =>
                {
                    _attached = true;
                    InsertElements();
                });
            }
            else
            {
                InsertElements();
            }
        }

        /// <summary>
        /// Binds the ForEach element to a reactive property of `IEnumerable<T>` type. This automatically re-renders the elements on property change
        /// </summary>
        /// <list>
        /// <item>The current implementation only re-renders when `.Set` is called on the reactive property</item>
        /// </list>
        /// <example>
        /// <code>
        /// var items = new ReactiveProperty<IEnumerable<string>>(new List<string> { "Item 1", "Item 2", "Item 3" });
        /// return VStack(
        ///    ForEach(items.Value, item => new Label(item)).BoundToProp(items),
        ///    Button("Add Item").OnClick(() => { items.Set(new List<string>(items.Value) { "New Item" }); })
        /// );
        /// </code>
        /// </example>
        /// <param name="prop">`ReactiveProperty<IEnumerable<T>>` to bind to</param>
        /// <returns></returns>
        public ForEach<T> BoundToProp(ReactiveProperty<IEnumerable<T>> prop)
        {
            prop.OnValueChanged += s => UpdateElements(s);
            return this;
        }

        /// <summary>
        /// Binds the ForEach element to a reactive property of `IEnumerable<T>` type with a custom transform function. This automatically re-renders the elements on property change
        /// </summary>
        /// <list>
        /// <item>The current implementation only re-renders when `.Set` is called on the reactive property</item>
        /// <item>The `transform` function can be used to prepare elements to be rendered by the `template` function</item>
        /// </list>
        /// <example>
        /// <code>
        /// var items = new ReactiveProperty<IEnumerable<string>>(new List<string> { "Item 1", "Item 2", "Item 3" });
        /// return VStack(
        ///    ForEach(items.Value, item => new Label(item)).BoundToProp(items, items => items.Reverse()),
        ///    Button("Add Item").OnClick(() => { items.Set(new List<string>(items.Value) { "New Item" }); })
        /// );
        /// </code>
        /// </example>
        /// <param name="prop">`ReactiveProperty<IEnumerable<T>>` to bind to</param>
        /// <param name="transform">A `Func<IEnumerable<T>, IEnumerable<T>>` transform function that prepares the elements to be rendered by the `template` function</param>
        /// <returns></returns>
        public ForEach<T> BoundToProp(ReactiveProperty<IEnumerable<T>> prop, Func<IEnumerable<T>, IEnumerable<T>> transform)
        {
            prop.OnValueChanged += s => UpdateElements(transform(s));
            return this;
        }

        public ForEach<T> BoundToProp(ReactiveProperty<List<T>> prop)
        {
            prop.OnValueChanged += s => UpdateElements(s);
            return this;
        }

        public ForEach<T> BoundToProp(ReactiveProperty<List<T>> prop, Func<IEnumerable<T>, List<T>> transform)
        {
            prop.OnValueChanged += s => UpdateElements(transform(s));
            return this;
        }
    }
}