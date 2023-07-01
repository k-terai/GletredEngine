// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include "CoreModule/Inc/IRenderModule.h"

namespace  GletredEngine
{
	class RenderModule final : public IRenderModule
	{
	public:
		void Initialize(GlobalData* data) override;
		void Startup() override;
		void Shutdown() override;
		void Update() override;
		void Terminate() override;

		void CreateScene(RenderModuleSceneInitData data) override;
	};
}
