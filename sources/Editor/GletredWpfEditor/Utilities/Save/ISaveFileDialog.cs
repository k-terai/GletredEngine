// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace GletredWpfEditor.Utilities.Save
{
    public interface ISaveFileDialog
    {
        /// <summary>
        /// Show dialog.
        /// </summary>
        /// <param name="title">Dialog title.</param>
        /// <param name="filter">Save file filter.</param>
        /// <returns>Save file path.</returns>
        string ShowFileDialog(string title, string filter);
    }
}
