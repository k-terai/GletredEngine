// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

#pragma once

namespace GletredEngine
{

#define SAFE_DELETE(p)       { if (p) { delete (p);     (p)=NULL; } }
#define SAFE_DELETE_ARRAY(p) { if (p) { delete[] (p);   (p)=NULL; } }
#define SAFE_RELEASE(p)      { if (p) { (p)->Release(); (p)=NULL; } }
#define SAFE_ADDREF(p)      { if (p) { (p)->AddRef(); } }
#define ARRAY_SIZE(_arr_) (sizeof(_arr_)/sizeof((_arr_)[0]))


#define DISALLOW_COPY_AND_ASSIGN(TypeName) \
private: \
TypeName(const TypeName&)=delete; \
void operator=(const TypeName&)=delete;

#define REGISTER_OBJECT_RTTI_HEADER(TypeName) \
public: \
virtual ctstring const ClassName() {return UNIQUE_RTTI.GetClassName();}  \
protected: \
const virtual Rtti& GetRtti() const {return UNIQUE_RTTI;} \
private: \
static const Rtti UNIQUE_RTTI;

#define REGISTER_OBJECT_RTTI_SOURCE(TypeName) \
const Rtti TypeName::UNIQUE_RTTI(_T("\"TypeName\""));

#ifdef _DEBUG
#   define CUSTOM_OUTPUT_DEBUG_STRING( str, ... ) \
      { \
        TCHAR c[2048]; \
        _stprintf_s( c, str, __VA_ARGS__ ); \
        OutputDebugString( c ); \
      }
#else
#    define CUSTOM_OUTPUT_DEBUG_STRING( str, ... )
#endif

#define PROPERTY(...) \

#define SINGLEON_BEHAVIOUR(TypeName) \
public: \
static TypeName* const GetInstance(){static TypeName instance; return &instance; } \

}