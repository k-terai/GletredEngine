// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using System.Globalization;
using System.Text;
using GletredEdShare.CoreModule;

namespace GletredEdShare.LocalizationModule
{
    public static class LocalizationManager
    {
        private static readonly Dictionary<string, Dictionary<EditorCommon.Result, string>> SEditorResultStrings = new();

        /// <summary>
        /// Using check project name error.
        /// </summary>
        public static Encoding ShiftJisEncoding { get; private set; } = null!;

        public static void Initialize()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ShiftJisEncoding = Encoding.GetEncoding("Shift_JIS");


            var enUs = CultureInfo.GetCultureInfo("en-US");
            var jaJp = CultureInfo.GetCultureInfo("ja-JP");

            //TODO: Remove external files(Ex.excel).
            SEditorResultStrings[enUs.Name] = new Dictionary<EditorCommon.Result, string>();
            SEditorResultStrings[jaJp.Name] = new Dictionary<EditorCommon.Result, string>();

            SEditorResultStrings[enUs.Name][EditorCommon.Result.ErrorProjectNameMin] = "Project name error. A character has not been entered.";
            SEditorResultStrings[jaJp.Name][EditorCommon.Result.ErrorProjectNameMin] = "プロジェクト名エラー。入力されていません。";

            SEditorResultStrings[enUs.Name][EditorCommon.Result.ErrorProjectNameMax] = $"Project name error.name is must be {EditorConst.MaxProjectNameLength} or less.";
            SEditorResultStrings[jaJp.Name][EditorCommon.Result.ErrorProjectNameMax] = $"プロジェクト名エラー。プロジェクト名は {EditorConst.MaxProjectNameLength} 以下である必要があります。";

            SEditorResultStrings[enUs.Name][EditorCommon.Result.ErrorProjectNameInvalid] = "Project name error. Invalid character in project name.";
            SEditorResultStrings[jaJp.Name][EditorCommon.Result.ErrorProjectNameInvalid] = "プロジェクト名エラー。プロジェクト名に無効な文字が含まれています。";

            SEditorResultStrings[enUs.Name][EditorCommon.Result.ErrorProjectNameInvalid] = "Project name error. Contains double-byte characters.";
            SEditorResultStrings[jaJp.Name][EditorCommon.Result.ErrorProjectNameInvalid] = "プロジェクト名エラー。マルチバイト文字が含まれています。";

            SEditorResultStrings[enUs.Name][EditorCommon.Result.ErrorProjectNamePathExists] = "Path error. A folder with that name already exists at that location.";
            SEditorResultStrings[jaJp.Name][EditorCommon.Result.ErrorProjectNamePathExists] = "プロジェクトパスエラー。指定されたパスには既に別のフォルダが存在しています。";

            SEditorResultStrings[enUs.Name][EditorCommon.Result.ErrorProjectPathNotExists] = "Location error. A folder with that name not exists.";
            SEditorResultStrings[jaJp.Name][EditorCommon.Result.ErrorProjectPathNotExists] = "プロジェクトパスエラー。指定されたパスにはフォルダが見つかりません。";

            SEditorResultStrings[enUs.Name][EditorCommon.Result.ErrorAssetNameMin] = "Asset name error.A character has not been entered.";
            SEditorResultStrings[jaJp.Name][EditorCommon.Result.ErrorAssetNameMin] = " アセット名エラー。入力されていません。";

            SEditorResultStrings[enUs.Name][EditorCommon.Result.ErrorAssetNameMax] = $"Asset name error.name is must be {EditorConst.MaxAssetNameLength} or less.";
            SEditorResultStrings[jaJp.Name][EditorCommon.Result.ErrorAssetNameMax] = $"アセット名エラー。アセット名は {EditorConst.MaxAssetNameLength} 以下である必要があります。";

            SEditorResultStrings[enUs.Name][EditorCommon.Result.ErrorAssetNameInvalid] = "Asset name error.Invalid character in asset name.";
            SEditorResultStrings[jaJp.Name][EditorCommon.Result.ErrorAssetNameInvalid] = "アセット名エラー。プロジェクト名に無効な文字が含まれています。";

            SEditorResultStrings[enUs.Name][EditorCommon.Result.ErrorAssetNameDoubleByte] = "Asset name error. Contains double-byte characters.";
            SEditorResultStrings[jaJp.Name][EditorCommon.Result.ErrorAssetNameDoubleByte] = "アセット名エラー。マルチバイト文字が含まれています。";

            SEditorResultStrings[enUs.Name][EditorCommon.Result.ErrorAssetNameSameName] = "Asset name error. Same name asset already exists.";
            SEditorResultStrings[jaJp.Name][EditorCommon.Result.ErrorAssetNameSameName] = "アセット名エラー。同名アセットが既に存在しています。";
        }

        public static string GetString(EditorCommon.Result result)
        {
            var name = CultureInfo.CurrentCulture.Name;

            if (!SEditorResultStrings.ContainsKey(name) || !SEditorResultStrings[name].ContainsKey(result))
            {
                return string.Empty;
            }

            return SEditorResultStrings[name][result];
        }

    }
}
