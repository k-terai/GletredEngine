// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GletredEdShare.AssetModule.Contexts;
using GletredEdShare.CoreModule;
using GletredEdShare.LocalizationModule;
using GletredEdShare.TreeModule.Assets;

namespace GletredEdShare.AssetModule
{

    public static class AssetDatabase
    {
        private static readonly Dictionary<string, Uri> AssetIconUriCache = new();
        private static Dictionary<Guid, Asset> _projectAssets = new();

        public static RootFolder Root { get; private set; } = null!;

        public static bool Startup()
        {
            _projectAssets = new Dictionary<Guid, Asset>();
            Asset.OnBaseAssetInitialized += OnBaseAssetInitialized;
            return true;
        }

        public static bool Initialize()
        {
            InitializeProjectAssets();
            return true;
        }

        public static void RegisterIconUrl<T>(Uri uri)
            where T : Asset
        {
            var name = typeof(T).Name;
            AssetIconUriCache[name] = uri;
        }

        public static Uri GetIconUri<T>()
            where T : Asset
        {
            return AssetIconUriCache[typeof(T).Name];
        }

        public static T GetAssetFromFullPath<T>(string fullPath)
           where T : Asset
        {
            var a = _projectAssets.Values.FirstOrDefault(t => t.FullPath.Equals(fullPath));
            return a as T ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Create <see cref="AssetTreeComponent"/> basis on <see cref="RootFolder"/>
        /// </summary>
        /// <returns>New <see cref="AssetTreeComponent"/> instance.</returns>
        public static AssetTreeComponent CreateNewRootTree()
        {
            var root = AssetTreeComponent.CreateNewRootTree(Root, Root.Name);
            InitAssetTreeFromAssetsFolderRecursive(Root, root);
            return root;
        }

        public static T CreateAsset<T>(AssetContext context) where T : Asset, new()
        {
            var asset = new T();
            asset.Initialize(context);
            return asset;
        }

        public static NormalFolder CreateFolder(this NormalFolder folder, string name)
        {
            try
            {
                var d = Directory.CreateDirectory(Path.Combine(folder.FullPath, name));
                var f = new NormalFolder();
                f.Initialize(new AssetContext(d, folder));
                return f;
            }
            catch (IOException exception)
            {
                LogModule.Debug.LogException(exception);
                throw new InvalidOperationException();
            }
        }

        public static EditorCommon.Result IsValidName(string name, NormalFolder parent)
        {
            var c = name;

            switch (c.Length)
            {
                //Check min length.
                case 0:
                    return EditorCommon.Result.ErrorAssetNameMin;
                //Check max length.
                case > EditorConst.MaxAssetNameLength:
                    return EditorCommon.Result.ErrorAssetNameMax;
            }

            //Check characters.
            var invalidChars = Path.GetInvalidFileNameChars();
            var invalidPathChars = Path.GetInvalidPathChars();

            if (c.IndexOfAny(invalidChars) > 0 || c.IndexOfAny(invalidPathChars) > 0 || c.Contains(" ") || c.Contains("　"))
            {
                return EditorCommon.Result.ErrorAssetNameInvalid;
            }

            //Check Half-width character.
            if (LocalizationManager.ShiftJisEncoding.GetByteCount(c) != c.Length)
            {
                return EditorCommon.Result.ErrorAssetNameDoubleByte;
            }

            //Check if there is a asset with the same name.
            return parent.Child.Count(t => t.Name == c) != 0 ? EditorCommon.Result.ErrorAssetNameSameName : EditorCommon.Result.Ok;
        }

        public static Asset ImportAsset(AssetContext context)
        {
            Asset asset = null!;

            //Copy source file into project.
            Debug.Assert(context.Parent != null, "context.Parent != null");
            File.Copy(context.FullPath, Path.Combine(context.Parent.FullPath, string.Concat(context.Name, context.Extension)));

            switch (context.Extension)
            {
                case ".png":
                case ".jpg":
                    {
                        asset = new Texture();
                        asset.Initialize(context);
                    }
                    break;
            }

            return asset;
        }

        private static void RegisterAsset(Asset asset)
        {
            Debug.Assert(_projectAssets.ContainsKey(asset.Id) == false);
            _projectAssets[asset.Id] = asset;
        }

        private static void InitializeProjectAssets()
        {
            Root = RootFolder.CreateInstance();
            InitializeProjectAssetsRecursive(Root);
        }

        private static void InitializeProjectAssetsRecursive(NormalFolder folder)
        {
            var d = folder.GetDirectoryInfo();
            var cds = d.GetDirectories("*", SearchOption.TopDirectoryOnly);
            var cfs = d.GetFiles("*", SearchOption.TopDirectoryOnly);


            foreach (var file in cfs)
            {
                var context = new AssetContext(file.FullName, folder);

                switch (file.Extension)
                {
                    case ".png":
                    case ".jpg":
                        {
                            Asset asset = new Texture();
                            asset.Initialize(context);
                        }
                        break;
                }
            }

            foreach (var directory in cds)
            {
                var f = new NormalFolder();
                f.Initialize(new AssetContext(directory, folder));
                InitializeProjectAssetsRecursive(f);
            }
        }

        private static void InitAssetTreeFromAssetsFolderRecursive(NormalFolder folder, AssetTreeComponent tree)
        {
            foreach (var c in folder.Child)
            {
                var nt = tree.AddChild(c, c.Name);
                if (c is NormalFolder normalFolder)
                {
                    InitAssetTreeFromAssetsFolderRecursive(normalFolder, nt);
                }
            }
        }

        private static void OnBaseAssetInitialized(Asset obj)
        {
            RegisterAsset(obj);
        }

    }
}
