// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Diagnostics;
using System.IO;
using GletredEdShare.AssetModule.Contexts;
using GletredEdShare.CoreModule;
using GletredEdShare.ProjectModule;
using GletredEdShare.AssetModule;

namespace GletredEdShare.AssetModule
{
    public sealed class RootFolder : NormalFolder
    {
        private FileSystemWatcher _watcher = null!;

        public override string FullPath
        {
            get
            {
                Debug.Assert(Project.Current != null, "Project.Current != null");
                return Project.Current.AssetsDirectory.FullName;
            }
        }

        public override string RelativePath => EditorConst.AssetsDirectoryName;

        public static RootFolder CreateInstance()
        {
            Debug.Assert(Project.Current != null);

            var root = new RootFolder();
            root.Initialize(new AssetContext(Project.Current.AssetsDirectory, null));
            return root;
        }

        public override void Initialize(AssetContext context)
        {
            base.Initialize(context);

            _watcher = new FileSystemWatcher
            {
                Path = context.FullPath,
                Filter = "",
                NotifyFilter =
            NotifyFilters.FileName
            | NotifyFilters.DirectoryName
            | NotifyFilters.LastWrite,

                IncludeSubdirectories = true,
                SynchronizingObject = null,
                EnableRaisingEvents = true
            };

            _watcher.Changed += Watcher_Changed;
            _watcher.Created += Watcher_Created;
            _watcher.Deleted += Watcher_Deleted;
            _watcher.Renamed += Watcher_Renamed;

            DefaultUri = AssetDatabase.GetIconUri<RootFolder>();
        }

        /// <summary>
        /// The Content folder does not need to save metadata, so just create an instance (do not call base method because it will be saved if you call the base class)
        /// </summary>
        protected override bool CreateMetaData(AssetContext context)
        {
            Debug.Assert(Project.Current != null, "Project.Current != null");

            MetaData = new AssetMetaData()
            {
                Version = 1,
                ConvertType = string.Empty,
                Id = Project.Current.Id
            };

            return true;
        }

        private RootFolder()
        {

        }

        /// <summary>
        /// Occurs when a file or directory of the specified System.IO.FileSystemWatcher.Path is created.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void Watcher_Created(object source, FileSystemEventArgs e)
        {

        }

        /// <summary>
        /// Occurs when the specified System.IO.FileSystemWatcher.Path file or directory is deleted.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void Watcher_Deleted(object source, FileSystemEventArgs e)
        {

        }

        /// <summary>
        /// Occurs when the specified System.IO.FileSystemWatcher.Path file or directory is changed.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void Watcher_Changed(object source, FileSystemEventArgs e)
        {

        }

        /// <summary>
        /// Occurs when the name of the specified System.IO.FileSystemWatcher.Path file or directory is renamed.
        /// </summary>
        private void Watcher_Renamed(object source, RenamedEventArgs e)
        {

        }
    }
}
