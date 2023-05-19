// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using GletredEdShare.CoreModule;
using GletredEdShare.LocalizationModule;
using GletredEdShare.SerializationModule;


namespace GletredEdShare.ProjectModule
{
    public sealed class Project : NotifyPropertyChangedBase
    {
        private readonly ProjectData _data;

        private readonly DirectoryInfo _rootDirectory = null!;
        private DirectoryInfo _assetsDirectory = null!;
        private DirectoryInfo _runtimeDirectory = null!;
        private DirectoryInfo _pluginsDirectory = null!;
        private DirectoryInfo _projectSettingsDirectory = null!;
        private DirectoryInfo _userSettingsDirectory = null!;

        public static Project? Current { get; private set; }

        public DirectoryInfo RootDirectory { get => _rootDirectory; private init { _rootDirectory = value; NotifyPropertyChanged(); } }
        public DirectoryInfo AssetsDirectory
        {
            get
            {
                Debug.Assert(RootDirectory != null);
                var path = Path.Combine(_rootDirectory.FullName, EditorConst.AssetsDirectoryName);
                _assetsDirectory = !Directory.Exists(path) ? Directory.CreateDirectory(path) : new DirectoryInfo(path);

                return _assetsDirectory;
            }
        }
        public DirectoryInfo RuntimeDirectory
        {
            get
            {
                Debug.Assert(RootDirectory != null);
                var path = Path.Combine(_rootDirectory.FullName, EditorConst.RuntimeDirectoryName);
                _runtimeDirectory = !Directory.Exists(path) ? Directory.CreateDirectory(path) : new DirectoryInfo(path);

                return _runtimeDirectory;
            }
        }
        public DirectoryInfo PluginsDirectory
        {
            get
            {
                Debug.Assert(RootDirectory != null);
                var path = Path.Combine(_rootDirectory.FullName, EditorConst.PluginDirectoryName);
                _pluginsDirectory = !Directory.Exists(path) ? Directory.CreateDirectory(path) : new DirectoryInfo(path);

                return _pluginsDirectory;
            }
        }
        public DirectoryInfo ProjectSettingsDirectory
        {
            get
            {
                Debug.Assert(RootDirectory != null);
                var path = Path.Combine(_rootDirectory.FullName, EditorConst.ProjectSettingsDirectoryName);
                _projectSettingsDirectory = !Directory.Exists(path) ? Directory.CreateDirectory(path) : new DirectoryInfo(path);

                return _projectSettingsDirectory;
            }
        }
        public DirectoryInfo UserSettingsDirectory
        {
            get
            {
                Debug.Assert(RootDirectory != null);
                var path = Path.Combine(_rootDirectory.FullName, EditorConst.UserSettingsDirectoryName);
                _userSettingsDirectory = !Directory.Exists(path) ? Directory.CreateDirectory(path) : new DirectoryInfo(path);

                return _userSettingsDirectory;
            }
        }
        public string Name
        {
            get
            {
                Debug.Assert(_rootDirectory != null);
                return _rootDirectory.Name;
            }
        }

        public string DataPath
        {
            get
            {
                Debug.Assert(RootDirectory != null);
                return Path.Combine(RootDirectory.FullName, Name + EditorConst.ProjectDataExtension);
            }
        }

        public Guid Id => _data.Id;

        public bool Save()
        {
            return Serializer.Serialize(_data, DataPath);
        }

        public static Project Create(string name, string parentFolderPath)
        {
            try
            {
                var p = new Project(Directory.CreateDirectory(Path.Combine(parentFolderPath, name)));
                p.CreateProjectDirectoriesIfNotExists();
                p.Save();

                Current ??= p;
                return p;

            }
            catch (Exception)
            {
                return null!;
            }
        }

        public static Project Load(string dataPath)
        {
            var directoryInfo = new FileInfo(dataPath).Directory;
            Debug.Assert(directoryInfo != null);

            var p = new Project(directoryInfo, JsonSerializer.FromJson<ProjectData>(dataPath));
            Current ??= p;
            return p;
        }

        public static EditorCommon.Result IsValidName(string name, string location)
        {
            switch (name.Length)
            {
                case 0:
                    return EditorCommon.Result.ErrorAssetNameMin;
                case > EditorConst.MaxProjectNameLength:
                    return EditorCommon.Result.ErrorProjectNameMax;
            }

            var invalidChars = Path.GetInvalidFileNameChars();
            var invalidPathChars = Path.GetInvalidPathChars();

            if ((name.IndexOfAny(invalidChars) > 0 || name.IndexOfAny(invalidPathChars) > 0 || name.Contains($" ") || name.Contains($"　")))
            {
                return EditorCommon.Result.ErrorProjectNameInvalid;
            }

            if (LocalizationManager.ShiftJisEncoding.GetByteCount(name) != name.Length)
            {
                return EditorCommon.Result.ErrorProjectNameDoubleByte;
            }

            var projectPath = Path.Combine(location, name);
            return Directory.Exists(projectPath) ? EditorCommon.Result.ErrorProjectPathNotExists : EditorCommon.Result.Ok;
        }

        public static EditorCommon.Result IsValidLocation(string location)
        {
            return !Directory.Exists(location) ? EditorCommon.Result.ErrorProjectPathNotExists : EditorCommon.Result.Ok;
        }

        private Project(DirectoryInfo root)
        {
            RootDirectory = root;
            CreateProjectDirectoriesIfNotExists();
            _data = new ProjectData
            {
                Id = Guid.NewGuid(),
                Version = 1,
                EngineVersion = EditorConst.EngineVersion
            };
        }

        private Project(DirectoryInfo root, ProjectData data)
        {
            RootDirectory = root;
            _data = data;
            CreateProjectDirectoriesIfNotExists();
        }

        private void CreateProjectDirectoriesIfNotExists()
        {
            //Access property,create folder.
            var d = AssetsDirectory;
            Debug.Assert(d != null);
            d = RuntimeDirectory;
            Debug.Assert(d != null);
            d = PluginsDirectory;
            Debug.Assert(d != null);
            d = ProjectSettingsDirectory;
            Debug.Assert(d != null);
            d = UserSettingsDirectory;
            Debug.Assert(d != null);
        }


    }
}
