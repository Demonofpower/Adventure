#pragma once
#include <string>

class Packet
{
public:
	std::string name;
	char* id;
	char* length;

	Packet(std::string name, char* id)
	{
		this->name = name;
		this->id = id;
	}
	
	void Print(std::string packet, bool silent);
};

