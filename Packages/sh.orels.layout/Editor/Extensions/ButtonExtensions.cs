using System;
using UnityEditor;
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

        /// <summary>
        /// Opens a popup window when the button is clicked
        /// </summary>
        /// <example>
        /// <code>
        /// Button().Text("Open Popup").OpenPopup(new TestPopup());
        /// </code>
        /// </example>
        /// <list>
        /// <item>You can use `EnhancedPopupWindow` to create popups with OTK as well</item>
        /// </list>
        /// <param name="el"></param>
        /// <param name="popup"></param>
        /// <returns></returns>
        public static Button OpenPopup(this Button el, PopupWindowContent popup)
        {
            el.clicked += () => UnityEditor.PopupWindow.Show(el.worldBound, popup);
            return el;
        }
    }
}