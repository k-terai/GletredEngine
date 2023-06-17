// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include "RenderModule/Src/DirectX/D3D12Common.h"
#include "CoreModule/Inc/Singleton.h"


namespace GletredEngine
{
	class D3D12Manager : public Singleton<D3D12Manager>
	{
	public:
		void Initialize(bool useWarpDevice);
		void Terminate();

		ComPtr<CID3D12Device> GetDevice()
		{
			return Device;
		}

		ComPtr<CIDXGIFactory> GetFactory()
		{
			return Factory;
		}

	private:
		friend class  Singleton<D3D12Manager>; //Access GetInstance
		D3D12Manager();
		~D3D12Manager() override;

		void SetDebugLayer();
		void CreateFactory();
		void CreateDevice(bool useWarpDevice);
		void GetHardwareAdapter(CIDXGIFactory* factory, CIDXGIAdapter** adapter, bool requestHighPerformanceAdapter = false) const;

		uint32 DxgiFactoryFlags;
		ComPtr<CID3D12Device> Device;
		ComPtr<CIDXGIFactory> Factory;
	};
}
