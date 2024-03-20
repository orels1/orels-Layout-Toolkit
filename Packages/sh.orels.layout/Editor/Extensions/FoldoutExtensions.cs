using UnityEngine.UIElements;

namespace ORL.Layout.Extensions
{
    public static partial class FoldoutExtensions
    {
        /// <summary>
        /// Sets the text of the foldout
        /// </summary>
        /// <example>
        /// <code>
        /// Foldout().Text("Foldout text");
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Foldout Text(this Foldout el, string text)
        {
            el.text = text;
            return el;
        }
    }
}