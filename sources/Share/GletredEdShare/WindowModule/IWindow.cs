﻿// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace GletredEdShare.WindowModule
{
    public interface IWindow<out T>
    where T : WindowViewModel
    {
        T ViewModel { get; }

        void ShowWindow();
    }
}
