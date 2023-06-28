// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include <cassert>
#include "RenderModule/Src/DirectX/D3D12ShaderCompiler.h"
#include "RenderModule/Src/DirectX/D3D12Helper.h"
#include "CoreModule/Inc/Logger.h"

using namespace  std;
using namespace  GletredEngine;

void D3D12ShaderCompiler::CompileFromFile(const ShaderType type, ctstring filePath, CID3DBlob** shaderBlob)
{
	ID3DBlob* errorBlob = nullptr;

	//NOTE : D3DCompileFromFile is require multi byte string.
	auto target = "vs_5_0";
	auto entry = "main";

	switch (type)
	{
	case ShaderType::Vertex:
		target = "vs_5_0";
		break;

	case ShaderType::Pixel:
		target = "ps_5_0";
		break;
	}

	const auto result = D3DCompileFromFile(filePath,
		nullptr,
		D3D_COMPILE_STANDARD_FILE_INCLUDE,
		entry,
		target,
		D3DCOMPILE_DEBUG | D3DCOMPILE_SKIP_OPTIMIZATION,
		0,
		shaderBlob,
		&errorBlob);

	SetExceptionIfFailed(result);
}

void D3D12ShaderCompiler::GetShaderObjectFromCso(const ctstring filePath, D3D12ShaderObject* shaderObject)
{
	assert(shaderObject != nullptr);
	FILE* file = nullptr;
	_wfopen_s(&file, filePath, L"rb");

	if (!file)
	{
		return;
	}

	int result = fseek(file, 0, SEEK_END);
	assert(result == 0);

	const long size = ftell(file);
	rewind(file);

	shaderObject->Size = size;
	shaderObject->Ptr = malloc(size);

	if (shaderObject->Ptr == nullptr)
	{
		return;
	}

	fread(shaderObject->Ptr, 1, shaderObject->Size, file);
	result = fclose(file);
	assert(result == 0);
	}
