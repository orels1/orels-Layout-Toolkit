using System;
using System.Collections.Generic;
using ORL.Layout.Elements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ORL.Layout
{
    public partial class EnhancedEditor : Editor
    {
        private VisualElement _r;

        public override VisualElement CreateInspectorGUI()
        {
            _r = new VisualElement();
            var utilStyles = Resources.Load<StyleSheet>("ORLLayoutEditorUtilityStyles");
            var styles = Resources.Load<StyleSheet>("ORLLayoutEditorStyles");
            if (utilStyles != null) _r.styleSheets.Add(utilStyles);
            if (styles != null) _r.styleSheets.Add(styles);
            serializedObject.Update();
            _r.Add(Render());
            return _r;
        }

        protected virtual VisualElement Render()
        {
            return null;
        }

        protected void ReloadGUI()
        {
            _r.Clear();
            _r.Add(Render());
        }

        protected VStack VStack(params VisualElement[] children)
        {
            return new VStack(children);
        }

        protected HStack HStack(params VisualElement[] children)
        {
            return new HStack(children);
        }

        protected ForEach<T> ForEach<T>(IEnumerable<T> items, Func<T, int, VisualElement> template)
        {
            return new ForEach<T>(items, template);
        }

        protected ForEach<T> ForEach<T>(IEnumerable<T> items, Func<T, VisualElement> template)
        {
            return new ForEach<T>(items, template);
        }

        protected Label Label(string text)
        {
            return new Label(text);
        }

        protected Label Label()
        {
            return new Label();
        }

        protected Button Button()
        {
            return new Button();
        }

        protected Button Button(Action clickEvent)
        {
            return new Button(clickEvent);
        }

        protected Foldout Foldout()
        {
            return new Foldout();
        }

        protected ScrollView ScrollView()
        {
            return new ScrollView();
        }

        protected PropertyField PropertyField()
        {
            return new PropertyField();
        }

        protected PropertyField PropertyField(SerializedProperty prop)
        {
            return new PropertyField(prop);
        }

        protected PropertyField PropertyField(SerializedProperty prop, string label)
        {
            return new PropertyField(prop, label);
        }

        protected Toggle Toggle()
        {
            return new Toggle();
        }

        protected Toggle Toggle(string label)
        {
            return new Toggle(label);
        }

        protected IntegerField IntegerField()
        {
            return new IntegerField();
        }

        protected IntegerField IntegerField(string label)
        {
            return new IntegerField(label);
        }

        protected IntegerField IntegerField(string label, int defaultValue)
        {
            return new IntegerField(label, defaultValue);
        }

        protected FloatField FloatField()
        {
            return new FloatField();
        }

        protected FloatField FloatField(string label)
        {
            return new FloatField(label);
        }

        protected FloatField FloatField(int maxLimit)
        {
            return new FloatField(maxLimit);
        }

        protected FloatField FloatField(string label, int maxLimit)
        {
            return new FloatField(label, maxLimit);
        }

        protected TextField TextField()
        {
            return new TextField();
        }

        protected TextField TextField(string label)
        {
            return new TextField(label);
        }

        protected ObjectField ObjectField()
        {
            return new ObjectField();
        }

        protected ObjectField ObjectField(string label)
        {
            return new ObjectField(label);
        }

        protected EnumField EnumField()
        {
            return new EnumField();
        }

        protected EnumField EnumField(string label)
        {
            return new EnumField(label);
        }

        protected EnumField EnumField(string label, Enum defaultValue)
        {
            return new EnumField(label, defaultValue);
        }

        protected Vector2Field Vector2Field()
        {
            return new Vector2Field();
        }

        protected Vector2Field Vector2Field(string label)
        {
            return new Vector2Field(label);
        }

        protected Vector3Field Vector3Field()
        {
            return new Vector3Field();
        }

        protected Vector3Field Vector3Field(string label)
        {
            return new Vector3Field(label);
        }

        protected Vector4Field Vector4Field()
        {
            return new Vector4Field();
        }

        protected Vector4Field Vector4Field(string label)
        {
            return new Vector4Field(label);
        }

        protected ColorField ColorField()
        {
            return new ColorField();
        }

        protected ColorField ColorField(string label)
        {
            return new ColorField(label);
        }

        protected GradientField GradientField()
        {
            return new GradientField();
        }

        protected GradientField GradientField(string label)
        {
            return new GradientField(label);
        }

        protected BoundsField BoundsField()
        {
            return new BoundsField();
        }

        protected BoundsField BoundsField(string label)
        {
            return new BoundsField(label);
        }

        protected RectField RectField()
        {
            return new RectField();
        }

        protected RectField RectField(string label)
        {
            return new RectField(label);
        }

        protected MinMaxSlider MinMaxSlider()
        {
            return new MinMaxSlider();
        }

        protected MinMaxSlider MinMaxSlider(string label)
        {
            return new MinMaxSlider(label);
        }

        protected MinMaxSlider MinMaxSlider(string label, float min, float max)
        {
            return new MinMaxSlider(label, min, max);
        }

        protected MinMaxSlider MinMaxSlider(string label, float min, float max, float minLimit, float maxLimit)
        {
            return new MinMaxSlider(label, min, max, minLimit, maxLimit);
        }

        protected Vector2IntField Vector2IntField()
        {
            return new Vector2IntField();
        }

        protected Vector2IntField Vector2IntField(string label)
        {
            return new Vector2IntField(label);
        }

        protected Vector3IntField Vector3IntField()
        {
            return new Vector3IntField();
        }

        protected Vector3IntField Vector3IntField(string label)
        {
            return new Vector3IntField(label);
        }
    }
}