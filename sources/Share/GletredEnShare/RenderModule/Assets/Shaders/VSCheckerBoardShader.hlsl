// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include"CheckerBoardShader.hlsli"

PSInput main(const float4 position : SV_Position, const float2 uv : TEXCOORD)
{
    PSInput result;
    result.position = position;
    result.uv = uv;

    return result;
}