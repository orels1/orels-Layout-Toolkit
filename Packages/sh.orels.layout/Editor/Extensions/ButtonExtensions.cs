using System;
using UnityEngine.UIElements;

namespace ORL.Layout.Extensions
{
    public static partial class ButtonExtensions
    {
        public static Button OnClick(this Button el, Action onClick)
        {
            el.clicked += onClick;
            return el;
        }
    }
}