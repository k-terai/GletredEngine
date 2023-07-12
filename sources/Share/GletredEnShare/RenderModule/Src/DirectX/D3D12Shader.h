// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include "CoreModule/Inc/Resource.h"
#include "RenderModule/Src/DirectX/D3D12Common.h"
#include "RenderModule/Src/DirectX/D3D12ShaderObject.h"

namespace GletredEngine
{
	class D3D12Shader final : public Resource
	{
	public:
		D3D12Shader();
		~D3D12Shader() override;

		void Initialize(ctstring vsFilePath, ctstring psFilePath);

		D3D12ShaderObject* GetVertex() const
		{
			return Vertex.get();
		}

		D3D12ShaderObject* GetPixel() const
		{
			return Pixel.get();
		}

		D3D12_SHADER_BYTECODE GetVertexByteCode() const
		{
			assert(IsInitialize());
			D3D12_SHADER_BYTECODE b;
			b.BytecodeLength = Vertex->Size;
			b.pShaderBytecode = Vertex->Ptr;
			return b;
		}

		D3D12_SHADER_BYTECODE GetPixelByteCode() const
		{
			assert(IsInitialize());
			D3D12_SHADER_BYTECODE b;
			b.BytecodeLength = Pixel->Size;
			b.pShaderBytecode = Pixel->Ptr;
			return b;
		}


	private:
		std::unique_ptr<D3D12ShaderObject> Vertex;
		std::unique_ptr<D3D12ShaderObject> Pixel;
	};
}