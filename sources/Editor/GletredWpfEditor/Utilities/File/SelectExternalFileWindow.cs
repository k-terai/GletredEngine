// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GletredEdShare.WindowModule;

namespace GletredWpfEditor.Utilities.File
{
    public sealed class SelectExternalFileWindow : ISelectExternalFileWindow
    {
        /// <summary>
        /// Absolute path to the most recently opened folder.
        /// </summary>
        private static string _recentOpenDirectory = null!;

        public WindowViewModel ViewModel => throw new NotImplementedException();

        /// <summary>
        /// Show window.
        /// </summary>
        /// <param name="description">Window description.</param>
        /// <param name="filter">Select filter.</param>
        /// <param name="multiSelect">True = Multi file select enable.</param>
        /// <returns>Select file list.</returns>
        public List<string> ShowWindow(string description, string filter, bool multiSelect)
        {
            OpenFileDialog dialog;

            using (dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = Directory.Exists(_recentOpenDirectory) ? _recentOpenDirectory : Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                dialog.Multiselect = multiSelect;
                dialog.Title = description;
                dialog.Filter = filter;
                var result = dialog.ShowDialog();

                if (result != DialogResult.OK)
                {
                    return null!;
                }

                var list = dialog.FileNames.ToList();
                _recentOpenDirectory = new FileInfo(dialog.FileName).DirectoryName!;
                return list;
            }
        }

        public void ShowWindow()
        {
            ShowWindow("Default", "*", false);
        }
    }
}
