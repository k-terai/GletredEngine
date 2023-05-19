// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GletredEdShare.CoreModule;

namespace GletredEdShare.LogModule
{
    [DataContract]
    [Serialization.Serialize(Serialization.SerializeAttribute.SerializeType.Json)]
    public sealed record LogData : SerializableBase
    {
        [DataContract]
        public class LogPack : NotifyPropertyChangedBase
        {
            [DataMember]
            public EditorCommon.LogType Type { get; set; }

            [DataMember] public string Message { get; set; } = null!;

            [DataMember] public string From { get; set; } = null!;

            [DataMember]
            public DateTime Time { get; set; }
        }

        [DataMember]
        public List<LogPack> Infos { get; set; }

        [DataMember]
        public List<LogPack> Warnings { get; set; }

        [DataMember]
        public List<LogPack> Errors { get; set; }

        public LogData()
        {
            Version = 1;
            Infos = new List<LogPack>();
            Warnings = new List<LogPack>();
            Errors = new List<LogPack>();
        }

    }
}
