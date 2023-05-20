// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Windows.Data;
using GletredEdShare.WindowModule;

namespace GletredWpfEditor.Converters
{
    public class ActiveContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is DockingWindowViewModel ? value : Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is DockingWindowViewModel ? value : Binding.DoNothing;
        }
    }
}
