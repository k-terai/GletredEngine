// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Runtime.Serialization;

namespace GletredEdShare.CoreModule
{
    public abstract record SerializableBase
    {
        [DataMember] public uint Version { get; set; }

        [DataMember]
        public Guid Id { get; set; }
    }
}
