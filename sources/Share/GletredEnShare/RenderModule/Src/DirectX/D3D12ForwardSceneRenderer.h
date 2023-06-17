// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include "RenderModule/Src/DirectX/D3D12SceneRenderer.h"

namespace GletredEngine
{
	class D3D12ForwardSceneRenderer : public D3D12SceneRenderer
	{
	public:
		D3D12ForwardSceneRenderer();
		~D3D12ForwardSceneRenderer() override;

		void Render() override;

	private:
		void TestRender();
	};
}
