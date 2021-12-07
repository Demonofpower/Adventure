#include "pch.h"
#include <Windows.h>
#include "winsock2.h"

#include <iostream>
#include <ostream>

#include "Util.h"

#define ANSI_COLOR_RED     "\x1b[31m"
#define ANSI_COLOR_GREEN   "\x1b[32m"
#define ANSI_COLOR_YELLOW  "\x1b[33m"
#define ANSI_COLOR_BLUE    "\x1b[34m"
#define ANSI_COLOR_MAGENTA "\x1b[35m"
#define ANSI_COLOR_CYAN    "\x1b[36m"
#define ANSI_COLOR_RESET   "\x1b[0m"

uintptr_t moduleBase = (uintptr_t)GetModuleHandle(L"PwnAdventure3-Win32-Shipping.exe");
uintptr_t ws2_32_moduleBase = (uintptr_t)GetModuleHandle(L"WS2_32.DLL");


typedef int(__stdcall* sendFunc)(SOCKET socket, char* buffer, int len, int flags);
sendFunc hSendFunc;
SOCKET socketFunctionCall;
int __stdcall SendFunc(SOCKET socket, char* buffer, int len, int flags)
{
	socketFunctionCall = socket;
	
	for (int i = 0; i < len; ++i)
	{
		printf("%02X ", (BYTE)buffer[i]);
	}
	printf("\n");
	
	return hSendFunc(socket, buffer, len, flags);
}

typedef int(__stdcall* recvFunc)(SOCKET socket, char* buffer, int len, int flags);
recvFunc hRecvFunc;
SOCKET socketFunctionRecv;
int __stdcall RecvFunc(SOCKET socket, char* buffer, int len, int flags)
{
	socketFunctionRecv = socket;

	for (int i = 0; i < len; ++i)
	{
		printf("%02X ", (BYTE)buffer[i]);
	}
	printf("\n");

	return hRecvFunc(socket, buffer, len, flags);
}

DWORD WINAPI HackThread(HMODULE hModule)
{
	FILE* f = 0;
	AllocConsole();
	freopen_s(&f, "CONOUT$", "w", stdout);

	std::cout << "Working" << std::endl;
	
	std::cout << std::hex;
	std::cout << moduleBase << std::endl;
	std::cout << ws2_32_moduleBase << std::endl;	
	std::cout << std::dec;
		

	while (true)
	{
		if (GetAsyncKeyState(VK_F1) & 1)
		{
			std::cout << "hooked!" << std::endl;;
			hSendFunc = (sendFunc)(GetProcAddress(GetModuleHandleA("ws2_32.dll"), "send"));
			hSendFunc = (sendFunc)Util::TrampolineHook((BYTE*)hSendFunc, (BYTE*)SendFunc, 5);

			hRecvFunc = (recvFunc)(GetProcAddress(GetModuleHandleA("ws2_32.dll"), "recv"));
			hRecvFunc = (recvFunc)Util::TrampolineHook((BYTE*)hRecvFunc, (BYTE*)RecvFunc, 5);
		}
		if (GetAsyncKeyState(VK_F12) & 1)
		{
			break;
		}
		Sleep(10);
	}

	fclose(f);
	FreeConsole();
	FreeLibraryAndExitThread(hModule, 0);
}

BOOL APIENTRY DllMain(HMODULE hModule,
                      DWORD ul_reason_for_call,
                      LPVOID lpReserved
)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		CloseHandle(CreateThread(nullptr, 0, (LPTHREAD_START_ROUTINE)HackThread, hModule, 0, nullptr));
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}
