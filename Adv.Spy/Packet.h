#pragma once
#include <string>

class Packet
{
public:
	std::string name;
	char* id;
	int idSize;

	Packet(std::string name, char* id, int idSize)
	{
		this->name = name;
		this->id = id;
		this->idSize = idSize;
	}
	
	void Print(char* buffer, int size, bool silent = false);
};