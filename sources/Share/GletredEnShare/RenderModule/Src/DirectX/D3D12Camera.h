// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include "CoreModule/Inc/NonCopyable.h"
#include "RenderModule/Src/DirectX/D3D12Common.h"

namespace GletredEngine
{
	class D3D12Camera : public NonCopyable
	{
	public:
		D3D12Camera();
		~D3D12Camera() override;

		void Initialize(float width, float height);

		D3D12_VIEWPORT* GetViewport()
		{
			return &Viewport;
		}

		D3D12_RECT* GetRect()
		{
			return &ScissorRect;
		}

		void Resize(float width, float height);

	private:
		CD3DX12_VIEWPORT Viewport;
		CD3DX12_RECT ScissorRect;
	};
}
