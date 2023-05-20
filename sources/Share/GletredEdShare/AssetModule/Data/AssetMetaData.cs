// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Runtime.Serialization;
using GletredEdShare.CoreModule;

namespace GletredEdShare.AssetModule
{
    [DataContract]
    [SerializationModule.Serialize(SerializationModule.SerializeAttribute.SerializeType.Json)]
    public sealed record AssetMetaData : SerializableBase
    {
        [DataContract]
        public record TextureData
        {
            [DataMember]
            public double Width { get; set; }

            [DataMember]
            public double Height { get; set; }
        }

        /// <summary>
        /// Asset convert type.(Ex. texture id .dds format)
        /// </summary>
        [DataMember]
        public string ConvertType { get; set; } = null!;

        /// <summary>
        /// Texture data.
        /// </summary>
        [DataMember]
        public TextureData Texture { get; set; } = null!;

        public AssetMetaData()
        {
            Version = 1;
        }

    }
}