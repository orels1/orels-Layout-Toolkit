using System;
using System.Collections.Generic;
using ORL.Layout.Extensions;
using UnityEngine.UIElements;

namespace ORL.Layout.Elements
{
    public class ForEach<T> : VisualElement
    {
        private List<VisualElement> _elements = new List<VisualElement>();
        private Func<T, VisualElement> _templateFunc;
        private Func<T, int, VisualElement> _templateFuncWithIndex;
        private bool _attached;

        public ForEach(IEnumerable<T> items, Func<T, VisualElement> template)
        {
            _templateFunc = template;
            UpdateElements(items);
        }

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

        public ForEach<T> BoundToProp(ReactiveProperty<IEnumerable<T>> prop)
        {
            prop.OnValueChanged += s => UpdateElements(s);
            return this;
        }

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