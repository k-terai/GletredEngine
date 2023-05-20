// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "CoreModule/Inc/GObject.h"
#include "CoreModule/Inc/Hasher.h"

using namespace std;
using namespace GletredEngine;

GObject::GObject() : UniqueId(C_INVALID_UNIQUE_ID)
{
	UniqueId = Fnv1Hash32(reinterpret_cast<uint8*>(this), 64);
}

GObject::~GObject()
{
	UniqueId = C_INVALID_UNIQUE_ID;
}