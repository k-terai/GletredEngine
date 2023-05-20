// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.


using System;
using GletredEdShare.CoreModule;
using GletredEdShare.WindowModule;

namespace GletredWpfEditor.Utilities.Explore
{
    public sealed class ExplorerWindow : IExplorerWindow
    {
        public WindowViewModel ViewModel => throw new NotImplementedException();

        public void ShowWindow(string path)
        {
            System.Diagnostics.Process.Start(EditorConst.Explorer, path);
        }

        public void ShowWindow()
        {
            ShowWindow(Environment.CurrentDirectory);
        }
    }
}
