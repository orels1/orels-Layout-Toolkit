using ORL.Layout.Extensions;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace ORL.Layout.Elements
{
    public class VStack : VisualElement
    {
        public VStack()
        {
            this.Direction(FlexDirection.Column);
        }

        public VStack(params VisualElement[] children) : this()
        {
            this.Children(children);
        }
    }
}