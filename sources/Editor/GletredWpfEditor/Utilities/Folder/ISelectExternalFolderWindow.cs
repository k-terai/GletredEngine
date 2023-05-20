// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using GletredEdShare.WindowModule;


namespace GletredWpfEditor.Utilities.Folder
{
    public interface ISelectExternalFolderWindow : IWindow<WindowViewModel>
    {
        /// <summary>
        /// Show window.
        /// </summary>
        /// <param name="description">Window title.</param>
        /// <returns>Select folder path.</returns>
        string ShowWindow(string description);

        /// <summary>
        /// Show window.
        /// </summary>
        /// <param name="description">Window title.</param>
        /// <param name="selectedPath">Specify the first folder to select.</param>
        /// <param name="showNewFolderButton">True = allow user to create new folder</param>
        /// <returns>Select folder path.</returns>
        string ShowWindow(string description, string selectedPath, bool showNewFolderButton);
    }
}
