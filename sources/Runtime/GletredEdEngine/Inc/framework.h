// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once
#include "CoreModule/Inc/Platform.h"

#ifdef GLETREDEDENGINE_EXPORTS
#define GLETREDEDENGINE_API __declspec(dllexport)
#else
#define GLETREDEDENGINE_API __declspec(dllimport)
#endif

EXTERN_C GLETREDEDENGINE_API void Initialize();

EXTERN_C GLETREDEDENGINE_API void Startup();

EXTERN_C GLETREDEDENGINE_API void Shutdown();

EXTERN_C GLETREDEDENGINE_API void Update();

EXTERN_C GLETREDEDENGINE_API void Terminate();


/*
 * Render Module
 */
EXTERN_C GLETREDEDENGINE_API void CreateScene(HWND hwnd);