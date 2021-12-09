#include "pch.h"
#include "TrampolineHook.h"

#include "Util.h"

TrampolineHook::TrampolineHook(BYTE** toHook, BYTE* ourFunc, int len)
{
	initialized = true;

	this->toHook = *toHook;
	this->ourFunc = ourFunc;
	this->ourFuncToGateway = toHook;
	this->len = len;

	Enable();
}

void TrampolineHook::Enable()
{
	if (!initialized || hooked)
	{
		return;
	}

	hooked = true;
	gateway = Util::TrampolineHook(toHook, ourFunc, len);

	*ourFuncToGateway = gateway;
}

void TrampolineHook::Disable()
{
	if(!initialized || !hooked)
	{
		return;
	}

	hooked = false;

	Util::Patch(toHook, gateway, len);
}