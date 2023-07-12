// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

#include "CoreModule/Inc/Platform.h"
#include "CoreModule/Inc/Hasher.h"

namespace GletredEngine
{
	struct D3D12BuildInResource
	{
		ctstring VsPositionColorPath = _T("VSPositionColor.cso");
		ctstring PsPositionColorPath = _T("PSPositionColor.cso");
		uniqueid PositionColorId = C_INVALID_UNIQUE_ID;
		uniqueid PositionColorDefaultMaterialId = C_INVALID_UNIQUE_ID;

		ctstring VsCheckerBoardPath = _T("VSCheckerBoardShader.cso");
		ctstring PsCheckerBoardPath = _T("PSCheckerBoardShader.cso");
		uniqueid CheckerBoardId = C_INVALID_UNIQUE_ID;


		uniqueid TriangleMeshId = C_INVALID_UNIQUE_ID;

		uniqueid CheckerBoardTextureId = C_INVALID_UNIQUE_ID;

		void GenerateHash()
		{
			// PositionColor shader
			{
				tstring temp = _T("EDD60CAA-FF0D-4332-814D-7BDDB90D5ABC");
				size_t size = 0;

#if UNICODE
				size = wcslen(temp.c_str());
#else
				size = strlen(temp.c_str());
#endif

				PositionColorId = Fnv1Hash32(temp.c_str(), size);
			}

			// PositionColor material
			{
				tstring temp = _T("4DC6FD02-A01A-4239-AE24-75CE90FC1014");
				size_t size = 0;

#if UNICODE
				size = wcslen(temp.c_str());
#else
				size = strlen(temp.c_str());
#endif


				PositionColorDefaultMaterialId = Fnv1Hash32(temp.c_str(), size);
			}

			// CheckerBoard shader
			{
				tstring temp = _T("1AEF0098-5979-4E39-B921-7F8093FD40C7");
				size_t size = 0;

#if UNICODE
				size = wcslen(temp.c_str());
#else
				size = strlen(temp.c_str());
#endif

				CheckerBoardId = Fnv1Hash32(temp.c_str(), size);
			}

			// Triangle shader
			{
				tstring temp = _T("125E17D6-4249-4107-A7CF-4EB8E7623537");
				size_t size = 0;

#if UNICODE
				size = wcslen(temp.c_str());
#else
				size = strlen(temp.c_str());
#endif

				TriangleMeshId = Fnv1Hash32(temp.c_str(), size);
			}

			// CheckerBoard texture
			{
				tstring temp = _T("6A50F17B-E1DC-4DFB-A851-27554B9EF684");
				size_t size = 0;

#if UNICODE
				size = wcslen(temp.c_str());
#else
				size = strlen(temp.c_str());
#endif

				CheckerBoardTextureId = Fnv1Hash32(temp.c_str(), size);
			}
		}
	};
}
