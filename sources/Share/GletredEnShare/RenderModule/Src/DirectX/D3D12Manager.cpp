// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Src/DirectX/D3D12Manager.h"
#include "RenderModule/Src/DirectX/D3D12Helper.h"

using namespace std;
using namespace GletredEngine;


D3D12Manager::D3D12Manager() : DxgiFactoryFlags(0), Factory(nullptr), Device(nullptr)
{

}

D3D12Manager::~D3D12Manager()
{
	Factory = nullptr;
	Device = nullptr;
	DxgiFactoryFlags = 0;
}

void D3D12Manager::Initialize(const bool useWarpDevice)
{
	SetDebugLayer();
	CreateFactory();
	CreateDevice(useWarpDevice);
}

void D3D12Manager::Terminate()
{
	Factory.Reset();
	Device.Reset();
}

void D3D12Manager::SetDebugLayer()
{

#if defined(_DEBUG)
	{
		ComPtr<ID3D12Debug> debugController;
		if (SUCCEEDED(D3D12GetDebugInterface(IID_PPV_ARGS(&debugController))))
		{
			debugController->EnableDebugLayer();
			DxgiFactoryFlags |= DXGI_CREATE_FACTORY_DEBUG;
		}
	}
#endif
}

void D3D12Manager::CreateFactory()
{
	const auto result = CreateDXGIFactory2(DxgiFactoryFlags, IID_PPV_ARGS(&Factory));
	SetExceptionIfFailed(result);
}

void D3D12Manager::CreateDevice(const bool useWarpDevice)
{
	HRESULT result;

	if (useWarpDevice)
	{
		ComPtr<IDXGIAdapter> warpAdapter;
		result = Factory->EnumWarpAdapter(IID_PPV_ARGS(&warpAdapter));
		if (!SetExceptionIfFailed(result))
		{
			return;
		}

		result = D3D12CreateDevice(warpAdapter.Get(), D3D_FEATURE_LEVEL_11_0, IID_PPV_ARGS(&Device));
		SetExceptionIfFailed(result);
	}
	else
	{
		ComPtr<IDXGIAdapter1> hardwareAdapter;
		GetHardwareAdapter(Factory.Get(), &hardwareAdapter);

		result = D3D12CreateDevice(hardwareAdapter.Get(), D3D_FEATURE_LEVEL_11_0, IID_PPV_ARGS(&Device));
		SetExceptionIfFailed(result);
	}
}

void D3D12Manager::GetHardwareAdapter(CIDXGIFactory* factory, CIDXGIAdapter** adapter, const bool requestHighPerformanceAdapter) const
{
	*adapter = nullptr;
	ComPtr<IDXGIAdapter1> tempAdapter;
	ComPtr<IDXGIFactory6> factory6;

	if (SUCCEEDED(factory->QueryInterface(IID_PPV_ARGS(&factory6))))
	{
		for (
			UINT adapterIndex = 0;
			SUCCEEDED(factory6->EnumAdapterByGpuPreference(
				adapterIndex,
				requestHighPerformanceAdapter == true ? DXGI_GPU_PREFERENCE_HIGH_PERFORMANCE : DXGI_GPU_PREFERENCE_UNSPECIFIED,
				IID_PPV_ARGS(&tempAdapter)));
			++adapterIndex)
		{
			DXGI_ADAPTER_DESC1 desc;
			tempAdapter->GetDesc1(&desc);

			//TODO: Software option.
			if (desc.Flags & DXGI_ADAPTER_FLAG_SOFTWARE)
			{
				continue;
			}

			// NOTE:Just check driver supports d3d12.
			if (SUCCEEDED(D3D12CreateDevice(tempAdapter.Get(), D3D_FEATURE_LEVEL_11_0, _uuidof(ID3D12Device), nullptr)))
			{
				break;
			}
		}
	}

	if (tempAdapter.Get() == nullptr)
	{
		for (UINT adapterIndex = 0; SUCCEEDED(factory->EnumAdapters1(adapterIndex, &tempAdapter)); ++adapterIndex)
		{
			DXGI_ADAPTER_DESC1 desc;
			tempAdapter->GetDesc1(&desc);

			if (desc.Flags & DXGI_ADAPTER_FLAG_SOFTWARE)
			{
				//TODO: Software option.
				continue;
			}

			// NOTE:Just check driver supports d3d12.
			if (SUCCEEDED(D3D12CreateDevice(tempAdapter.Get(), D3D_FEATURE_LEVEL_11_0, _uuidof(ID3D12Device), nullptr)))
			{
				break;
			}
		}
	}

	*adapter = tempAdapter.Detach();
}

