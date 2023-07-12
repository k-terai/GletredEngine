// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "RenderModule/Src/DirectX/D3D12Camera.h"

using namespace  std;
using namespace  GletredEngine;

D3D12Camera::D3D12Camera() : Priority(-1), Viewport(), ScissorRect()
{

}

D3D12Camera::~D3D12Camera()
{

}

void D3D12Camera::Initialize(const int32 priority, const float width, const float height)
{
	SetPriority(priority);
	Resize(width, height);
}

void D3D12Camera::Resize(const float width, const float height)
{
	ScissorRect.top = 0;
	ScissorRect.left = 0;
	ScissorRect.right = ScissorRect.left + static_cast<LONG>(width);
	ScissorRect.bottom = ScissorRect.top + static_cast<LONG>(height);

	Viewport.Width = width;
	Viewport.Height = height;
	Viewport.MinDepth = 0;
	Viewport.MaxDepth = 1.0f;
	Viewport.TopLeftX = 0;
	Viewport.TopLeftY = 0;
}



