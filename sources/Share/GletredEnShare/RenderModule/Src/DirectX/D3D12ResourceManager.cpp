// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Src/DirectX//D3D12ResourceManager.h"
#include "RenderModule/Src/DirectX/D3D12Shader.h"
#include "RenderModule/Src/DirectX/D3D12Mesh.h"
#include "RenderModule/Src/DirectX/D3D12Texture.h"
#include "RenderModule/Src/DirectX/D3D12Manager.h"
#include "RenderModule/Src/DirectX/D3D12Material.h"
#include "VertexTypes.h"

using namespace  GletredEngine;
using namespace  std;
using namespace  DirectX;



D3D12ResourceManager::D3D12ResourceManager()
{

}

D3D12ResourceManager::~D3D12ResourceManager()
{
	ResourceMap.clear();
}

void D3D12ResourceManager::Initialize()
{
	BuildResource.GenerateHash();

	// Create position color shader.
	{
		const auto r = CreateResource<D3D12Shader>(BuildResource.PositionColorId);
		r->Initialize(BuildResource.VsPositionColorPath, BuildResource.PsPositionColorPath);
	}

	// Create position color material.
	{
		const auto r = CreateResource<D3D12Material>(BuildResource.PositionColorDefaultMaterialId);
		r->Initialize(BuildResource.PositionColorId);
	}

	//Create checker board shader.
	{
		const auto r = CreateResource<D3D12Shader>(BuildResource.CheckerBoardId);
		r->Initialize(BuildResource.VsCheckerBoardPath, BuildResource.PsCheckerBoardPath);
	}

	//Create triangle mesh
	{
		const auto r = CreateResource<D3D12Mesh<VertexPositionColor>>(BuildResource.TriangleMeshId);
		vector<VertexPositionColor> vertex;
		vertex.emplace_back(VertexPositionColor(XMFLOAT3(0.0f, 0.25f / 5, 0.0f), XMFLOAT4(1.0f, 0.0f, 0.0f, 1.0f)));
		vertex.emplace_back(VertexPositionColor(XMFLOAT3(0.25f, -0.25f / 5, 0.0f), XMFLOAT4(0.0f, 1.0f, 0.0f, 1.0f)));
		vertex.emplace_back(VertexPositionColor(XMFLOAT3(-0.25f, -0.25f / 5, 0.0f), XMFLOAT4(0.0f, 0.0f, 1.0f, 1.0f)));
		r->Initialize(D3D12Manager::GetInstance()->GetDevice(), vertex, VertexPositionColor::InputLayout);
		r->Map(vertex);
	}

	//Create checker board texture.
	{
		const auto r = CreateResource<D3D12Texture>(BuildResource.CheckerBoardTextureId);
		r->InitializeAsCheckerBoard(D3D12Manager::GetInstance()->GetDevice(), 256, 256, 4);
	}


}

void D3D12ResourceManager::Terminate()
{
	for (auto& v : ResourceMap)
	{
		v.second.reset();
	}
	ResourceMap.clear();
}



