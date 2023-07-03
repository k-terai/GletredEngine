// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include "CoreModule/Inc/Platform.h"
#include "CoreModule/Inc/Hasher.h"

namespace GletredEngine
{
	struct BuildInResource
	{
		ctstring VsPositionColorPath = _T("VSPositionColor.cso");
		ctstring PsPositionColorPath = _T("PSPositionColor.cso");
		uniqueid PositionColorId = C_INVALID_UNIQUE_ID;

		ctstring VsCheckerBoardPath = _T("VSCheckerBoardShader.cso");
		ctstring PsCheckerBoardPath = _T("PSCheckerBoardShader.cso");
		uniqueid CheckerBoardId = C_INVALID_UNIQUE_ID;

		uniqueid TriangleMeshId = C_INVALID_UNIQUE_ID;

		void GenerateHash()
		{
			//PositionColor
			{
				tchar temp[] = _T("EDD60CAA-FF0D-4332-814D-7BDDB90D5ABC)");
				PositionColorId = Fnv1Hash32(temp);
			}

			//CheckerBoard
			{
				tchar temp[] = _T("1AEF0098-5979-4E39-B921-7F8093FD40C7");
				CheckerBoardId = Fnv1Hash32(temp);
			}

			// Triangle
			{
				tchar temp[] = _T("125E17D6-4249-4107-A7CF-4EB8E7623537");
				TriangleMeshId = Fnv1Hash32(temp);
			}

		}
	};
}
