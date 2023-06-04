// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using GletredEdShare.ControlModule;

namespace GletredWpfEditor.Viewport
{
    public interface IViewportControl : IControl<ViewportControlViewModel>
    {
        IntPtr WindowHandle { get; }
    }
}
