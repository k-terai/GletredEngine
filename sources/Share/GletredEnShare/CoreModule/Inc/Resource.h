// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include"NonCopyable.h"
#include <assert.h>

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

		uniqueid GetResourceId() const
		{
			return ResourceId;
		}

		void SetResourceId(const uniqueid id)
		{
			assert(ResourceId == C_INVALID_UNIQUE_ID);
			ResourceId = id;
		}

	protected:
		uniqueid ResourceId;
		bool IsInitialized;
	};
}