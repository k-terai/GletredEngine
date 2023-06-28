// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include "CoreModule/Inc/Common.h"
#include "CoreModule/Inc/Platform.h"
#include "RenderModule/Src/DirectX/D3D12ShaderObject.h"
#include "RenderModule/Src/DirectX/D3D12Common.h"

namespace GletredEngine
{
	namespace D3D12ShaderCompiler
	{
		void CompileFromFile(const ShaderType type, ctstring filePath, CID3DBlob** shaderBlob);
		void GetShaderObjectFromCso(ctstring filePath, D3D12ShaderObject* shaderObject);
	}
}
