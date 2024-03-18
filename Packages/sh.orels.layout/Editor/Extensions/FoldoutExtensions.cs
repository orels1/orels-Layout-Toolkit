using UnityEngine.UIElements;

namespace ORL.Layout.Extensions
{
    public static partial class FoldoutExtensions
    {
        public static Foldout Text(this Foldout el, string text)
        {
            el.text = text;
            return el;
        }
    }
}