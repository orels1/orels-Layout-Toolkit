using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ORL.Layout
{
    public class ReactiveProperty<T>
    {
        public T Value { get; private set; }
        public event Action<T> OnValueChanged;

        public ReactiveProperty<T> Set(T value)
        {
            if (_targetObject != null)
            {
                Undo.RecordObject(_targetObject, $"Updated Property Value");
            }
            OnValueChanged?.Invoke(value);
            Value = value;
            return this;
        }

        public ReactiveProperty()
        {
            Value = default;
        }

        public ReactiveProperty(T value = default)
        {
            Value = value;
        }

        public static implicit operator T(ReactiveProperty<T> property)
        {
            return property.Value;
        }

        public ReactiveProperty<T> OnChanged(Action<T> action)
        {
            OnValueChanged += action;
            return this;
        }

        private UnityEngine.Object _targetObject;

        public ReactiveProperty<T> BindToObject(UnityEngine.Object targetObject)
        {
            _targetObject = targetObject;
            return this;
        }

        private void SaveToProperty(SerializedProperty property, object value)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    property.intValue = Convert.ToInt32(value);
                    break;
                case SerializedPropertyType.Boolean:
                    property.boolValue = Convert.ToBoolean(value);
                    break;
                case SerializedPropertyType.Float:
                    property.floatValue = Convert.ToSingle(value);
                    break;
                case SerializedPropertyType.String:
                    property.stringValue = Convert.ToString(value);
                    break;
                case SerializedPropertyType.Enum:
                    property.enumValueIndex = Convert.ToInt32(value);
                    break;
                case SerializedPropertyType.ObjectReference:
                    property.objectReferenceValue = value as UnityEngine.Object;
                    break;
                case SerializedPropertyType.LayerMask:
                    if (value is LayerMask mask)
                    {
                        property.intValue = mask.value;
                    }
                    break;
                case SerializedPropertyType.Vector2:
                    if (value is Vector2 vector2)
                    {
                        property.vector2Value = vector2;
                    }
                    break;
                case SerializedPropertyType.Vector3:
                    if (value is Vector3 vector3)
                    {
                        property.vector3Value = vector3;
                    }
                    break;
                case SerializedPropertyType.Vector4:
                    if (value is Vector4 vector4)
                    {
                        property.vector4Value = vector4;
                    }
                    break;
                case SerializedPropertyType.Rect:
                    if (value is Rect rect)
                    {
                        property.rectValue = rect;
                    }
                    break;
                case SerializedPropertyType.ArraySize:
                    property.arraySize = Convert.ToInt32(value);
                    break;
                case SerializedPropertyType.Character:
                    if (value is char character)
                    {
                        property.intValue = character;
                    }
                    break;
                case SerializedPropertyType.AnimationCurve:
                    if (value is AnimationCurve curve)
                    {
                        property.animationCurveValue = curve;
                    }
                    break;
                case SerializedPropertyType.Bounds:
                    if (value is Bounds bounds)
                    {
                        property.boundsValue = bounds;
                    }
                    break;
                case SerializedPropertyType.Quaternion:
                    if (value is Quaternion quaternion)
                    {
                        property.quaternionValue = quaternion;
                    }
                    break;
                case SerializedPropertyType.ExposedReference:
                    if (value is UnityEngine.Object obj)
                    {
                        property.exposedReferenceValue = obj;
                    }
                    break;
                case SerializedPropertyType.Color:
                    if (value is Color color)
                    {
                        property.colorValue = color;
                    }
                    break;
                case SerializedPropertyType.Vector2Int:
                    if (value is Vector2Int vector2Int)
                    {
                        property.vector2IntValue = vector2Int;
                    }
                    break;
                case SerializedPropertyType.Vector3Int:
                    if (value is Vector3Int vector3Int)
                    {
                        property.vector3IntValue = vector3Int;
                    }
                    break;
                case SerializedPropertyType.RectInt:
                    if (value is RectInt rectInt)
                    {
                        property.rectIntValue = rectInt;
                    }
                    break;
                case SerializedPropertyType.BoundsInt:
                    if (value is BoundsInt boundsInt)
                    {
                        property.boundsIntValue = boundsInt;
                    }
                    break;
                default:
                    Debug.LogWarning($"Property type {property.propertyType} is not supported by ReactiveProperty binding");
                    break;
            }
        }

        public ReactiveProperty<T> BindToProperty(SerializedProperty property)
        {
            OnValueChanged += value =>
            {
                var tType = typeof(T);
                var isArray = tType.IsArray || tType.IsGenericType && tType.GetGenericTypeDefinition() == typeof(List<>);
                if (isArray && property.isArray)
                {
                    var arrayElementType = tType.IsArray ? tType.GetElementType() : tType.GetGenericArguments()[0];
                    if (arrayElementType == null)
                    {
                        Debug.LogWarning($"Could not determine array element type for {tType}");
                        return;
                    }

                    if (arrayElementType.Name.ToLowerInvariant() != property.arrayElementType)
                    {
                        Debug.LogWarning($"Array element type mismatch: {arrayElementType.Name} != {property.arrayElementType}");
                        return;
                    }

                    if (tType.IsArray)
                    {
                        property.arraySize = (value as Array).Length;
                        for (int i = 0; i < (value as Array).Length; i++)
                        {
                            var el = property.GetArrayElementAtIndex(i);
                            SaveToProperty(el, (value as Array).GetValue(i));
                        }
                    }
                    else
                    {
                        property.arraySize = (value as System.Collections.IList).Count;
                        for (int i = 0; i < (value as System.Collections.IList).Count; i++)
                        {
                            var el = property.GetArrayElementAtIndex(i);
                            SaveToProperty(el, (value as System.Collections.IList)[i]);
                        }
                    }
                }
                else if (property.propertyType == SerializedPropertyType.Generic)
                {
                    Debug.LogWarning($"Property type {property.propertyType} is not supported by ReactiveProperty binding");
                }
                else
                {
                    SaveToProperty(property, value);
                }

                property.serializedObject.ApplyModifiedProperties();
            };
            return this;
        }
    }
}