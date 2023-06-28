// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include "CoreModule/Inc/Macro.h"
#include "CoreModule/Inc/Platform.h"

namespace GletredEngine
{
	struct D3D12ShaderObject
	{
		void* Ptr; //.cso pointer
		int32  Size;

		D3D12ShaderObject() : Ptr(nullptr), Size(-1)
		{

		}

		bool IsEnable() const
		{
			return Ptr != nullptr && Size > 0;
		}

		void Release()
		{
			SAFE_DELETE(Ptr);
			Size = -1;
		}

	};
}