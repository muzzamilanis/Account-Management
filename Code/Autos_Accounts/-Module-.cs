using System;
using System.Reflection;
using System.Runtime.InteropServices;

// Token: 0x02000001 RID: 1
internal class <Module>
{
	// Token: 0x06000001 RID: 1 RVA: 0x00020048 File Offset: 0x0001CA48
	static <Module>()
	{
		<Module>.\u202E\u202B\u206C\u202A\u202A\u206C\u200D\u206C\u206C\u200D\u206F\u200E\u200F\u206D\u200B\u202C\u206B\u202C\u200D\u200B\u202E\u200C\u202C\u206A\u200E\u202A\u202E\u202B\u202D\u200B\u200C\u200F\u206C\u206D\u200E\u206C\u206D\u202B\u200F\u206A\u202E();
	}

	// Token: 0x06000002 RID: 2
	[DllImport("kernel32.dll", EntryPoint = "VirtualProtect")]
	internal static extern bool \u206F\u202B\u206C\u200C\u206A\u200E\u202B\u200E\u206A\u206A\u206C\u200E\u206E\u206C\u206B\u200D\u200D\u202B\u206B\u200D\u200D\u206A\u200B\u206B\u202D\u202A\u202B\u202E\u202A\u202A\u202E\u202E\u200E\u202C\u202A\u202A\u202C\u200E\u202B\u206F\u202E(IntPtr, uint, uint, ref uint);

	// Token: 0x06000003 RID: 3 RVA: 0x00020050 File Offset: 0x0001CA50
	internal unsafe static void \u202E\u202B\u206C\u202A\u202A\u206C\u200D\u206C\u206C\u200D\u206F\u200E\u200F\u206D\u200B\u202C\u206B\u202C\u200D\u200B\u202E\u200C\u202C\u206A\u200E\u202A\u202E\u202B\u202D\u200B\u200C\u200F\u206C\u206D\u200E\u206C\u206D\u202B\u200F\u206A\u202E()
	{
		Module module = typeof(<Module>).Module;
		string fullyQualifiedName = module.FullyQualifiedName;
		bool flag = fullyQualifiedName.Length > 0 && fullyQualifiedName[0] == '<';
		byte* ptr = (byte*)((void*)Marshal.GetHINSTANCE(module));
		byte* ptr2 = ptr + *(uint*)(ptr + 60);
		ushort num = *(ushort*)(ptr2 + 6);
		ushort num2 = *(ushort*)(ptr2 + 20);
		uint* ptr3 = null;
		uint num3 = 0U;
		uint* ptr4 = (uint*)(ptr2 + 24 + num2);
		uint num4 = 2037012335U;
		uint num5 = 2896466330U;
		uint num6 = 3176681001U;
		uint num7 = 2247335079U;
		for (int i = 0; i < (int)num; i++)
		{
			uint num8 = *(ptr4++) * *(ptr4++);
			if (num8 == 2179404347U)
			{
				ptr3 = (uint*)(ptr + (flag ? ptr4[3] : ptr4[1]) / 4U);
				num3 = (flag ? ptr4[2] : (*ptr4)) >> 2;
			}
			else if (num8 != 0U)
			{
				uint* ptr5 = (uint*)(ptr + (flag ? ptr4[3] : ptr4[1]) / 4U);
				uint num9 = ptr4[2] >> 2;
				for (uint num10 = 0U; num10 < num9; num10 += 1U)
				{
					uint num11 = (num4 ^ *(ptr5++)) + num5 + num6 * num7;
					num4 = num5;
					num5 = num7;
					num7 = num11;
				}
			}
			ptr4 += 8;
		}
		uint[] array = new uint[16];
		uint[] array2 = new uint[16];
		for (int j = 0; j < 16; j++)
		{
			array[j] = num7;
			array2[j] = num5;
			num4 = (num5 >> 5 | num5 << 27);
			num5 = (num6 >> 3 | num6 << 29);
			num6 = (num7 >> 7 | num7 << 25);
			num7 = (num4 >> 11 | num4 << 21);
		}
		array[0] = (array[0] ^ array2[0]);
		array[1] = array[1] * array2[1];
		array[2] = array[2] + array2[2];
		array[3] = (array[3] ^ array2[3]);
		array[4] = array[4] * array2[4];
		array[5] = array[5] + array2[5];
		array[6] = (array[6] ^ array2[6]);
		array[7] = array[7] * array2[7];
		array[8] = array[8] + array2[8];
		array[9] = (array[9] ^ array2[9]);
		array[10] = array[10] * array2[10];
		array[11] = array[11] + array2[11];
		array[12] = (array[12] ^ array2[12]);
		array[13] = array[13] * array2[13];
		array[14] = array[14] + array2[14];
		array[15] = (array[15] ^ array2[15]);
		uint num12 = 64U;
		<Module>.\u206F\u202B\u206C\u200C\u206A\u200E\u202B\u200E\u206A\u206A\u206C\u200E\u206E\u206C\u206B\u200D\u200D\u202B\u206B\u200D\u200D\u206A\u200B\u206B\u202D\u202A\u202B\u202E\u202A\u202A\u202E\u202E\u200E\u202C\u202A\u202A\u202C\u200E\u202B\u206F\u202E((IntPtr)((void*)ptr3), num3 << 2, num12, ref num12);
		if (num12 == 64U)
		{
			return;
		}
		uint num13 = 0U;
		for (uint num14 = 0U; num14 < num3; num14 += 1U)
		{
			*ptr3 ^= array[(int)((UIntPtr)(num13 & 15U))];
			array[(int)((UIntPtr)(num13 & 15U))] = (array[(int)((UIntPtr)(num13 & 15U))] ^ *(ptr3++)) + 1035675673U;
			num13 += 1U;
		}
	}
}
