// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "PositionColor.hlsli"

PSInput main(const float4 position : POSITION, const float4 color : COLOR)
{
    PSInput result;
    result.position = position;
    result.color = color;
    return result;
}