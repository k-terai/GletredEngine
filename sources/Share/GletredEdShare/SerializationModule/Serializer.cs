// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Diagnostics;
using System.Reflection;
using GletredEdShare.CoreModule;

namespace GletredEdShare.SerializationModule
{
    public static class Serializer
    {
        public static bool Serialize<T>(T file, string path) where T : SerializableBase
        {
            var att = typeof(T).GetCustomAttribute<SerializeAttribute>();
            Debug.Assert(att != null, nameof(att) + " != null");

            return att.Type switch
            {
                SerializeAttribute.SerializeType.Json => JsonSerializer.ToJson(file, path),
                SerializeAttribute.SerializeType.Xml => throw new InvalidOperationException(),
                _ => throw new InvalidOperationException()
            };
        }

        public static T Deserialize<T>(string path) where T : SerializableBase
        {
            var att = typeof(T).GetCustomAttribute<SerializeAttribute>();
            Debug.Assert(att != null, nameof(att) + " != null");

            return att.Type switch
            {
                SerializeAttribute.SerializeType.Json => JsonSerializer.FromJson<T>(path),
                SerializeAttribute.SerializeType.Xml => throw new InvalidOperationException(),
                _ => throw new InvalidOperationException()
            };
        }
    }
}
