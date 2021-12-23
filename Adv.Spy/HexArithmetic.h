#pragma once

#include <string>

namespace Hex
{
	int8_t Read8(char** buffer);
	int16_t Read16(char** buffer);
	int32_t Read32(char** buffer);
	std::string ReadString(char** buffer);

	bool StartsSame(char* a, char* b, int size);
}
