// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using GletredEdShare.AssetModule;
using GletredEdShare.AssetModule.Contexts;
using GletredEdShare.CoreModule;

namespace GletredEdShare.AssetModule
{
    public class NormalFolder : Asset
    {
        private ObservableCollection<Asset> _child = new();

        public ObservableCollection<Asset> Child { get => _child; protected set { _child = value; NotifyPropertyChanged(); } }

        public NormalFolder ParentFolder => Parent as NormalFolder ?? throw new InvalidOperationException();


        public override void Initialize(AssetContext context)
        {
            base.Initialize(context);

            OnChildAssetInitialized += ca =>
            {
                Child.Add(ca);
            };
        }

        public DirectoryInfo GetDirectoryInfo()
        {
            return new DirectoryInfo(FullPath);
        }

        public override bool Rename(string name)
        {
            try
            {
                var newName = name;
                Debug.Assert(Parent != null, nameof(Parent) + " != null");
                var newDirectoryPath = Path.Combine(Parent.FullPath, newName + Extension);
                var newMetaPath = Path.Combine(Parent.FullPath, newName + Extension + EditorConst.AssetMetadataExtension);

                Directory.Move(FullPath, newDirectoryPath);
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
    }
}
