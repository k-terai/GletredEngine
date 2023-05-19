// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace GletredEdShare.CoreModule
{
    public static class EditorConst
    {
        // Projects.
        public const string ProjectDataExtension = ".project";
        public const string AssetsDirectoryName = "Assets";
        public const string RuntimeDirectoryName = "Runtime";
        public const string PluginDirectoryName = "Plugins";
        public const string ProjectSettingsDirectoryName = "ProjectSettings";
        public const string UserSettingsDirectoryName = "UserSettings";
        public const int MaxProjectNameLength = 20;

        //Assets.
        public const string AssetMetadataExtension = ".meta";
        public const int MaxAssetNameLength = 50;
        public const string AssetImportFilter = "Asset files(*.png *.jpg *.obj *.fbx)|*.png;*.fbx;*.obj;*.jpg";

        // Engines.
        public const string EngineVersion = "1.0.0";

        // Thumbnail.
        public const string ThumbnailFileImageExtension = ".png";
        public const int ThumbnailFileSize = 256;
        public const int TinyThumbnailSize = 40;
        public const int SmallThumbnailSize = 80;
        public const int MediumThumbnailSize = 150;
        public const int LargeThumbnailSize = 200;
        public const int HugeThumbnailSize = 300;

        // Language code.
        public const string JapaneseCode = "ja-jp";
        public const string EnglishCode = "en-us";

        // Windows helper exe.
        public const string Explorer = "EXPLORER.EXE";

        // Editor engine dll name.
        public const string EditorEngineDllName = "GletredEdEngine.dll";

    }
}
