// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include "CoreModule/Inc/Singleton.h"
#include "CoreModule/Inc/Resource.h"
#include "RenderModule/Src/BuildInResource.h"
#include <memory>
#include <unordered_map>


namespace GletredEngine
{
	class ResourceManager : public Singleton<ResourceManager>
	{
	public:
		void Initialize();
		void Terminate();

		const BuildInResource* GetBuildResourceData() const
		{
			return &BuildResource;
		}

		std::shared_ptr<Resource> GetResource(const uniqueid id)
		{
			return ResourceMap[id];
		}

		template<class T>
		T* CreateResource(const uniqueid id)
		{
			ResourceMap[id] = std::make_shared<T>();
			ResourceMap[id]->SetResourceId(id);
			return reinterpret_cast<T*>(ResourceMap[id].get());
		}

		bool IsExists(const uniqueid id) const
		{
			return ResourceMap.count(id) > 0;
		}

	private:
		friend class  Singleton<ResourceManager>; //Access GetInstance
		ResourceManager();
		~ResourceManager() override;

		std::unordered_map<uniqueid, std::shared_ptr<Resource>> ResourceMap;
		BuildInResource BuildResource;
	};
}
