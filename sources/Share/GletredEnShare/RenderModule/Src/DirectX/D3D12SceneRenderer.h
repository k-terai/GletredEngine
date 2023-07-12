// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include "CoreModule/Inc/NonCopyable.h"
#include "RenderModule/Src/DirectX/D3D12Common.h"
#include "RenderModule/Src/DirectX/D3D12RenderTarget.h"
#include "RenderModule/Src/DirectX/D3D12GraphicsCommand.h"
#include "RenderModule/Src/DirectX/D3D12Camera.h"
#include "RenderModule/Src/DirectX/D3D12MeshRenderer.h"

namespace GletredEngine
{
	class D3D12SceneRenderer : public NonCopyable
	{
	public:
		D3D12SceneRenderer();
		~D3D12SceneRenderer() override;
		void Initialize(ComPtr<CID3D12Device> device,
			ComPtr<CIDXGIFactory> factory,
			WindowHandle hwnd);

		virtual void Render() = 0;
		virtual void Terminate();

		std::weak_ptr<D3D12Camera> CreateCamera(int32 priority, int32 width, int32 height);
		std::weak_ptr<D3D12MeshRenderer> CreateMeshRenderer(uniqueid meshId, uniqueid materialId);

	protected:
		D3D12Camera* GetHightestPriorityCamera() const;

		ComPtr<CID3D12Device> Device;
		ComPtr<CIDXGIFactory> Factory;
		ComPtr<CIDXGISwapChain> SwapChain;
		D3D12GraphicsCommand Command;
		D3D12RenderTarget SwapChainRenderTarget;
		uint32 FrameCount;
		uint32 FrameIndex;
		std::vector<std::shared_ptr<D3D12Camera>> Cameras;
		std::vector<std::shared_ptr<D3D12MeshRenderer>> MeshRenderers;

	private:
		void CreateSwapChain(HWND hwnd);
	};
}
