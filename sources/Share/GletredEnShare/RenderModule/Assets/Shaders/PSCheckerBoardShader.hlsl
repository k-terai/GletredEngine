// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include"CheckerBoardShader.hlsli"

float4 main(const PSInput input) : SV_TARGET
{
    return mainTexture.Sample(mainSampler, input.uv);
}