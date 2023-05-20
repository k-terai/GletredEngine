// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.Generic;
using GletredEdShare.CoreModule;

namespace GletredWpfEditor.Commands
{
    public static partial class EditorCommand
    {
        public static Dictionary<string, DelegateCommand> AllCommands { get; } = new();

        public static void Initialize()
        {
            InitCommonCommands();
            InitProjectCommands();
        }
    }
}