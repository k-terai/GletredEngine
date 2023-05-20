// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Windows;
using GletredEdShare.CoreModule;

namespace GletredWpfEditor.Commands
{
    public static partial class EditorCommand
    {
        public static DelegateCommand ExitCommand { get; private set; } = null!;

        public static void InitCommonCommands()
        {
            ExitCommand = new DelegateCommand(

                _ =>
                {
                    EditorManager.Shutdown();
                }
                ,
                _ => Application.Current.MainWindow != null);

            AllCommands["ExitCommand"] = ExitCommand;
        }
    }
}