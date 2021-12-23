#include "pch.h"
#include "HexArithmetic.h"

namespace Hex
{
	int8_t Read8(char** buffer)
	{
		char* localBuffer = *buffer;
		*buffer += 1;
		return (int8_t)*localBuffer;
	}

	int16_t Read16(char** buffer)
	{
		char* localBuffer = *buffer;
		*buffer += 2;
		return (int16_t) *localBuffer;
	}

	int32_t Read32(char** buffer)
	{
		char* localBuffer = *buffer;
		*buffer += 4;
		return (int32_t)*localBuffer;
	}

	std::string ReadString(char** buffer)
	{
		char* localBuffer = *buffer;
		const int16_t stringLength = (int16_t)*localBuffer;
		localBuffer += 2;

		std::string readString(localBuffer, stringLength);

		*buffer += 2 + stringLength;

		return readString;
	}

	bool StartsSame(char* a, char* b, int size)
	{
		for (int i = 0; i < size; ++i)
		{
			if(a[i] != b[i])
			{
				return false;
			}
		}

		return true;
	}
}
