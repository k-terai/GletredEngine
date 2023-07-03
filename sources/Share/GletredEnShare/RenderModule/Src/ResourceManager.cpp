// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Src/ResourceManager.h"
#include "RenderModule/Src/DirectX/D3D12Shader.h"
#include "RenderModule/Src/DirectX/D3D12Mesh.h"
#include "RenderModule/Src/DirectX/D3D12Manager.h"
#include "VertexTypes.h"

using namespace  GletredEngine;
using namespace  std;
using namespace  DirectX;



ResourceManager::ResourceManager()
{

}

ResourceManager::~ResourceManager()
{
	ResourceMap.clear();
}

void ResourceManager::Initialize()
{
	BuildResource.GenerateHash();

	//Create position color shader.
	{
		const auto r = CreateResource<D3D12Shader>(BuildResource.PositionColorId);
		r->Initialize(BuildResource.VsPositionColorPath, BuildResource.PsPositionColorPath);
	}

	//Create checker board shader.
	{
		const auto r = CreateResource<D3D12Shader>(BuildResource.CheckerBoardId);
		r->Initialize(BuildResource.VsCheckerBoardPath, BuildResource.PsCheckerBoardPath);
	}

	//Create triangle mesh
	{
		const auto r = CreateResource<D3D12Mesh<VertexPositionColor>>(BuildResource.CheckerBoardId);
		vector<VertexPositionColor> vertex;
		vertex.emplace_back(VertexPositionColor(XMFLOAT3(0.0f, 0.25f / 2, 0.0f), XMFLOAT4(1.0f, 0.0f, 0.0f, 1.0f)));
		vertex.emplace_back(VertexPositionColor(XMFLOAT3(0.25f, -0.25f / 2, 0.0f), XMFLOAT4(0.0f, 1.0f, 0.0f, 1.0f)));
		vertex.emplace_back(VertexPositionColor(XMFLOAT3(-0.25f, -0.25f / 2, 0.0f), XMFLOAT4(0.0f, 0.0f, 1.0f, 1.0f)));
		r->Initialize(D3D12Manager::GetInstance()->GetDevice(), vertex, VertexPositionColor::InputLayout);
	}


}

void ResourceManager::Terminate()
{
	for (auto& v : ResourceMap)
	{
		v.second.reset();
	}
	ResourceMap.clear();
}



