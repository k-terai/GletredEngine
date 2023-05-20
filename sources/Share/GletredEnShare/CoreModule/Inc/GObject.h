// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include "Common.h"
#include "Platform.h"

namespace GletredEngine
{
	class GObject
	{
	public:
		GObject();
		virtual ~GObject();

		uniqueid GetUniqueId() const
		{
			return UniqueId;
		}

	private:
		uniqueid UniqueId;
	};
}