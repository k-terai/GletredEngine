// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Src/DirectX/D3D12Shader.h"
#include "RenderModule/Src/DirectX/D3D12ShaderCompiler.h"

using namespace std;
using namespace GletredEngine;

D3D12Shader::D3D12Shader()
{

}

D3D12Shader::~D3D12Shader()
{

}

void D3D12Shader::Initialize(const ctstring vsFilePath, const ctstring psFilePath)
{
	Vertex = make_unique<D3D12ShaderObject>();
	Pixel = make_unique<D3D12ShaderObject>();

	D3D12ShaderCompiler::GetShaderObjectFromCso(vsFilePath, Vertex.get());
	D3D12ShaderCompiler::GetShaderObjectFromCso(psFilePath, Pixel.get());

	IsInitialized = true;
}


