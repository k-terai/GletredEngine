﻿// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Threading;

namespace GletredWpfEditor
{
    public static class EditorDispatcher
    {
        public static void Execute(Action action, DispatcherPriority priority)
        {
            if (Application.Current == null)
            {
                return;
            }

            Application.Current.Dispatcher.Invoke(
                action, priority, new object[]
                {

                });
        }
    }
}
