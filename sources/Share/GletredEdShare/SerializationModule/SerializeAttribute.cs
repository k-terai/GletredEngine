// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace GletredEdShare.SerializationModule
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SerializeAttribute : Attribute
    {
        public enum SerializeType
        {
            Json,
            Xml
        }

        public SerializeType Type { get; }

        public SerializeAttribute(SerializeType type)
        {
            Type = type;
        }
    }
}
