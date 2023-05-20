// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using GletredEdShare.AssetModule.Contexts;
using GletredEdShare.CoreModule;

#if WPF
using System;
using System.IO;
using System.Drawing;
#endif

namespace GletredEdShare.AssetModule
{
    public sealed class Texture : Asset
    {
        public override void CreateThumbnail(bool isForceUpdate)
        {
#if WPF
            CreateRuntimeDirectoryIfNotExists();
            var path = System.IO.Path.Combine(RuntimeDirPath, Id.ToString() + EditorConst.ThumbnailFileSize);

            if (isForceUpdate == false && File.Exists(path))
            {
                return;
            }

            using var source = new Bitmap(FullPath);
            const int width = EditorConst.ThumbnailFileSize;
            const int height = EditorConst.ThumbnailFileSize;

            using var dest = new Bitmap(source, width, height);
            dest.Save(path);
            ThumbnailUri = new Uri(path, UriKind.Absolute);

#else
            base.CreateThumbnail(false);
#endif
        }

        protected override bool CreateMetaData(AssetContext context)
        {
            MetaData = new AssetMetaData()
            {
                Texture = new AssetMetaData.TextureData()
                {
                    Width = 100,
                    Height = 100
                }
            };

            return base.CreateMetaData(context);
        }
    }
}