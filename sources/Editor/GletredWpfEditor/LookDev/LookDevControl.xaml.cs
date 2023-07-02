// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Windows.Controls;
using GletredEdShare.RuntimeModule;

namespace GletredWpfEditor.LookDev
{
    /// <summary>
    /// LookDevControl.xaml の相互作用ロジック
    /// </summary>
    public partial class LookDevControl : ILookDevControl
    {

        public LookDevControlViewModel ViewModel => DataContext as LookDevControlViewModel ?? throw new InvalidOperationException();
        public UserControl Control => this;

        public LookDevControl()
        { 
            InitializeComponent();

            Runtime.EdEngine.OnEdEngineLaunched += () =>
            {
                Runtime.EdEngine.EdCreateScene(Viewport.WindowHandle);
            };

            Runtime.EdEngine.OnEdEngineTerminated += () =>
            {

            };

            

            if (Runtime.EdEngine.IsActive)
            {
                Runtime.EdEngine.EdCreateScene(Viewport.WindowHandle);
            }
            
        }

    }
}
