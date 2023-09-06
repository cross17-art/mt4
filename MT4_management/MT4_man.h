#pragma once

#define ENGINELIBRARY_EXPORTS

#ifdef ENGINELIBRARY_EXPORTS
#define ENGINELIBRARY_API __declspec(dllexport)
#else
#define ENGINELIBRARY_API __declspec(dllimport)
#endif


extern "C" ENGINELIBRARY_API bool Start();
extern "C" ENGINELIBRARY_API void Stop();
extern "C" ENGINELIBRARY_API LPCSTR ErrorDescription(int num);
extern "C" ENGINELIBRARY_API int Release();
extern "C" ENGINELIBRARY_API int Connect(LPCSTR str);
extern "C" ENGINELIBRARY_API int Disconect();
extern "C" ENGINELIBRARY_API void MemFree();
extern "C" ENGINELIBRARY_API int Login(const int login, LPCSTR password);
extern "C" ENGINELIBRARY_API int Ping();
extern "C" ENGINELIBRARY_API void WorkingDirectory(LPCSTR path);
extern "C" ENGINELIBRARY_API void* SymbolsGetAll(int* total);
extern "C" ENGINELIBRARY_API int SymbolsRefresh();
extern "C" ENGINELIBRARY_API int CfgUpdateSymbol(const void* cfg);
extern "C" ENGINELIBRARY_API void* GroupsRequest(int* total);

