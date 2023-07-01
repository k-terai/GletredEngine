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
                    Runtime.EdEngine.EdLaunch();
                }
                ,
                _ => Runtime.EdEngine.IsActive == false);

            TerminateRuntimeCommand = new DelegateCommand(

                _ =>
                {
                    Runtime.EdEngine.EdTerminate();
                }
                ,
                _ => Runtime.EdEngine.IsActive);
        }
    }
}
