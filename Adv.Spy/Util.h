#pragma once
namespace Util
{
	void Patch(void* dst, void* patch, int size);
	void Nop(void* dst, unsigned int size);
	bool Hook(void* toHook, void* ourFunc, int len);
	BYTE* TrampolineHook(BYTE* toHook, BYTE* ourFunc, int len);
}
