// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using GletredEdShare.AssetModule.Contexts;
using GletredEdShare.CoreModule;
using GletredEdShare.ProjectModule;
using GletredEdShare.SerializationModule;

namespace GletredEdShare.AssetModule
{
    public abstract class Asset : NotifyPropertyChangedBase
    {
        /// <summary>
        /// NOTE: null is possible.(Ex: Root folder)
        /// </summary>
        private Asset? _parent;
        private string _name = null!;
        private Uri _defaultUri = null!;
        private bool _isDirty;

        protected event Action<Asset>? OnChildAssetInitialized;
        protected AssetMetaData? MetaData { get; set; }

        public static event Action<Asset>? OnBaseAssetInitialized;

        public Asset? Parent { get => _parent; protected set { _parent = value; NotifyPropertyChanged(); } }
        public string Name { get => _name; protected set { _name = value; NotifyPropertyChanged(); } }
        public Uri DefaultUri { get => _defaultUri; protected set { _defaultUri = value; NotifyPropertyChanged(); } }

        public Uri ThumbnailUri { get; protected set; } = null!;

        public string Extension { get; protected set; } = null!;
        public string NameWithExtension => Name + Extension;
        public Guid Id
        {
            get
            {
                Debug.Assert(MetaData != null, nameof(MetaData) + " != null");
                return MetaData.Id;
            }
        }

        public string ConvertType
        {
            get
            {
                Debug.Assert(MetaData != null, nameof(MetaData) + " != null");
                return MetaData.ConvertType;
            }
        }

        public uint Version
        {
            get
            {
                Debug.Assert(MetaData != null, nameof(MetaData) + " != null");
                return MetaData.Version;
            }
        }

        /// <summary>
        /// Absolute path (Ex. C:...)
        /// </summary>
        public virtual string FullPath
        {
            get
            {
                Debug.Assert(_parent != null, nameof(_parent) + " != null");
                return Path.Combine(_parent.FullPath, NameWithExtension);
            }
        }

        /// <summary>
        /// Relative path from project folder.(Ex. Assets/Textures/SampleTexture.png)
        /// </summary>
        public virtual string RelativePath
        {
            get
            {
                Debug.Assert(_parent != null, nameof(_parent) + " != null");
                return Path.Combine(_parent.RelativePath, NameWithExtension);
            }
        }

        public string MetaDataFullPath => FullPath + EditorConst.AssetMetadataExtension;

        public string RuntimeDirPath
        {
            get
            {
                Debug.Assert(Project.Current != null, "Project.Current != null");
                return Path.Combine(Project.Current.RuntimeDirectory.FullName, Id.ToString().Substring(0, 2));
            }
        }

        public bool IsDirty { get => _isDirty; set { _isDirty = value; NotifyPropertyChanged(); } }

        public virtual void Initialize(AssetContext context)
        {
            Parent = context.Parent;
            Name = context.Name;
            Extension = context.Extension;
            ThumbnailUri = DefaultUri = context.DefaultUri;

            if (!context.IsMetaDataExists)
            {
                CreateMetaData(context);
            }
            else
            {
                MetaData = Serializer.Deserialize<AssetMetaData>(MetaDataFullPath);
            }

            Parent?.OnChildAssetInitialized?.Invoke(this);

            CreateThumbnail(true);

            IsDirty = false;
            OnBaseAssetInitialized?.Invoke(this);
        }

        public virtual bool Save()
        {
            if (!IsDirty)
            {
                return true;
            }

            IsDirty = false;

            Debug.Assert(MetaData != null, nameof(MetaData) + " != null");
            return Serializer.Serialize(MetaData, MetaDataFullPath);

        }

        public virtual bool Rename(string name)
        {
            try
            {
                var newName = name;
                Debug.Assert(Parent != null, nameof(Parent) + " != null");

                var newFilePath = Path.Combine(Parent.FullPath, newName + Extension);
                var newMetaPath = Path.Combine(Parent.FullPath, newName + Extension + EditorConst.AssetMetadataExtension);

                File.Move(FullPath, newFilePath);
                File.Move(MetaDataFullPath, newMetaPath);

                Name = name;
                return true;

            }
            catch (IOException exception)
            {
                LogModule.Debug.LogException(exception);
                return false;
            }
        }

        public virtual void CreateThumbnail(bool isForceUpdate)
        {
            ThumbnailUri = DefaultUri;
        }

        protected virtual bool CreateMetaData(AssetContext context)
        {
            MetaData ??= new AssetMetaData();

            MetaData.Version = 1;
            MetaData.ConvertType = context.ConvertType;
            MetaData.Id = Guid.NewGuid();

            return Serializer.Serialize(MetaData, MetaDataFullPath);
        }

        protected void CreateRuntimeDirectoryIfNotExists()
        {
            var path = RuntimeDirPath;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
