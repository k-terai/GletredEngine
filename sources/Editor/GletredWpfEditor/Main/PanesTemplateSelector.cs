// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using GletredEdShare.WindowModule;

namespace GletredWpfEditor.Main
{
    public class PanesTemplateSelector : DataTemplateSelector
    {
        public ObservableCollection<DockingWindowViewModel> DockingWindows = new();


        public DataTemplate LogViewerPaneTemplate
        {
            get;
            set;
        } = null!;

        public DataTemplate AssetBrowserPaneTemplate
        {
            get;
            set;
        } = null!;

        public DataTemplate ViewportPaneTemplate
        {
            get;
            set;
        } = null!;

        public class AssetBrowserPaneViewModel : DockingWindowViewModel
        {
            public AssetBrowserPaneViewModel()
            {
                Title = Resources.AssetBrowser;
                Name = Resources.AssetBrowser;
                ContentId = string.Empty;
                ToolTip = Resources.AseetBrowserToolTip;
                IconSource = ResourceService.Current.GetFluentIconUri(Resources.Icon_AssetBrowser);
            }
        }

        public class LogViewerPaneViewModel : DockingWindowViewModel
        {
            public LogViewerPaneViewModel()
            {
                Title = Resources.LogViewer;
                Name = Resources.LogViewer;
                ContentId = string.Empty;
                ToolTip = Resources.LogViewerToolTip;
                IconSource = ResourceService.Current.GetFluentIconUri(Resources.Icon_LogViewer);
            }
        }

        public class ViewportPaneViewModel : DockingWindowViewModel
        {
            public ViewportPaneViewModel()
            {
                Title = Resources.Viewport;
                Name = Resources.Viewport;
                ContentId = string.Empty;
                ToolTip = Resources.ViewportToolTip;
                IconSource = ResourceService.Current.GetFluentIconUri(Resources.Icon_Viewport);
            }
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var content = item as ContentPresenter;
            Debug.Assert(content != null, nameof(content) + " != null");

            if (content.Content is not DockingWindowViewModel)
            {
                return base.SelectTemplate(content.Content, container) ?? throw new InvalidOperationException();
            }

            return content.Content switch
            {
                LogViewerPaneViewModel => LogViewerPaneTemplate,
                AssetBrowserPaneViewModel => AssetBrowserPaneTemplate,
                ViewportPaneViewModel => ViewportPaneTemplate,
                _ => base.SelectTemplate(content.Content, container) ?? throw new InvalidOperationException()
            };
        }
    }
}
