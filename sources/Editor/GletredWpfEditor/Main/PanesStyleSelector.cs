// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Windows.Controls;
using System.Windows;
using GletredEdShare.WindowModule;

namespace GletredWpfEditor.Main
{
    public class PanesStyleSelector : StyleSelector
    {
        public Style? AnchorableStyle
        {
            get;
            set;
        }

        public Style? DocumentStyle
        {
            get;
            set;
        }

        public override Style? SelectStyle(object item, DependencyObject container)
        {
            if (item is not DockingWindowViewModel docking)
            {
                return base.SelectStyle(item, container);
            }

            return docking.Type switch
            {
                DockingWindowViewModel.DockingType.Anchorable => AnchorableStyle,
                DockingWindowViewModel.DockingType.Document => DocumentStyle,
                _ => base.SelectStyle(item, container)
            };
        }
    }
}
