// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#include "CoreModule/Inc/Resource.h"

using namespace GletredEngine;
using namespace std;


Resource::Resource() : IsInitialized(false)
{

}

Resource::~Resource()
{
	IsInitialized = false;
}