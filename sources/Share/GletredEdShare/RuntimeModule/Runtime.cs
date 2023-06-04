// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Diagnostics;
using GletredEdShare.CoreModule;
using GletredEdShare.NativeModule;

namespace GletredEdShare.RuntimeModule
{
    public static class Runtime
    {
        public static bool IsActive { get; private set; }

        private static DllInfo _editorEngineDllInfo;

        #region DllDelegate

        private delegate void Initialize(IntPtr windowHandle);

        private delegate void Terminate();

        #endregion


        public static void LaunchEdEngine(IntPtr windowHandle)
        {
            var directory = CoreUtility.GetExecutingAssemblyDirectory();
            var engine = CoreUtility.GetFileInfoInParent(EditorConst.EditorEngineDllName, directory);
            Debug.Assert(engine != null, nameof(engine) + " != null");

            var context = new DllContext()
            {
                FilePath = engine.FullName
            };

            NativeLibraryManager.LoadNativeLibrary(context, out _editorEngineDllInfo);
            NativeLibraryManager.GetNativeDelegate<Initialize>(_editorEngineDllInfo.Id)
                .Invoke(windowHandle);
            IsActive = true;
        }

        public static void TerminateEdEngine()
        {
            NativeLibraryManager.GetNativeDelegate<Terminate>(_editorEngineDllInfo.Id).Invoke();
            NativeLibraryManager.FreeNativeLibrary(_editorEngineDllInfo.Id);
            IsActive = false;
        }
    }
}
