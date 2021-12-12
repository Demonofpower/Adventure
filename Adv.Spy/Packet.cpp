#include "pch.h"
#include "Packet.h"

#include <fstream>
#include <iostream>

struct HexCharStruct
{
	unsigned char c;
	HexCharStruct(unsigned char _c) : c(_c) { }
};

inline std::ostream& operator<<(std::ostream& o, const HexCharStruct& hs)
{
	return (o << std::hex << (int)hs.c);
}

inline HexCharStruct hex(unsigned char _c)
{
	return HexCharStruct(_c);
}

//void Packet::Print(char* buffer, int size, bool silent)
//{
//	if (silent)
//	{
//		std::cout << name << std::endl;
//		return;
//	}
//
//	/*FILE* fptr;
//	fopen_s(&fptr, "C:\\Users\\Juli\\Desktop\\src\\Adventure\\Debug\\Sessions\\a.txt", "a");
//	
//	for (int i = 0; i < size; ++i)
//	{
//		printf("%02X ", (BYTE)buffer[i]);
//
//		fprintf(fptr, "%02X", (BYTE)buffer[i]);
//	}
//
//	fprintf(fptr, "\n");
//
//	fclose(fptr);*/
//}