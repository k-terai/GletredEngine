// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include "CoreModule/Inc/NonCopyable.h"
#include "RenderModule/Src/DirectX/D3D12Common.h"
#include "RenderModule/Src/DirectX/D3D12RenderTarget.h"
#include "RenderModule/Src/DirectX/D3D12GraphicsCommand.h"

namespace GletredEngine
{
	class D3D12SceneRenderer : public NonCopyable
	{
	public:
		D3D12SceneRenderer();
		virtual ~D3D12SceneRenderer() override;
		void Initialize(ComPtr<CID3D12Device> device,
			ComPtr<CIDXGIFactory> factory,
			HWND hwnd);

		virtual void Render() = 0;
		virtual void Terminate();

	protected:
		ComPtr<CID3D12Device> Device;
		ComPtr<CIDXGIFactory> Factory;
		ComPtr<CIDXGISwapChain> SwapChain;
		D3D12GraphicsCommand Command;
		D3D12RenderTarget SwapChainRenderTarget;
		uint32 FrameCount;
		uint32 FrameIndex;

	private:
		void CreateSwapChain(HWND hwnd);
	};
}
