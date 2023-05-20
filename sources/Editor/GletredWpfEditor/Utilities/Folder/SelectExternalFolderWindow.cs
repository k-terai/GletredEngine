// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Windows.Forms;
using GletredEdShare.WindowModule;

namespace GletredWpfEditor.Utilities.Folder
{
    public sealed class SelectExternalFolderWindow : ISelectExternalFolderWindow
    {
        public WindowViewModel ViewModel => throw new NotImplementedException();

        /// <summary>
        /// Show window.
        /// </summary>
        /// <param name="description">Window title.</param>
        /// <returns>Select folder path.</returns>
        public string ShowWindow(string description)
        {
            FolderBrowserDialog fbd;

            using (fbd = new FolderBrowserDialog())
            {
                fbd.Description = description;
                var r = fbd.ShowDialog();

                return r == DialogResult.OK ? fbd.SelectedPath : string.Empty;
            }
        }

        /// <summary>
        /// Show window.
        /// </summary>
        /// <param name="description">Window title.</param>
        /// <param name="selectedPath">Specify the first folder to select.</param>
        /// <param name="showNewFolderButton">True = allow user to create new folder</param>
        /// <returns>Select folder path.</returns>
        public string ShowWindow(string description, string selectedPath, bool showNewFolderButton)
        {
            FolderBrowserDialog fbd;

            using (fbd = new FolderBrowserDialog())
            {
                fbd.Description = description;
                //fbd.RootFolder = System.Environment.SpecialFolder.CommonDocuments;
                fbd.SelectedPath = selectedPath;
                fbd.ShowNewFolderButton = showNewFolderButton;
                var r = fbd.ShowDialog();

                return r == DialogResult.OK ? fbd.SelectedPath : string.Empty;
            }
        }

        public void ShowWindow()
        {
            ShowWindow("default", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), false);
        }
    }
}
