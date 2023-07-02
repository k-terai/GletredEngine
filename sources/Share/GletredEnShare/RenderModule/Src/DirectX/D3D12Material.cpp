// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Src/DirectX/D3D12Material.h"
#include "RenderModule/Src/ResourceManager.h"
#include "RenderModule/Src/DirectX/D3D12Mesh.h"

using namespace GletredEngine;
using namespace  std;

D3D12Material::D3D12Material(): TexCount(0)
{

}

D3D12Material::~D3D12Material()
{
	MainShader = nullptr;
	Textures.clear();
}

void D3D12Material::Initialize(const uniqueid shaderResourceId)
{
	MainShader = ResourceManager::GetInstance()->GetResource(shaderResourceId);
}

void D3D12Material::Destroy()
{
	MainShader.reset();
	for(auto& t : Textures)
	{
		t.reset();
	}
	Textures.clear();
	TexCount = 0;
}

void D3D12Material::SetTexture(uniqueid textureResourceId)
{
	Textures.emplace_back(ResourceManager::GetInstance()->GetResource(textureResourceId));
	++TexCount;
}

