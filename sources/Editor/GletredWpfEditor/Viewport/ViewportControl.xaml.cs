// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Windows.Controls;

namespace GletredWpfEditor.Viewport
{
    /// <summary>
    /// ViewportControl.xaml の相互作用ロジック
    /// </summary>
    public partial class ViewportControl : IViewportControl
    {
        public IntPtr WindowHandle => MainForm.Handle;
        public ViewportControlViewModel ViewModel => DataContext as ViewportControlViewModel ?? throw new InvalidOperationException();
        public UserControl Control => this;

        public ViewportControl()
        {
            InitializeComponent();
        }


    }
}
