#include "framework.h"

namespace Util
{
	void Patch(void* dst, void* patch, int size)
	{
		DWORD oldprotect;
		VirtualProtect(dst, size, PAGE_EXECUTE_READWRITE, &oldprotect);
		memcpy(dst, patch, size);
		VirtualProtect(dst, size, oldprotect, &oldprotect);
	}

	void Nop(void* dst, unsigned int size)
	{
		DWORD oldprotect;
		VirtualProtect(dst, size, PAGE_EXECUTE_READWRITE, &oldprotect);
		memset(dst, 0x90, size);
		VirtualProtect(dst, size, oldprotect, &oldprotect);
	}

	bool Hook(void* toHook, void* ourFunc, int len)
	{
		if (len < 5)
		{
			return false;
		}

		DWORD curProtection;
		VirtualProtect(toHook, len, PAGE_EXECUTE_READWRITE, &curProtection);

		memset(toHook, 0x90, len);

		DWORD relativeAddress = ((DWORD)ourFunc - (DWORD)toHook) - 5;

		*(BYTE*)toHook = 0xE9;
		*(DWORD*)((DWORD)toHook + 1) = relativeAddress;

		DWORD temp;
		VirtualProtect(toHook, len, curProtection, &temp);

		return true;
	}

	BYTE* TrampolineHook(BYTE* toHook, BYTE* ourFunc, int len)
	{
		BYTE* gateway = (BYTE*)VirtualAlloc(0, len + 5, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);

		memcpy_s(gateway, len, toHook, len);

		DWORD relativeAddress = ((DWORD)toHook - (DWORD)gateway) - 5;
		*(gateway + len) = 0xE9;
		*(DWORD*)((DWORD)gateway + len + 1) = relativeAddress;

		Hook(toHook, ourFunc, len);

		return gateway;
	}
}
