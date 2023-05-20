// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "Hasher.h"

namespace GletredEngine
{
	uint32 Fnv1Hash32(ptstring string)
	{
		auto hval = C_FNV_BASIS_32;
		auto current = reinterpret_cast<uint8*>(string);

		while (*current != 0)
		{
			hval = hval + (hval << 1) + (hval << 4) + (hval << 7) + (hval << 8) + (hval << 24);
			hval = hval ^ static_cast<uint32>(*current);
			++current;
		}

		return hval;
	}

	uint32 Fnv1Hash32(const uint8* const bytes, const size_t length)
	{
		auto hval = C_FNV_BASIS_32;
		const auto current = bytes;

		for (size_t i = 0; i < length; i++)
		{
			hval = hval + (hval << 1) + (hval << 4) + (hval << 7) + (hval << 8) + (hval << 24);
			hval = hval ^ static_cast<uint32>(current[i]);
		}

		return hval;
	}

	uint64 Fnv1Hash64(ptstring string)
	{
		auto hval = C_FNV_BASIS_64;
		auto current = reinterpret_cast<uint8*>(string);

		while (*current != 0)
		{
			hval = hval + (hval << 1) + (hval << 4) + (hval << 5) + (hval << 7) + (hval << 8) + (hval << 40);
			hval = hval ^ static_cast<uint32>(*current);
			++current;
		}

		return hval;
	}

	uint64 Fnv1Hash64(const uint8* const bytes, const size_t length)
	{
		auto hval = C_FNV_BASIS_64;
		const auto current = bytes;

		for (size_t i = 0; i < length; i++)
		{
			hval = hval + (hval << 1) + (hval << 4) + (hval << 5) + (hval << 7) + (hval << 8) + (hval << 40);
			hval = hval ^ static_cast<uint64>(current[i]);
		}

		return hval;
	}

	uint32 Fnv1HashLowercase32(ptstring string)
	{
		auto hval = C_FNV_BASIS_32;
		auto current = reinterpret_cast<unsigned char*>(string);

		while (*current != 0)
		{
			auto val = *current;
			if (val >= 'A' && val <= 'Z')
			{
				val += 'a' - 'A';
			}
			hval = hval + (hval << 1) + (hval << 4) + (hval << 7) + (hval << 8) + (hval << 24);
			hval = hval ^ static_cast<uint32>(val);
			++current;
		}

		return hval;
	}

	uint64 Fnv1HashLowercase64(ptstring string)
	{
		auto hval = C_FNV_BASIS_64;
		auto current = reinterpret_cast<unsigned char*>(string);

		while (*current != 0)
		{
			auto val = *current;
			if (val >= 'A' && val <= 'Z')
			{
				val += 'a' - 'A';
			}
			hval = hval + (hval << 1) + (hval << 4) + (hval << 5) + (hval << 7) + (hval << 8) + (hval << 40);
			hval = hval ^ static_cast<uint64>(val);
			++current;
		}

		return hval;
	}
}