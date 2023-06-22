// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "PositionColor.hlsli"

float4 main(PSInput input) : SV_TARGET
{
    return input.color;
}
