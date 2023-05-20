// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using AvalonDock.Layout;

namespace GletredWpfEditor
{
    class LayoutInitializer : ILayoutUpdateStrategy
    {
        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            var destPane = destinationContainer as LayoutAnchorablePane;

            if (destinationContainer != null &&
                destinationContainer.FindParent<LayoutFloatingWindow>() != null)
            {
                return false;
            }

            //var toolsPane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(d => d.Name == "LeftPane");
            //if (toolsPane != null)
            //{
            //    toolsPane.Children.Add(anchorableToShow);
            //    return true;
            //}

            return false;
        }


        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {
        }


        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
        {
            return false;
        }

        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
        {

        }
    }
}