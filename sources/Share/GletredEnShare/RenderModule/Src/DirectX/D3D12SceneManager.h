// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include "CoreModule/Inc/Singleton.h"
#include "RenderModule/Src/DirectX/D3D12Common.h"
#include "CoreModule/Inc/NonCopyable.h"
#include "RenderModule/Src/DirectX/D3D12SceneRenderer.h"
#include <memory>
#include <vector>


namespace GletredEngine
{
	class D3D12SceneManager : public Singleton<D3D12SceneManager>
	{
	public:
		D3D12SceneManager();
		virtual ~D3D12SceneManager();

		void Initialize(ComPtr<CID3D12Device> device, ComPtr<CIDXGIFactory> factory,bool supportFullScreen);
		void Terminate();
		void CreateSceneRenderer(HWND hwnd);

		void Update();

	private:
		friend class  Singleton<D3D12SceneManager>; //Access GetInstance

		bool SupportFullScreen;
		ComPtr<CID3D12Device> Device;
		ComPtr<CIDXGIFactory> Factory;
		std::vector<std::shared_ptr<D3D12SceneRenderer>> SceneRendererVector;
	};
}
