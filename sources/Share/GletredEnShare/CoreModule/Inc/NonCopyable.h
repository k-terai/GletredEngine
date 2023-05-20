// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include"GObject.h"

namespace GletredEngine
{
	class NonCopyable : public GObject
	{
	protected:
		NonCopyable();
		~NonCopyable() override;

	private:
		NonCopyable(const NonCopyable&) = delete;
		NonCopyable& operator=(const NonCopyable&) = delete;
	};
}