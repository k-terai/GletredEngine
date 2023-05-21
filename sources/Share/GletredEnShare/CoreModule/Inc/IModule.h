// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include "CoreModule/Inc/GlobalData.h"

namespace  GletredEngine
{
	class IModule
	{
	public:
		IModule() = default;
		virtual ~IModule() = default;
		virtual void Initialize(GlobalData* data) = 0;
		virtual void Startup() = 0;
		virtual void Update() = 0;
		virtual void Terminate() = 0;
	};
}
