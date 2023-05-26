// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include <mutex>
#include "CoreModule/Inc/Singleton.h"

namespace GletredEngine
{
	class D3D12Manager : public Singleton<D3D12Manager>
	{
	public:
		void Initialize();

	private:
		friend class  Singleton<D3D12Manager>; //Access GetInstance
		D3D12Manager();
		~D3D12Manager() override;

		void SetDebugLayer();
	};
}
