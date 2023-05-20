// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Windows.Controls;

namespace GletredWpfEditor.LogViewer
{
    /// <summary>
    /// LogViewerControl.xaml の相互作用ロジック
    /// </summary>
    public partial class LogViewerControl : ILogViewerControl
    {
        public LogViewerControlViewModel ViewModel => DataContext as LogViewerControlViewModel ?? throw new InvalidOperationException();


        public UserControl Control => this;

        public LogViewerControl()
        {
            InitializeComponent();
        }
    }
}
