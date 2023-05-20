// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using GletredEdShare.WindowModule;

namespace GletredWpfEditor.Utilities.Explore
{
    public interface IExplorerWindow : IWindow<WindowViewModel>
    {
        /// <summary>
        /// Show explorer window.
        /// </summary>
        /// <param name="path">File or directory absolute path.</param>
        void ShowWindow(string path);
    }
}
