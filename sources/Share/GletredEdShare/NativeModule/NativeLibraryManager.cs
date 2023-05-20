// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace GletredEdShare.NativeModule
{
    public static class NativeLibraryManager
    {
        private static readonly Dictionary<Guid, IntPtr> LoadLib = new();

        public static bool LoadNativeLibrary(in DllContext context, out DllInfo info)
        {
            info = new DllInfo();

            if (File.Exists(context.FilePath) == false)
            {
                return false;
            }

            info.Id = Guid.NewGuid();
            var ptr = NativeLibrary.Load(context.FilePath);
            if (ptr == IntPtr.Zero)
            {
                return false;
            }

            LoadLib[info.Id] = ptr;
            return true;
        }

        public static void FreeNativeLibrary(in Guid id)
        {
            if (!LoadLib.ContainsKey(id))
            {
                return;
            }

            NativeLibrary.Free(LoadLib[id]);
            LoadLib.Remove(id);
        }

        public static T GetNativeDelegate<T>(in Guid id)
            where T : Delegate
        {
            if (LoadLib.ContainsKey(id) == false)
            {
                return null!;
            }

            var export = NativeLibrary.GetExport(LoadLib[id], typeof(T).Name);
            return (T)Marshal.GetDelegateForFunctionPointer(export, typeof(T));
        }
    }
}
