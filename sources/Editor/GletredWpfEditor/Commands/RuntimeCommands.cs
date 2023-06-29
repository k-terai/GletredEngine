// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Diagnostics;
using System.Linq;
using GletredEdShare.CoreModule;
using GletredEdShare.RuntimeModule;
using GletredWpfEditor.Viewport;

namespace GletredWpfEditor.Commands
{
    public static partial class EditorCommand
    {
        public static DelegateCommand LaunchRuntimeCommand { get; private set; } = null!;
        public static DelegateCommand TerminateRuntimeCommand { get; private set; } = null!;

        private static void InitRuntimeCommands()
        {
            LaunchRuntimeCommand = new DelegateCommand(

                _ =>
                {
                    var viewport =
                        EditorManager.MainWindow.ViewModel.MainTab.First(t => t.OwnerControl is IViewportControl).OwnerControl as IViewportControl;

                    Debug.Assert(viewport != null, nameof(viewport) + " != null");
                    Runtime.LaunchEdEngine(viewport.WindowHandle);
                }
                ,
                _ => Runtime.IsActive == false);

            TerminateRuntimeCommand = new DelegateCommand(

                _ =>
                {
                    Runtime.TerminateEdEngine();
                }
                ,
                _ => Runtime.IsActive);
        }
    }
}
