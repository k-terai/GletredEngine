// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.IO;

namespace GletredWpfEditor.Utilities.Save
{
    public sealed class SaveFileDialog : ISaveFileDialog
    {
        /// <summary>
        /// Absolute path to the most recently opened folder.
        /// </summary>
        private static string _recentOpenDirectory = null!;

        /// <summary>
        /// Show dialog.
        /// </summary>
        /// <param name="title">Dialog title.</param>
        /// <param name="filter">Save file filter.</param>
        /// <returns>Save file path.</returns>
        public string ShowFileDialog(string title, string filter)
        {
            var sfd = new System.Windows.Forms.SaveFileDialog();

            sfd.InitialDirectory = Directory.Exists(_recentOpenDirectory) ? _recentOpenDirectory : @"C:\";
            sfd.Filter = filter;
            sfd.Title = title;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _recentOpenDirectory = new FileInfo(sfd.FileName).DirectoryName!;
                return sfd.FileName;
            }
            else
            {
                return null!;
            }
        }
    }
}
