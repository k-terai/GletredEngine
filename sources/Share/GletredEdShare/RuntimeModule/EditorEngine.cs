// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using GletredEdShare.CoreModule;
using GletredEdShare.NativeModule;
using System;
using System.Diagnostics;

namespace GletredEdShare.RuntimeModule
{
    public class EditorEngine
    {
        public bool IsActive { get; private set; }

        public event Action? OnEdEngineLaunched;
        public event Action? OnEdEngineTerminated;

        private DllInfo _dllInfo;

        private delegate void Initialize();
        private delegate void Startup();
        private delegate void Shutdown();
        private delegate void Update();
        private delegate void Terminate();
        private delegate void CreateScene(IntPtr hwnd);

        public void EdLaunch()
        {
            var directory = CoreUtility.GetExecutingAssemblyDirectory();
            var engine = CoreUtility.GetFileInfoInParent(EditorConst.EditorEngineDllName, directory);
            Debug.Assert(engine != null, nameof(engine) + " != null");

            var context = new DllContext()
            {
                FilePath = engine.FullName
            };

            NativeLibraryManager.LoadNativeLibrary(context, out _dllInfo);
            NativeLibraryManager.GetNativeDelegate<Initialize>(_dllInfo.Id)
                .Invoke();
            IsActive = true;
            OnEdEngineLaunched?.Invoke();
        }

        public void EdStartup()
        {
            NativeLibraryManager.GetNativeDelegate<Startup>(_dllInfo.Id).Invoke();
        }

        public void EdShutdown()
        {
            NativeLibraryManager.GetNativeDelegate<Shutdown>(_dllInfo.Id).Invoke();
        }

        public void EdUpdate()
        {
            NativeLibraryManager.GetNativeDelegate<Update>(_dllInfo.Id).Invoke();
        }

        public void EdCreateScene(IntPtr hwnd)
        {
            NativeLibraryManager.GetNativeDelegate<CreateScene>(_dllInfo.Id).Invoke(hwnd);
        }

        public void EdTerminate()
        {
            NativeLibraryManager.GetNativeDelegate<Terminate>(_dllInfo.Id).Invoke();
            NativeLibraryManager.FreeNativeLibrary(_dllInfo.Id);
            IsActive = false;
            OnEdEngineTerminated?.Invoke();
        }
    }
}
