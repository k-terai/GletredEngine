// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Globalization;
using System.IO;
using GletredEdShare.CoreModule;

namespace GletredWpfEditor
{
    public class ResourceService : NotifyPropertyChangedBase
    {
        public static ResourceService Current { get; } = new();

        public Resources Resources { get; } = new();

        public void ChangeCulture(string name)
        {
            Resources.Culture = CultureInfo.GetCultureInfo(name);
            CultureInfo.CurrentCulture = Resources.Culture;
            NotifyPropertyChanged(nameof(Resources));
        }

        public Uri GetFluentIconUri(string fileName)
        {
            return new Uri(Path.Combine(@"/GletredWpfEditor;component/ThirdParty/fluentui-system-icons/", fileName), UriKind.RelativeOrAbsolute);
        }
    }
}
