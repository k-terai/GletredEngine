// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.


#if WPF
using System.Windows.Controls;
#endif

namespace GletredEdShare.ControlModule
{
    public interface IControl
    {
#if WPF
        UserControl Control { get; }
#endif
    }
}