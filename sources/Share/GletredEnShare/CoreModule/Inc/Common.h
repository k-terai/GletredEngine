// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include"Platform.h"
#include<functional>

namespace GletredEngine
{
	constexpr uint32 C_INVALID_UNIQUE_ID = 0;

	enum class LogType
	{
		Info,
		Warning,
		Error,
		Exception
	};

	enum class RenderType
	{
		Forward,
		Deferred
	};

	enum class ShaderType
	{
		Vertex,
		Pixel
	};

	enum class BuildInShaderType
	{
		None,

		/**
		 * \brief See TestShader1
		 */
		TestShader1,

		/**
		 * \brief See CheckerBoardShader
		 */
		CheckerBoard
	};

	enum class BuildInMeshType
	{
		None,

		/**
		 * \brief Basic triangle
		 */
		TrianglePositionColor,

		/**
		 * \brief Triangle with position and texture
		 */
		TrianglePositionTexture
	};

	enum class BuildInTextureType
	{
		None,

		/**
		* \brief Checker board
		*/
		CheckerBoard
	};

	using LogCallback = std::function<void(const LogType logType, const tchar* const buffer)>;
}