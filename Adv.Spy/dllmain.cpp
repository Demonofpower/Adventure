#include "pch.h"

#pragma warning( disable : 4996)
#pragma comment(lib, "Ws2_32.lib")

#include <Windows.h>
#include "WinSock2.h"

#include <iostream>
#include <ostream>
#include <vector>


#include "PacketChecker.h"
#include "PacketReverser.h"
#include "TrampolineHook.h"

uintptr_t moduleBase = (uintptr_t)GetModuleHandle(L"PwnAdventure3-Win32-Shipping.exe");
uintptr_t logicBase = (uintptr_t)GetModuleHandle(L"GameLogic.dll");
uintptr_t ws2_32_moduleBase = (uintptr_t)GetModuleHandle(L"WS2_32.DLL");
uintptr_t ssleay32_moduleBase = (uintptr_t)GetModuleHandle(L"SSLEAY32.DLL");

PacketReverser reverser;

SOCKET socketMaster;
SOCKET socketGame;

Type HandleSocket(SOCKET socket)
{
	struct sockaddr_in local_address;
	int addr_size = sizeof(local_address);
	getpeername(socket, (struct sockaddr*)&local_address, &addr_size);

	char* connected_ip = inet_ntoa(local_address.sin_addr);
	int port = ntohs(local_address.sin_port);

	if (port == 3333)
	{
		if (!socketMaster)
		{
			socketMaster = socket;
		}

		return MASTER;
	}
	else
	{
		if (!socketGame)
		{
			socketGame = socket;
		}

		return GAME;
	}
}

void DebugPrint(char* buffer, Type t, int sr)
{
	if (t == MASTER)
	{
		return;
	}

	if (*((WORD*)buffer) == *(WORD*)"\x6D\x76")
	{
		return;
	}
	if (*((WORD*)buffer) == *(WORD*)"\x52\x48")
	{
		return;
	}

	if (sr == 1)
	{
		printf("S ");
	}
	else
	{
		printf("R ");
	}
}

typedef int (__stdcall* sendFunc)(SOCKET socket, char* buffer, int len, int flags);
sendFunc hSendFunc;

int __stdcall SendFunc(SOCKET socket, char* buffer, int len, int flags)
{
	Type currType = HandleSocket(socket);
	
	if (currType != MASTER)
	{
		PacketChecker::Check(buffer, len, SEND, currType);
	}
	
	/*DebugPrint(buffer, currType, 1);

	reverser.Print(buffer, len, SEND, currType);*/

	return hSendFunc(socket, buffer, len, flags);
}

typedef int (__stdcall* recvFunc)(SOCKET socket, char* buffer, int len, int flags);
recvFunc hRecvFunc;

int __stdcall RecvFunc(SOCKET socket, char* buffer, int len, int flags)
{
	Type currType = HandleSocket(socket);

	if (currType != MASTER)
	{
		PacketChecker::Check(buffer, len, RECV, currType);
	}
	
	/*DebugPrint(buffer, currType, 2);

	reverser.Print(buffer, len, RECV, currType);*/

	return hRecvFunc(socket, buffer, len, flags);
}

typedef int (__cdecl* sslReadFunc)(void* ssl, void* buf, int num);
sslReadFunc hSslReadFunc;

int __cdecl SslReadFunc(void* ssl, void* buf, int num)
{
	int result = hSslReadFunc(ssl, buf, num);

	PacketChecker::ProcessMasterPacket((char*)buf, num, RECV);
	
	return result;
}

typedef int(__cdecl* sslWriteFunc)(void* ssl, void* buf, int num);
sslReadFunc hSslWriteFunc;

int __cdecl SslWriteFunc(void* ssl, void* buf, int num)
{
	PacketChecker::ProcessMasterPacket((char*)buf, num, SEND);
	return hSslWriteFunc(ssl, buf, num);
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

	Pipe::CreatePipe();

	bool socketHooksEnabled = false;
	hSendFunc = (sendFunc)(GetProcAddress(GetModuleHandleA("ws2_32.dll"), "send"));
	TrampolineHook sendHook((BYTE**)&hSendFunc, (BYTE*)SendFunc, 5);
	hRecvFunc = (recvFunc)(GetProcAddress(GetModuleHandleA("ws2_32.dll"), "recv"));
	TrampolineHook recvHook((BYTE**)&hRecvFunc, (BYTE*)RecvFunc, 5);

	hSslReadFunc = (sslReadFunc)(GetProcAddress(GetModuleHandleA("ssleay32.dll"), "SSL_read"));
	TrampolineHook sslReadHook((BYTE**)&hSslReadFunc, (BYTE*)SslReadFunc, 8);
	hSslWriteFunc = (sslWriteFunc)(GetProcAddress(GetModuleHandleA("ssleay32.dll"), "SSL_write"));
	TrampolineHook sslWriteHook((BYTE**)&hSslWriteFunc, (BYTE*)SslWriteFunc, 8);

	while (true)
	{
		if (GetAsyncKeyState(VK_F1) & 1)
		{
			if (!socketHooksEnabled)
			{
				std::cout << "hooked!" << std::endl;
				sendHook.Enable();
				recvHook.Enable();
				sslReadHook.Enable();
				sslWriteHook.Enable();
			}
			else
			{
				std::cout << "restored!" << std::endl;;
				sendHook.Disable();
				recvHook.Disable();
				sslReadHook.Disable();
				sslWriteHook.Disable();
			}
			socketHooksEnabled = !socketHooksEnabled;
		}
		if (GetAsyncKeyState(VK_F12) & 1)
		{
			break;
		}
		if (GetAsyncKeyState(VK_F2) & 1)
		{
			system("cls");
		}
		Sleep(10);
	}

	if (socketHooksEnabled)
	{
		sendHook.Disable();
		recvHook.Disable();
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
