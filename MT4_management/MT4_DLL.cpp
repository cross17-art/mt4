#include "pch.h"
#include "MT4_man.h"
#include <winsock.h>
#include <Windows.h>
#include "MT4ManagerAPI.h"
#include <fstream>
using namespace std;

typedef INT(CALLBACK* LPFNDLLVERSIOIN)();
typedef INT(CALLBACK* LPFNDLLCREATE)(int version, CManagerInterface** obj);

HINSTANCE hDLL;               // Handle to DLL
LPFNDLLVERSIOIN lpfnDllVersion;    // Function pointer
LPFNDLLCREATE lpfnDllCreate;

CManagerInterface* manager;
ConSymbol conSymbols;
ConGroup conGroup;

void Stop()
{
	FreeLibrary(hDLL);
}
bool Start()	//	возвращает, получилось ли ...
{
	bool result = false;
	hDLL = LoadLibraryA("mtmanapi64.dll");
	if (hDLL == NULL) return result;
	lpfnDllVersion = (LPFNDLLVERSIOIN)GetProcAddress(hDLL, "MtManVersion");
	lpfnDllCreate = (LPFNDLLCREATE)GetProcAddress(hDLL, "MtManCreate");
	if (!lpfnDllVersion)
	{
		FreeLibrary(hDLL);		// handle the error
	}
	else
	{
		// call the function
		int uReturnVal = lpfnDllVersion();
		int res = lpfnDllCreate(uReturnVal, &manager);
		result = (res == 1);
	}
	return result;
}
int Disconect()	
{
	int res = manager->Disconnect();
	return res;
}

void MemFree()	
{
	manager->MemFree(manager);
}

int Release()
{
	int res = manager->Release();
	return res;
}

LPCSTR ErrorDescription(int num)
{
	return manager->ErrorDescription(num);
}
int Connect(LPCSTR str)
{
	return manager->Connect(str);
}
int Login(const int login, LPCSTR password)
{
	return manager->Login(login, password);
}

void writeToFile(string str)
{
	ofstream fout("log_file.txt", ios_base::app);
	fout << str << "\n";
	fout.close();
}

void writeToFile(long long str)
{
	ofstream fout("log_file.txt", ios_base::app);

	fout << str << "\n";
	fout.close();
}

void* GroupsRequest(int* total)
{
	writeToFile("GroupsRequest");

	ConGroup* conGroup = manager->GroupsRequest(total);
	writeToFile(sizeof(conGroup[0]));
	writeToFile(sizeof(conGroup[1]));
	writeToFile(sizeof(conGroup[2]));
	writeToFile(sizeof(conGroup[3]));
	writeToFile(sizeof(conGroup[4]));
	writeToFile("END GroupsRequest");
	return conGroup;
}

void* SymbolsGetAll(int* total)
{
	writeToFile(*total);

	ConSymbol* consymbol = manager->SymbolsGetAll(total);

	writeToFile(*total);
	writeToFile((long long)consymbol);
	return consymbol;
}

int CfgUpdateSymbol(const void* conSymbol)
{
	if (conSymbol == nullptr) {
		return 2412;
	}

	ConSymbol* conSymbolEnd = (ConSymbol*)conSymbol;
	int res = manager->CfgUpdateSymbol(conSymbolEnd);
	return res;
}


int SymbolsRefresh()
{
	return manager->SymbolsRefresh();
}

int Ping()
{
	return manager->Ping();
}

void WorkingDirectory(LPCSTR path)
{
	manager->WorkingDirectory(path);
}


