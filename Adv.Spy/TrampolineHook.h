#pragma once
class TrampolineHook
{
public:
	bool initialized;
	bool hooked;

	int len;
	BYTE* toHook;
	BYTE* ourFunc;
	BYTE* gateway;
	BYTE** ourFuncToGateway;
	
	TrampolineHook(BYTE** toHook, BYTE* ourFunc, int len);
	
	void Enable();
	void Disable();
};

