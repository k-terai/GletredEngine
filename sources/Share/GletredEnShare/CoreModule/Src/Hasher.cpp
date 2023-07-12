// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.


#include "CoreModule/Inc/Hasher.h"
#include "CoreModule/Inc/Platform.h"

namespace GletredEngine
{
	uint32 Fnv1Hash32(const void* data, const size_t size)
	{
		constexpr uint32 FnvOffsetBasis = 0x811C9DC5;
		constexpr uint32 FnvPrime = 0x01000193;

		auto bytes = static_cast<const uint8*>(data);
		uint32_t hash = FnvOffsetBasis;

		for (size_t i = 0; i < size; ++i) 
		{

			hash ^= bytes[i];
			hash *= FnvPrime;
		}

		return hash;
	}

	uint64 Fnv1Hash64(const void* data, const size_t size)
	{
		constexpr uint64 FnvPrime = 0x00000100000001B3;
		constexpr uint64 FnvOffsetBasis = 0xcbf29ce484222325;

		auto bytes = static_cast<const uint8*>(data);
		uint64 hash = FnvOffsetBasis;

		for (size_t i = 0; i < size; ++i)
		{
			hash ^= bytes[i];
			hash *= FnvPrime;
		}

		return hash;
	}
}
