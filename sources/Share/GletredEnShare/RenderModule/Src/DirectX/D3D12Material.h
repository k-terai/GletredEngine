// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include "RenderModule/Src/DirectX/D3D12Common.h"
#include "RenderModule/Src/DirectX/D3D12Shader.h"
#include "RenderModule/Src/DirectX/D3D12Texture.h"
#include "CoreModule/Inc/Resource.h"

namespace GletredEngine
{
	class D3D12Material : public Resource
	{
	public:
		D3D12Material();
		~D3D12Material() override;

		void Initialize(uniqueid shaderResourceId);
		void Destroy();

		void SetTexture(uniqueid textureResourceId);
		D3D12Texture* GetTexture(const uint32 index) const
		{
			return reinterpret_cast<D3D12Texture*> (Textures[index].get());
		}
		bool IsAnyTexture() const
		{
			return TexCount != 0;
		}


		D3D12Shader* GetShader() const { return reinterpret_cast<D3D12Shader*>(MainShader.get()); }

	private:
		std::shared_ptr<Resource> MainShader;
		std::vector<std::shared_ptr<Resource>> Textures;
		uint32 TexCount;
	};
}
