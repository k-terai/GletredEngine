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
        public static EditorEngine EdEngine { get; set; } = new();
    }
}
