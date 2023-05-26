// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Windows.Controls;

namespace GletredWpfEditor.Preview
{
    /// <summary>
    /// ViewportControl.xaml の相互作用ロジック
    /// </summary>
    public partial class PreviewControl : IPreviewControl
    {
        public PreviewControlViewModel ViewModel => DataContext as PreviewControlViewModel ?? throw new InvalidOperationException();
        public UserControl Control => this;

        public PreviewControl()
        {
            InitializeComponent();
        }


    }
}
