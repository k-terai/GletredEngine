// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using GletredEdShare.WindowModule;

namespace GletredWpfEditor.Utilities.File
{
    public interface ISelectExternalFileWindow : IWindow<WindowViewModel>
    {
        /// <summary>
        /// Show window.
        /// </summary>
        /// <param name="description">Window description.</param>
        /// <param name="filter">Select filter.</param>
        /// <param name="multiSelect">True = Multi file select enable.</param>
        /// <returns>Select file list.</returns>
        List<string>? ShowWindow(string description, string filter, bool multiSelect);
    }
}
