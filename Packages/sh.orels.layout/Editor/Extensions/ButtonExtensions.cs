using System;
using UnityEngine.UIElements;

namespace ORL.Layout.Extensions
{
    public static partial class ButtonExtensions
    {
        /// <summary>
        /// Binds a click event to the button
        /// </summary>
        /// <example>
        /// <code>
        /// Button().Text("Click me").OnClick(() => Debug.Log("Clicked!"));
        /// </code>
        /// </example>
        /// <param name="el"></param>
        /// <param name="onClick">Click `Action`</param>
        /// <returns></returns>
        public static Button OnClick(this Button el, Action onClick)
        {
            el.clicked += onClick;
            return el;
        }
    }
}