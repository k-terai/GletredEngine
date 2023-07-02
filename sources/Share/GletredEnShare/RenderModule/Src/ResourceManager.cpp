// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Src/ResourceManager.h"
#include "RenderModule/Src/DirectX/D3D12Shader.h"

using namespace  GletredEngine;
using namespace  std;



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


}

void ResourceManager::Terminate()	
{
	for(auto& v : ResourceMap)
	{
		v.second.reset();
	}
	ResourceMap.clear();
}



