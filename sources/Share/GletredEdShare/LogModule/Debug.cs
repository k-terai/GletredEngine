// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace GletredEdShare.LogModule
{
    public class Debug
    {
        public static event Action<string?, string?>? OnLog;
        public static event Action<string?, string?>? OnWarning;
        public static event Action<string?, string?>? OnError;

        public static event Action<Exception>? OnException;

        public static void Log(object? message, object? from)
        {
            OnLog?.Invoke(message?.ToString(), from?.ToString());
        }

        public static void LogWarning(object? message, object? from)
        {
            OnWarning?.Invoke(message?.ToString(), from?.ToString());
        }

        public static void LogError(object? message, object? from)
        {
            OnError?.Invoke(message?.ToString(), from?.ToString());
        }

        public static void LogException(Exception exception)
        {
            OnException?.Invoke(exception);
        }

        public static void Assert(bool condition)
        {
            System.Diagnostics.Debug.Assert(condition);
        }
    }
}
