// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.


namespace GletredEdShare.CoreModule
{
    public static class EditorCommon
    {
        /// <summary>
        /// Define all editor results that should be check.
        /// This enum is only define editor results such as folder path missing,not include runtime.
        /// </summary>
        public enum Result
        {
            Ok = 0,                         // Success
            ErrorProjectNameMin,          // Project name error.A character has not been entered.
            ErrorProjectNameMax,          // Project name error.name is too long.
            ErrorProjectNameInvalid,      // Project name error.Invalid character in project name.
            ErrorProjectNameDoubleByte,   // Project name error.Contains double-byte characters.
            ErrorProjectNamePathExists,  // Path error.A folder with that name already exists at that location.
            ErrorProjectPathNotExists,   // Location error.A folder with that name not exists.
            ErrorAssetNameMin,            // Asset name error.A character has not been entered.
            ErrorAssetNameMax,            // Asset name error.name is too long.
            ErrorAssetNameInvalid,        // Asset name error.Invalid character in asset name.
            ErrorAssetNameDoubleByte,     // Asset name error. Contains double-byte characters.
            ErrorAssetNameSameName        // Asset name error. Same name asset already exists.
        }

        /// <summary>
        /// Define all editor and engine log types.
        /// </summary>
        [System.Flags]
        public enum LogType
        {
            None = 0x01 << 1,
            Information = 0x01 << 2,
            Warning = 0x01 << 3,
            Error = 0x01 << 4,
            All = None | Information | Warning | Error
        }

        /// <summary>
        /// Define all remote type.
        /// </summary>
        public enum RemoteType
        {
            Managed, // Managed mode
            Native   // Native mode
        }
    }
}
