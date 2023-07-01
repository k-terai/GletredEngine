// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include "IModule.h"

namespace GletredEngine
{
	class IRenderModule : public IModule
	{
	public:
		~IRenderModule() override = default;
		virtual void CreateScene(RenderModuleSceneInitData data) = 0;
	};
}