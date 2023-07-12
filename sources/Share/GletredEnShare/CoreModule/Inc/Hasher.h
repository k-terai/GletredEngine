// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include"Platform.h"

namespace GletredEngine
{
	static constexpr uint32_t C_FNV_BASIS_32 = 2166136261U;
	static constexpr uint64_t C_FNV_BASIS_64 = 14695981039346656037U;
	static constexpr uint32_t C_FNV_PRIME_32 = 16777619U;
	static constexpr uint64_t C_FNV_PRIME_64 = 1099511628211LLU;

	uint32 Fnv1Hash32(const void* data, size_t size);
	uint64 Fnv1Hash64(const void* data, size_t size);
}