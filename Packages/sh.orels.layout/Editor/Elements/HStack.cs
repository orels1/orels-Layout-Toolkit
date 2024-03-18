using ORL.Layout.Extensions;
using UnityEngine.UIElements;

namespace ORL.Layout.Elements
{
    public class HStack : VisualElement
    {
        public HStack()
        {
            this.Direction(FlexDirection.Row);
        }

        public HStack(params VisualElement[] children) : this()
        {
            this.Children(children);
        }
    }
}