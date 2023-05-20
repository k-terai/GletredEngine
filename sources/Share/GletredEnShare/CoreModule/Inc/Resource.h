// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include"NonCopyable.h"

namespace GletredEngine
{
	class Resource : public NonCopyable
	{
	public:
		Resource();
		~Resource() override;

		bool IsInitialize() const
		{
			return IsInitialized;
		}

	protected:
		bool IsInitialized;
	};
}