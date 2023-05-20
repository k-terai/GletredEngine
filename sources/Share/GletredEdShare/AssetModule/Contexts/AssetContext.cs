// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;
using GletredEdShare.AssetModule;
using GletredEdShare.CoreModule;

namespace GletredEdShare.AssetModule.Contexts
{
    public class AssetContext
    {
        public Asset? Parent { get; private set; }
        public string Name { get; private set; } = null!;
        public string FullPath { get; private set; } = null!;
        public string Extension { get; private set; } = string.Empty;
        public string ConvertType { get; private set; } = string.Empty;
        public bool IsMetaDataExists { get; set; }

        public bool IsFolderAsset { get; set; }

        public Uri DefaultUri { get; private set; } = null!;

        public AssetContext(string fullPath, Asset parent)
        {
            Parent = parent;

            if (Directory.Exists(fullPath))
            {
                Initialize(new DirectoryInfo(fullPath), parent);
            }
            else if (File.Exists(fullPath))
            {
                Initialize(new FileInfo(fullPath), parent);
            }

            IsMetaDataExists = File.Exists(FullPath + EditorConst.AssetMetadataExtension);
        }

        public AssetContext(FileSystemInfo info, Asset? parent)
        {
            Initialize(info, parent);
        }

        private void Initialize(FileSystemInfo info, Asset? parent)
        {
            Parent = parent;
            Name = info.Name;
            FullPath = info.FullName;
            Extension = string.Empty;
            ConvertType = string.Empty;
            IsFolderAsset = true;
        }

        private void Initialize(FileInfo info, Asset? parent)
        {

            Parent = parent;
            Name = Path.GetFileNameWithoutExtension(info.Name);
            FullPath = info.FullName;
            Extension = info.Extension;
            ConvertType = string.Empty;
            IsFolderAsset = false;
            DefaultUri = AssetDatabase.GetIconUri<Texture>();
            ConvertType = ".dds";
        }
    }
}
