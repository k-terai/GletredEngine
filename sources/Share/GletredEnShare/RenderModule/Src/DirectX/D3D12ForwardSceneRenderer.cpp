// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Src/DirectX/D3D12ForwardSceneRenderer.h"

using namespace std;
using namespace GletredEngine;

D3D12ForwardSceneRenderer::D3D12ForwardSceneRenderer()
{

}

D3D12ForwardSceneRenderer::~D3D12ForwardSceneRenderer()
{

}

void D3D12ForwardSceneRenderer::Render()
{
	TestRender();
}

void D3D12ForwardSceneRenderer::TestRender()
{
	Command.Reset();
	Command.SetViewportsAndScissorRects(GetHightestPriorityCamera());
	Command.TransitionPresentToRenderTarget(SwapChainRenderTarget);
	Command.SetRenderTargets(SwapChainRenderTarget);
	Command.ClearRenderTargetView(SwapChainRenderTarget);

	for(auto& r : MeshRenderers)
	{
		r->Render(&Command);
	}

	Command.TransitionRenderTargetToPresent(SwapChainRenderTarget);
	Command.Close();
	Command.Execute();
	Command.PresentAndWait();
}

