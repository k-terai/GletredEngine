// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Src/DirectX/D3D12MeshRenderer.h"
#include "RenderModule/Src/DirectX/D3D12Shader.h"
#include "RenderModule/Src/DirectX/D3D12MeshBase.h"
#include "RenderModule/Src/DirectX/D3D12Helper.h"
#include "RenderModule/Src/DirectX/D3D12ResourceManager.h"

using namespace std;
using namespace GletredEngine;

D3D12MeshRenderer::D3D12MeshRenderer() : Device(nullptr), Mesh(nullptr), Material(nullptr), RootSignature(nullptr),
PipelineState(nullptr)
{

}

D3D12MeshRenderer::~D3D12MeshRenderer()
{
	Device = nullptr;
	Mesh = nullptr;
	Material = nullptr;
	RootSignature = nullptr;
	PipelineState = nullptr;
}

void D3D12MeshRenderer::Initialize(const uniqueid meshId, const uniqueid materialId)
{
	Mesh = D3D12ResourceManager::GetInstance()->GetResource(meshId);
	Material = D3D12ResourceManager::GetInstance()->GetResource(materialId);

	CreateSamplerRootSignature();
}

void D3D12MeshRenderer::Render(const D3D12GraphicsCommand* command)
{
	command->SetGraphicsRootSignature(RootSignature.Get());

	const auto mat = GetMaterial();
	const auto mesh = GetMesh();

	if (mat->IsAnyTexture())
	{
		const auto texture = mat->GetTexture(0);
		command->SetDescriptorHeaps(texture->GetDescriptorHeap());
		command->SetGraphicsRootDescriptorTable(texture->GetDescriptorHeap());
	}

	command->SetPipelineState(PipelineState.Get());
	command->SetPrimitiveTopology(D3D_PRIMITIVE_TOPOLOGY_TRIANGLELIST);
	command->SetVertexBuffers(mesh->GetVertexBufferView());
	command->DrawInstanced(mesh->GetVertexCount(), 1);
}


void D3D12MeshRenderer::CreateEmptyRootSignature()
{
	CD3DX12_ROOT_SIGNATURE_DESC rootSignatureDesc;
	SecureZeroMemory(&rootSignatureDesc, sizeof(CD3DX12_ROOT_SIGNATURE_DESC));

	rootSignatureDesc.Init(0, nullptr, 0, nullptr, D3D12_ROOT_SIGNATURE_FLAG_ALLOW_INPUT_ASSEMBLER_INPUT_LAYOUT);

	ComPtr<ID3DBlob> signature;
	ComPtr<ID3DBlob> error;

	HRESULT result = D3D12SerializeRootSignature(&rootSignatureDesc, D3D_ROOT_SIGNATURE_VERSION_1, &signature, &error);
	SetExceptionIfFailed(result);

	result = Device->CreateRootSignature(0, signature->GetBufferPointer(), signature->GetBufferSize(), IID_PPV_ARGS(&RootSignature));
	SetExceptionIfFailed(result);
}

void D3D12MeshRenderer::CreateSamplerRootSignature()
{
	D3D12_FEATURE_DATA_ROOT_SIGNATURE featureData;
	featureData.HighestVersion = D3D_ROOT_SIGNATURE_VERSION_1_1;

	if (FAILED(Device->CheckFeatureSupport(D3D12_FEATURE_ROOT_SIGNATURE, &featureData, sizeof(featureData))))
	{
		featureData.HighestVersion = D3D_ROOT_SIGNATURE_VERSION_1_0;
	}

	CD3DX12_DESCRIPTOR_RANGE1 ranges[1];
	ranges[0].Init(D3D12_DESCRIPTOR_RANGE_TYPE_SRV, 1, 0, 0, D3D12_DESCRIPTOR_RANGE_FLAG_DATA_STATIC);

	CD3DX12_ROOT_PARAMETER1 rootParameters[1];
	rootParameters[0].InitAsDescriptorTable(1, &ranges[0], D3D12_SHADER_VISIBILITY_PIXEL);

	D3D12_STATIC_SAMPLER_DESC sampler = {};
	SecureZeroMemory(&sampler, sizeof(D3D12_STATIC_SAMPLER_DESC));
	sampler.Filter = D3D12_FILTER_MIN_MAG_MIP_POINT;
	sampler.AddressU = D3D12_TEXTURE_ADDRESS_MODE_BORDER;
	sampler.AddressV = D3D12_TEXTURE_ADDRESS_MODE_BORDER;
	sampler.AddressW = D3D12_TEXTURE_ADDRESS_MODE_BORDER;
	sampler.MipLODBias = 0;
	sampler.MaxAnisotropy = 0;
	sampler.ComparisonFunc = D3D12_COMPARISON_FUNC_NEVER;
	sampler.BorderColor = D3D12_STATIC_BORDER_COLOR_TRANSPARENT_BLACK;
	sampler.MinLOD = 0.0f;
	sampler.MaxLOD = D3D12_FLOAT32_MAX;
	sampler.ShaderRegister = 0;
	sampler.RegisterSpace = 0;
	sampler.ShaderVisibility = D3D12_SHADER_VISIBILITY_PIXEL;

	CD3DX12_VERSIONED_ROOT_SIGNATURE_DESC rootSignatureDesc;
	rootSignatureDesc.Init_1_1(_countof(rootParameters), rootParameters, 1, &sampler, D3D12_ROOT_SIGNATURE_FLAG_ALLOW_INPUT_ASSEMBLER_INPUT_LAYOUT);

	ComPtr<ID3DBlob> signature;
	ComPtr<ID3DBlob> error;
	auto result = D3DX12SerializeVersionedRootSignature(&rootSignatureDesc, featureData.HighestVersion, &signature, &error);
	SetExceptionIfFailed(result);

	result = Device->CreateRootSignature(0, signature->GetBufferPointer(), signature->GetBufferSize(), IID_PPV_ARGS(&RootSignature));
	SetExceptionIfFailed(result);
}

void D3D12MeshRenderer::CreateGraphicsPipelineState()
{
	D3D12_GRAPHICS_PIPELINE_STATE_DESC psoDesc = {};
	SecureZeroMemory(&psoDesc, sizeof(D3D12_GRAPHICS_PIPELINE_STATE_DESC));

	const auto shader = reinterpret_cast<D3D12Shader*>(Material.get());
	const auto mesh = reinterpret_cast<D3D12MeshBase*>(Mesh.get());

	psoDesc.InputLayout = mesh->GetInputLayoutDesc();
	psoDesc.pRootSignature = RootSignature.Get();
	psoDesc.VS = shader->GetVertexByteCode();
	psoDesc.PS = shader->GetPixelByteCode();
	psoDesc.RasterizerState = CD3DX12_RASTERIZER_DESC(D3D12_DEFAULT);
	psoDesc.BlendState = CD3DX12_BLEND_DESC(D3D12_DEFAULT);
	psoDesc.DepthStencilState.DepthEnable = FALSE;
	psoDesc.DepthStencilState.StencilEnable = FALSE;
	psoDesc.SampleMask = UINT_MAX;
	psoDesc.PrimitiveTopologyType = D3D12_PRIMITIVE_TOPOLOGY_TYPE_TRIANGLE;
	psoDesc.NumRenderTargets = 1;
	psoDesc.RTVFormats[0] = DXGI_FORMAT_R8G8B8A8_UNORM;
	psoDesc.SampleDesc.Count = 1;

	const HRESULT result = Device->CreateGraphicsPipelineState(&psoDesc, IID_PPV_ARGS(&PipelineState));
	Logger::IsFailureLog(result);
}




