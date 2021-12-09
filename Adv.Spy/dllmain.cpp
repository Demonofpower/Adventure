#include "pch.h"
#include <Windows.h>
#include "WinSock2.h"

#include <iostream>
#include <ostream>


#include "PacketReverser.h"
#include "TrampolineHook.h"
#include "Util.h"

#define ANSI_COLOR_RED     "\x1b[31m"
#define ANSI_COLOR_GREEN   "\x1b[32m"
#define ANSI_COLOR_YELLOW  "\x1b[33m"
#define ANSI_COLOR_BLUE    "\x1b[34m"
#define ANSI_COLOR_MAGENTA "\x1b[35m"
#define ANSI_COLOR_CYAN    "\x1b[36m"
#define ANSI_COLOR_RESET   "\x1b[0m"

#pragma comment(lib, "Ws2_32.lib")

uintptr_t moduleBase = (uintptr_t)GetModuleHandle(L"PwnAdventure3-Win32-Shipping.exe");
uintptr_t ws2_32_moduleBase = (uintptr_t)GetModuleHandle(L"WS2_32.DLL");


SOCKET socketMaster;
SOCKET socketGame;
typedef int (__stdcall* sendFunc)(SOCKET socket, char* buffer, int len, int flags);
sendFunc hSendFunc;

int __stdcall SendFunc(SOCKET socket, char* buffer, int len, int flags)
{
	if (!socketGame)
	{
		if (!socketMaster)
		{
			socketMaster = socket;
		}
		else
		{
			socketGame = socket;
		}
	}

	if (socket == socketMaster)
	{
		printf("[Master]");
	}
	else
	{
		printf("[Game]");
	}

	printf(" <-- ");

	PacketReverser::Reverse(buffer, len);

	printf("\n");

	return hSendFunc(socket, buffer, len, flags);
}

typedef int (__stdcall* recvFunc)(SOCKET socket, char* buffer, int len, int flags);
recvFunc hRecvFunc;

int __stdcall RecvFunc(SOCKET socket, char* buffer, int len, int flags)
{
	if (socket == socketMaster)
	{
		printf("[Master]");
	}
	else
	{
		printf("[Game]");
	}

	printf(" --> ");

	PacketReverser::Reverse(buffer, len);

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

	bool socketHooksEnabled = false;
	hSendFunc = (sendFunc)(GetProcAddress(GetModuleHandleA("ws2_32.dll"), "send"));
	TrampolineHook sendHook((BYTE**)&hSendFunc, (BYTE*)SendFunc, 5);
	hRecvFunc = (recvFunc)(GetProcAddress(GetModuleHandleA("ws2_32.dll"), "recv"));
	TrampolineHook recvHook((BYTE**)&hRecvFunc, (BYTE*)RecvFunc, 5);
	while (true)
	{
		if (GetAsyncKeyState(VK_F1) & 1)
		{

			if (!socketHooksEnabled)
			{
				std::cout << "hooked!" << std::endl;
				sendHook.Enable();
				recvHook.Enable();
			}
			else
			{
				std::cout << "restored!" << std::endl;;
				sendHook.Disable();
				recvHook.Disable();
			}
			socketHooksEnabled = !socketHooksEnabled;
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
