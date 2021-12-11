#include "pch.h"
#include "HexArithmetic.h"

namespace Hex
{
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
