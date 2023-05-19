// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Runtime.Serialization;
using GletredEdShare.CoreModule;
using GletredEdShare.SerializationModule;

namespace GletredEdShare.ProjectModule
{
    [DataContract]
    [Serialize(SerializeAttribute.SerializeType.Json)]
    public record ProjectData : SerializableBase
    {
        [DataMember]
        public string EngineVersion { get; set; } = string.Empty;
    }
}
