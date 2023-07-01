// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include "CoreModule/Inc/IModule.h"
#include "CoreModule/Inc/Singleton.h"

namespace GletredEngine
{
	class FrameworkModule final : IModule
	{
	public:
		FrameworkModule();
		~FrameworkModule() override;
		void Initialize(GlobalData* data) override;
		void Startup() override;
		void Shutdown() override;
		void Update() override;
		void Terminate() override;

		IRenderModule* GetRenderModule() const
		{
			return RenderModule;
		}

	private:
		IRenderModule* RenderModule;
	};
}
