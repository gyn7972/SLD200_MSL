using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
	public static class BytesConverter
	{
		/// <summary>
		/// 부호 비트를 검출하기 위한 마스크입니다.
		/// </summary>
		public const byte SignBitMask = 128;

		/// <summary>
		/// 부호 없는 8비트 정수를 부호 있는 8비트 정수로 변환합니다.
		/// </summary>
		/// <param name="value">부호 없는 8비트 정수입니다.</param>
		/// <returns>부호 있는 8비트 정수입니다.</returns>
		public static sbyte ToSByte(byte value)
		{
			return (sbyte)value;
		}

		/// <summary>
		/// 부호 있는 8비트 정수를 부호 없는 8비트 정수로 변환합니다.
		/// </summary>
		/// <param name="value">부호 있는 8비트 정수입니다.</param>
		/// <returns>부호 없는 8비트 정수입니다.</returns>
		public static byte ToByte(sbyte value)
		{
			return (byte)value;
		}

		/// <summary>
		/// 부호 있는 8비트 정수의 배열을 부호 없는 8비트 정수의 배열로 변환합니다.
		/// </summary>
		/// <param name="values">부호 있는 8비트 정수의 배열입니다.</param>
		/// <returns>부호 없는 8비트 정수의 배열입니다.</returns>
		public static byte[] ToBytes(sbyte[] values)
		{
			byte[] array = new byte[values.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ToByte(values[i]);
			}
			
			return array;
		}

		/// <summary>
		/// 지정된 바이트 배열의 처음 두 바이트를 부호 있는 16비트 정수로 변환합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <returns>부호 있는 16비트 정수입니다.</returns>
		public static short ToInt16(byte[] bytes)
		{
			return ToInt16(bytes, 0);
		}

		/// <summary>
		/// 지정된 바이트 배열의 지정된 인덱스부터 두 바이트를 사용해서 부호 있는 16비트 정수로 변환합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <param name="index">시작 인덱스를 지정합니다.</param>
		/// <returns>부호 있는 16비트 정수입니다.</returns>
		public static short ToInt16(byte[] bytes, int index)
		{
			return (short)(((bytes[index] << 8) & 0xFF00) + bytes[index + 1]);
		}

		/// <summary>
		/// 부호 있는 16비트 정수를 바이트 배열로 변환합니다.
		/// </summary>
		/// <param name="value">부호 있는 16비트 정수입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytes(short value)
		{
			return new byte[2]
			{
				(byte)((uint)(value >> 8) & 0xFFu),
				(byte)((uint)value & 0xFFu)
			};
		}

		/// <summary>
		/// 부호 있는 16비트 정수의 배열을 바이트 배열로 변환합니다.
		/// </summary>
		/// <param name="values">부호 있는 16비트 정수의 배열입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytes(short[] values)
		{
			byte[] array = new byte[0];
			for (int i = 0; i < values.Length; i++)
			{
				array = Concat(array, ToBytes(values[i]));
			}
			return array;
			
		}

		/// <summary>
		/// Converts a signed short value to a byte array.
		/// </summary>
		/// <param name="value">A signed short value to convert.</param>
		/// <param name="bytes">A byte array to save the converting result.</param>
		/// <param name="index">The first index number of byte array to start to save.</param>
		/// <returns>Returns the saved count of bytes.</returns>
		public static int ToBytes(short value, byte[] bytes, int index)
		{
			byte[] array = new byte[2];
			array = ToBytes(value);
			array.CopyTo(bytes, index);
			return Marshal.SizeOf(typeof(short));
		}

		/// <summary>
		/// 지정된 바이트 배열의 처음 두 바이트를 부호 없는 16비트 정수로 변환합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <returns>부호 없는 16비트 정수입니다.</returns>
		public static ushort ToUint16(byte[] bytes)
		{
			return ToUint16(bytes, 0);
		}

		/// <summary>
		/// 지정된 바이트 배열의 지정된 인덱스부터 두 바이트를 사용해서 부호 없는 16비트 정수로 변환합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <param name="index">시작 인덱스를 지정합니다.</param>
		/// <returns>부호 없는 16비트 정수입니다.</returns>
		public static ushort ToUint16(byte[] bytes, int index)
		{
			return (ushort)(((bytes[index] << 8) & 0xFF00) + bytes[index + 1]);
		}

		/// <summary>
		/// 부호 없는 16비트 정수를 바이트 배열로 변환합니다.
		/// </summary>
		/// <param name="value">부호 없는 16비트 정수입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytes(ushort value)
		{
			return new byte[2]
			{
				(byte)((uint)(value >> 8) & 0xFFu),
				(byte)(value & 0xFFu)
			};
		}

		/// <summary>
		/// 부호 없는 16비트 정수의 배열을 바이트 배열로 변환합니다.
		/// </summary>
		/// <param name="values">부호 없는 16비트 정수의 배열입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytes(ushort[] values)
		{
			byte[] array = new byte[0];
			for (int i = 0; i < values.Length; i++)
			{
				array = Concat(array, ToBytes(values[i]));
			}
			return array;
		}

		/// <summary>
		/// Converts an unsigned short value to a byte array.
		/// </summary>
		/// <param name="value">An unsigned short value to convert.</param>
		/// <param name="bytes">A byte array to save the converting result.</param>
		/// <param name="index">The first index number of byte array to start to save.</param>
		/// <returns>Returns the saved count of bytes.</returns>
		public static int ToBytes(ushort value, byte[] bytes, int index)
		{
			byte[] array = new byte[2];
			array = ToBytes(value);
			array.CopyTo(bytes, index);
			return Marshal.SizeOf(typeof(ushort));
		}

		/// <summary>
		/// 지정된 바이트 배열의 처음 네 바이트를 부호 있는 32비트 정수로 변환합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <returns>부호 있는 32비트 정수입니다.</returns>
		public static int ToInt32(byte[] bytes)
		{
			return ToInt32(bytes, 0);
		}

		/// <summary>
		/// 지정된 바이트 배열의 지정된 인덱스부터 네 바이트를 사용해서 부호 있는 32비트 정수로 변환합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <param name="index">시작 인덱스를 지정합니다.</param>
		/// <returns>부호 있는 32비트 정수입니다.</returns>
		public static int ToInt32(byte[] bytes, int index)
		{
			return (int)((bytes[index] << 24) & 0xFF000000u) + ((bytes[index + 1] << 16) & 0xFF0000) + ((bytes[index + 2] << 8) & 0xFF00) + bytes[index + 3];
		}

		/// <summary>
		/// 부호 있는 32비트 정수를 바이트 배열로 변환합니다.
		/// </summary>
		/// <param name="value">부호 있는 32비트 정수입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytes(int value)
		{
			return new byte[4]
			{
				(byte)((uint)(value >> 24) & 0xFFu),
				(byte)((uint)(value >> 16) & 0xFFu),
				(byte)((uint)(value >> 8) & 0xFFu),
				(byte)((uint)value & 0xFFu)
			};
		}

		/// <summary>
		/// 부호 있는 32비트 정수의 배열을 바이트 배열로 변환합니다.
		/// </summary>
		/// <param name="values">부호 있는 32비트 정수의 배열입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytes(int[] values)
		{
			byte[] array = new byte[0];
			for (int i = 0; i < values.Length; i++)
			{
				array = Concat(array, ToBytes(values[i]));
			}
			return array;
		}

		/// <summary>
		/// Converts a signed integer to a byte array.
		/// </summary>
		/// <param name="value">A signed integer to convert.</param>
		/// <param name="bytes">A byte array to save the converting result.</param>
		/// <param name="index">The first index number of byte array to start to save.</param>
		/// <returns>Returns the saved count of bytes.</returns>
		public static int ToBytes(int value, byte[] bytes, int index)
		{
			byte[] array = new byte[4];
			array = ToBytes(value);
			array.CopyTo(bytes, index);
			return Marshal.SizeOf(typeof(int));
		}

		/// <summary>
		/// 지정된 바이트 배열의 처음 네 바이트를 부호 없는 32비트 정수로 변환합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <returns>부호 없는 32비트 정수입니다.</returns>
		public static uint ToUint32(byte[] bytes)
		{
			return ToUint32(bytes, 0);
		}

		/// <summary>
		/// 지정된 바이트 배열의 지정된 인덱스부터 네 바이트를 사용해서 부호 없는 32비트 정수로 변환합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <param name="index">시작 인덱스를 지정합니다.</param>
		/// <returns>부호 없는 32비트 정수입니다.</returns>
		public static uint ToUint32(byte[] bytes, int index)
		{
			return (uint)((int)((bytes[index] << 24) & 0xFF000000u) + ((bytes[index + 1] << 16) & 0xFF0000) + ((bytes[index + 2] << 8) & 0xFF00) + bytes[index + 3]);
		}

		/// <summary>
		/// 부호 없는 32비트 정수를 바이트 배열로 변환합니다.
		/// </summary>
		/// <param name="value">부호 없는 32비트 정수입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytes(uint value)
		{
			return new byte[4]
			{
				(byte)((value >> 24) & 0xFFu),
				(byte)((value >> 16) & 0xFFu),
				(byte)((value >> 8) & 0xFFu),
				(byte)(value & 0xFFu)
			};
		}

		/// <summary>
		/// 부호 없는 32비트 정수의 배열을 바이트 배열로 변환합니다.
		/// </summary>
		/// <param name="values">부호 없는 32비트 정수의 배열입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytes(uint[] values)
		{
			byte[] array = new byte[0];
			for (int i = 0; i < values.Length; i++)
			{
				array = Concat(array, ToBytes(values[i]));
			}
			return array;
		}

		/// <summary>
		/// Converts an unsigned integer to a byte array.
		/// </summary>
		/// <param name="value">An unsigned integer to convert.</param>
		/// <param name="bytes">A byte array to save the converting result.</param>
		/// <param name="index">The first index number of byte array to start to save.</param>
		/// <returns>Returns the saved count of bytes.</returns>
		public static int ToBytes(uint value, byte[] bytes, int index)
		{
			byte[] array = new byte[4];
			array = ToBytes(value);
			array.CopyTo(bytes, index);
			return Marshal.SizeOf(typeof(uint));
		}

		/// <summary>
		/// 지정된 바이트 배열의 처음 여덟 바이트를 부호 있는 64비트 정수로 변환합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <returns>부호 있는 64비트 정수입니다.</returns>
		public static long ToInt64(byte[] bytes)
		{
			return ToInt64(bytes, 0);
		}

		/// <summary>
		/// 지정된 바이트 배열의 지정된 인덱스부터 여덟 바이트를 사용해서 부호 있는 64비트 정수로 변환합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <param name="index">시작 인덱스를 지정합니다.</param>
		/// <returns>부호 있는 64비트 정수입니다.</returns>
		public static long ToInt64(byte[] bytes, int index)
		{
			return (long)((((ulong)bytes[index] << 56) & 0xFF00000000000000uL) + (((ulong)bytes[index + 1] << 48) & 0xFF000000000000L) + (((ulong)bytes[index + 2] << 40) & 0xFF0000000000L) + (((ulong)bytes[index + 3] << 32) & 0xFF00000000L) + (((ulong)bytes[index + 4] << 24) & 0xFF000000u) + (((ulong)bytes[index + 5] << 16) & 0xFF0000) + (((ulong)bytes[index + 6] << 8) & 0xFF00) + ((ulong)bytes[index + 7] & 0xFFuL));
		}

		/// <summary>
		/// 부호 있는 64비트 정수를 바이트 배열로 변환합니다.
		/// </summary>
		/// <param name="value">부호 있는 64비트 정수입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytes(long value)
		{
			return new byte[8]
			{
				(byte)((value >> 56) & 0xFF),
				(byte)((value >> 48) & 0xFF),
				(byte)((value >> 40) & 0xFF),
				(byte)((value >> 32) & 0xFF),
				(byte)((value >> 24) & 0xFF),
				(byte)((value >> 16) & 0xFF),
				(byte)((value >> 8) & 0xFF),
				(byte)(value & 0xFF)
			};
		}

		/// <summary>
		/// 부호 있는 64비트 정수의 배열을 바이트 배열로 변환합니다.
		/// </summary>
		/// <param name="values">부호 있는 64비트 정수의 배열입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytes(long[] values)
		{
			byte[] array = new byte[0];
			for (int i = 0; i < values.Length; i++)
			{
				array = Concat(array, ToBytes(values[i]));
			}
			return array;
		}

		/// <summary>
		/// Converts a signed long value to a byte array.
		/// </summary>
		/// <param name="value">A signed long value to convert.</param>
		/// <param name="bytes">A byte array to save the converting result.</param>
		/// <param name="index">The first index number of byte array to start to save.</param>
		/// <returns>Returns the saved count of bytes.</returns>
		public static int ToBytes(long value, byte[] bytes, int index)
		{
			byte[] array = new byte[8];
			array = ToBytes(value);
			array.CopyTo(bytes, index);
			return Marshal.SizeOf(typeof(long));
		}

		/// <summary>
		/// 지정된 바이트 배열의 처음 여덟 바이트를 부호 없는 64비트 정수로 변환합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <returns>부호 없는 64비트 정수입니다.</returns>
		public static ulong ToUint64(byte[] bytes)
		{
			return ToUint64(bytes, 0);
		}

		/// <summary>
		/// 지정된 바이트 배열의 지정된 인덱스부터 여덟 바이트를 사용해서 부호 없는 64비트 정수로 변환합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <param name="index">시작 인덱스를 지정합니다.</param>
		/// <returns>부호 없는 64비트 정수입니다.</returns>
		public static ulong ToUint64(byte[] bytes, int index)
		{
			return (ulong)(((bytes[index] << 24) & -72057594037927936L) + ((bytes[index + 1] << 16) & 0xFF000000000000L) + ((bytes[index + 2] << 8) & 0xFF0000000000L) + ((int)bytes[index + 3] & 0xFF00000000L) + ((bytes[index + 4] << 24) & 0xFF000000u) + ((bytes[index + 5] << 16) & 0xFF0000) + ((bytes[index + 6] << 8) & 0xFF00) + bytes[index + 7]);
		}

		/// <summary>
		/// 부호 없는 64비트 정수를 바이트 배열로 변환합니다.
		/// </summary>
		/// <param name="value">부호 없는 64비트 정수입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytes(ulong value)
		{
			return new byte[8]
			{
				(byte)((value >> 56) & 0xFF),
				(byte)((value >> 48) & 0xFF),
				(byte)((value >> 40) & 0xFF),
				(byte)((value >> 32) & 0xFF),
				(byte)((value >> 24) & 0xFF),
				(byte)((value >> 16) & 0xFF),
				(byte)((value >> 8) & 0xFF),
				(byte)(value & 0xFF)
			};
		}

		/// <summary>
		/// 부호 없는 64비트 정수의 배열을 바이트 배열로 변환합니다.
		/// </summary>
		/// <param name="values">부호 없는 64비트 정수의 배열입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytes(ulong[] values)
		{
			byte[] array = new byte[0];
			for (int i = 0; i < values.Length; i++)
			{
				array = Concat(array, ToBytes(values[i]));
			}
			return array;
		}

		/// <summary>
		/// Converts an unsigned long value to a byte array.
		/// </summary>
		/// <param name="value">An unsigned long value to convert.</param>
		/// <param name="bytes">A byte array to save the converting result.</param>
		/// <param name="index">The first index number of byte array to start to save.</param>
		/// <returns>Returns the saved count of bytes.</returns>
		public static int ToBytes(ulong value, byte[] bytes, int index)
		{
			byte[] array = new byte[8];
			array = ToBytes(value);
			array.CopyTo(bytes, index);
			return Marshal.SizeOf(typeof(ulong));
		}

		/// <summary>
		/// 지정된 바이트 배열의 처음 여덟 바이트를 배정밀도 부동 소수점 숫자로 변환합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <returns>배정밀도 부동 소수점 숫자입니다.</returns>
		public static double ToDouble(byte[] bytes)
		{
			return ToDouble(bytes, 0);
		}

		/// <summary>
		/// 지정된 바이트 배열의 지정된 인덱스부터 여덟 바이트를 사용해서 배정밀도 부동 소수점 숫자로 변환합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <param name="index">시작 인덱스를 지정합니다.</param>
		/// <returns>배정밀도 부동 소수점 숫자입니다.</returns>
		public static double ToDouble(byte[] bytes, int index)
		{
			byte[] array = new byte[8];
			Array.Copy(bytes, index, array, 0, array.Length);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(array);
			}
			return BitConverter.ToDouble(array, 0);
		}

		/// <summary>
		/// 배정밀도 부동 소수점 숫자를 바이트 배열로 변환합니다.
		/// </summary>
		/// <param name="value">배정밀도 부동 소수점 숫자입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytes(double value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			return bytes;
		}

		/// <summary>
		/// 배정밀도 부동 소수점 숫자의 배열을 바이트 배열로 변환합니다.
		/// </summary>
		/// <param name="values">배정밀도 부동 소수점 숫자의 배열입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytes(double[] values)
		{
			byte[] array = new byte[0];
			for (int i = 0; i < values.Length; i++)
			{
				array = Concat(array, ToBytes(values[i]));
			}
			return array;
		}

		/// <summary>
		/// Converts a double-precision floating point number to a byte array.
		/// </summary>
		/// <param name="value">A double-precision floating point number to convert.</param>
		/// <param name="bytes">A byte array to save the converting result.</param>
		/// <param name="index">The first index number of byte array to start to save.</param>
		/// <returns>Returns the saved count of bytes.</returns>
		public static int ToBytes(double value, byte[] bytes, int index)
		{
			byte[] array = ToBytes(value);
			array.CopyTo(bytes, index);
			return array.Length;
		}

		/// <summary>
		/// 지정된 바이트 배열의 처음 네 바이트를 단정밀도 부동 소수점 숫자로 변환합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <returns>단정밀도 부동 소수점 숫자입니다.</returns>
		public static float ToSingle(byte[] bytes)
		{
			return ToSingle(bytes, 0);
		}

		/// <summary>
		/// 지정된 바이트 배열의 지정된 인덱스부터 네 바이트를 사용해서 단정밀도 부동 소수점 숫자로 변환합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <param name="index">시작 인덱스를 지정합니다.</param>
		/// <returns>단정밀도 부동 소수점 숫자입니다.</returns>
		public static float ToSingle(byte[] bytes, int index)
		{
			byte[] array = new byte[4];
			Array.Copy(bytes, index, array, 0, array.Length);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(array);
			}
			return BitConverter.ToSingle(array, 0);
		}

		/// <summary>
		/// 단정밀도 부동 소수점 숫자를 바이트 배열로 변환합니다.
		/// </summary>
		/// <param name="value">단정밀도 부동 소수점 숫자입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytes(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			return bytes;
		}

		/// <summary>
		/// 단정밀도 부동 소수점 숫자의 배열을 바이트 배열로 변환합니다.
		/// </summary>
		/// <param name="values">단정밀도 부동 소수점 숫자의 배열입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytes(float[] values)
		{
			byte[] array = new byte[0];
			for (int i = 0; i < values.Length; i++)
			{
				array = Concat(array, ToBytes(values[i]));
			}
			return array;
		}

		/// <summary>
		/// Converts a single-precision floating point number to a byte array.
		/// </summary>
		/// <param name="value">A single-precision floating point number to convert.</param>
		/// <param name="bytes">A byte array to save the converting result.</param>
		/// <param name="index">The first index number of byte array to start to save.</param>
		/// <returns>Returns the saved count of bytes.</returns>
		public static int ToBytes(float value, byte[] bytes, int index)
		{
			byte[] array = ToBytes(value);
			array.CopyTo(bytes, index);
			return array.Length;
		}

		/// <summary>
		/// 지정된 바이트 배열을 문자열로 변환합니다. ASCII 엔코딩을 사용합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <returns>변환된 문자열입니다.</returns>
		public static string ToString(byte[] bytes)
		{
			return Encoding.ASCII.GetString(bytes);
		}

		/// <summary>
		/// 지정된 바이트 배열에서 지정된 인덱스부터 끝까지 문자열로 변환합니다. ASCII 엔코딩을 사용합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <param name="index">시작 인덱스를 지정합니다.</param>
		/// <returns>변환된 문자열입니다.</returns>
		public static string ToString(byte[] bytes, int index)
		{
			return Encoding.ASCII.GetString(bytes, index, bytes.Length - index);
		}

		/// <summary>
		/// 지정된 바이트 배열에서 지정된 인덱스부터 지정된 개수만큼 문자열로 변환합니다. ASCII 엔코딩을 사용합니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <param name="index">시작 인덱스를 지정합니다.</param>
		/// <param name="count">변환할 바이트 개수를 지정합니다.</param>
		/// <returns>변환된 문자열입니다.</returns>
		public static string ToString(byte[] bytes, int index, int count)
		{
			return Encoding.ASCII.GetString(bytes, index, count);
		}

		/// <summary>
		/// 문자열을 바이트 배열로 변환합니다. ASCII 엔코딩을 사용합니다.
		/// </summary>
		/// <param name="s">변환할 문자열입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytes(string s)
		{
			return Encoding.ASCII.GetBytes(s);
		}

		/// <summary>
		/// Converts a part of a string to a byte array using ASCII encoding.
		/// </summary>
		/// <param name="s">A string to convert.</param>
		/// <param name="charIndex">The index of the first character to convert.</param>
		/// <param name="charCount">The number of characters to convert.</param>
		/// <param name="bytes">A byte array to save the converting result.</param>
		/// <param name="byteIndex">The first index number of byte array to start to save.</param>
		/// <returns>The saved count of bytes.</returns>
		public static int ToBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			return Encoding.ASCII.GetBytes(s, charIndex, charCount, bytes, byteIndex);
		}

		/// <summary>
		/// Converts a string in BCD format to a byte array.
		/// </summary>
		/// <param name="s">변환할 문자열입니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToBytesFromBcd(string s)
		{
			int num = s.Length / 2 + s.Length % 2;
			byte[] array = new byte[num];
			string text = s;
			for (int num2 = num - 1; num2 >= 0; num2--)
			{
				int num3;
				if (text.Length >= 2)
				{
					num3 = 2;
				}
				else
				{
					num3 = text.Length;
				}
				int num4 = num3;
				string text2 = (text2 = text.Substring(text.Length - num4, num4));
				array[num2] = byte.Parse(text2, NumberStyles.Number);
				text = text.Remove(text.Length - num4, num4);
			}
			return array;
		}

		/// <summary>
		/// Converts a byte array to a BCD string.
		/// </summary>
		/// <param name="bytes">변환할 바이트 배열입니다.</param>
		/// <returns>BCD 포맷의 문자열입니다.</returns>
		public static string ToBcdString(byte[] bytes)
		{
			return ToBcdString(bytes, 0, bytes.Length);
		}

		/// <summary>
		/// Converts the part of a byte array to a BCD string.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <param name="index">시작 인덱스입니다.</param>
		/// <param name="count">변환할 바이트 수입니다.</param>
		/// <returns>BCD 포맷의 문자열입니다.</returns>
		public static string ToBcdString(byte[] bytes, int index, int count)
		{
			string text = string.Empty;
			for (int i = index; i < index + count; i++)
			{
				text += bytes[i].ToString();
			}
			return text;
		}

		/// <summary>
		/// 바이트 배열을 ASCII 엔코딩을 사용해서 문자열로 변환합니다. 
		/// 지정된 인덱스로부터 처음 나타나는 널 문자 앞까지만 변환되므로 반환되는 바이트 배열의 길이는 지정된 바이트 수보다 작을 수 있습니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <param name="startIndex">시작 인덱스를 지정합니다.</param>
		/// <param name="count">변환할 바이트 수를 지정합니다.</param>
		/// <returns>변환된 문자열입니다.</returns>
		public static string ToStringFromFixedBytes(byte[] bytes, int startIndex, int count)
		{
			int num = 0;
			if (bytes == null)
			{
				return null;
			}
			int num2;
			if (startIndex >= 0)
			{

				num2 = ((bytes.Length <= startIndex) ? 1 : 0);
			}
			else
			{
				num2 = 1;
			}
			if (num2 != 0)
			{
				throw new ArgumentOutOfRangeException("num2");
			}
			if (bytes.Length < startIndex + count)
			{
				throw new ArgumentOutOfRangeException();
			}
			num = Array.IndexOf(bytes, (byte)0, startIndex, count);
			int num3;
			if (startIndex <= num)
			{

				num3 = ((num < startIndex + count) ? 1 : 0);
			}
			else
			{
				num3 = 0;
			}
			if (num3 != 0)
			{
				return Encoding.ASCII.GetString(bytes, startIndex, num - startIndex);
			}
			return Encoding.ASCII.GetString(bytes, startIndex, count);
		}

		/// <summary>
		/// 바이트 배열을 ASCII 엔코딩을 사용해서 문자열로 변환합니다. 
		/// 처음 나타나는 널 문자 앞까지만 변환되므로 반환되는 바이트 배열의 길이는 지정된 바이트 배열의 길이보다 작을 수 있습니다.
		/// </summary>
		/// <param name="bytes">바이트 배열입니다.</param>
		/// <returns>변환된 문자열입니다.</returns>
		public static string ToStringFromFixedBytes(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			return ToStringFromFixedBytes(bytes, 0, bytes.Length);
		}

		/// <summary>
		/// 문자열을 지정된 길이의 바이트 배열로 변환합니다. ASCII 엔코딩을 사용합니다. 문자열의 길이가 바이트 배열의 길이보다 작으면 나머지는 0으로 채워집니다.
		/// </summary>
		/// <param name="s">문자열을 지정합니다.</param>
		/// <param name="count">반환되는 바이트 배열의 길이를 지정합니다.</param>
		/// <returns>바이트 배열입니다.</returns>
		public static byte[] ToFixedBytes(string s, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			byte[] array = new byte[count];
			if (s == null)
			{
				return array;
			}
			byte[] bytes = Encoding.ASCII.GetBytes(s);
			if (bytes.Length <= array.Length)
			{
				bytes.CopyTo(array, 0);
			}
			else
			{
				Array.Copy(bytes, array, array.Length);
			}
			return array;
		}

		/// <summary>
		/// 관리되지 않는 메모리 블록을 지정된 형식으로 변환합니다.
		/// </summary>
		/// <param name="bytes">관리되지 않는 메모리 블록을 나타내는 바이트 배열입니다.</param>
		/// <param name="type">대상 형식을 지정합니다.</param>
		/// <returns>변환된 개체입니다.</returns>
		public static object FromUnmanagedBytes(byte[] bytes, Type type)
		{
			int num = 0;
			IntPtr intPtr = IntPtr.Zero;
			object result = null;
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			num = Marshal.SizeOf(type);
			try
			{
				intPtr = Marshal.AllocHGlobal(num);
				Marshal.Copy(bytes, 0, intPtr, num);
				result = Marshal.PtrToStructure(intPtr, type);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return result;
		}

		/// <summary>
		/// 지정된 개체를 관리되지 않는 메모리 블록으로 변환합니다.
		/// </summary>
		/// <param name="value">변환할 개체입니다.</param>
		/// <returns>관리되지 않는 메모리 블록을 나타내는 바이트 배열입니다.</returns>
		public static byte[] ToUnmanagedBytes(object value)
		{
			int num = 0;
			byte[] array = null;
			IntPtr intPtr = IntPtr.Zero;
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			num = Marshal.SizeOf(value);
			array = new byte[num];
			try
			{
				intPtr = Marshal.AllocHGlobal(num);
				Marshal.StructureToPtr(value, intPtr, fDeleteOld: true);
				Marshal.Copy(intPtr, array, 0, num);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return array;
		}

		/// <summary>
		/// 바이트 배열을 지정된 형식으로 변환합니다. 지정된 형식이 IConvertFromBytes 인터페이스를 구현하는 경우 해당 구현을 사용합니다.
		/// </summary>
		/// <param name="bytes">변환할 바이트 배열입니다.</param>
		/// <param name="type">대상 형식을 지정합니다.</param>
		/// <returns>변환된 개체입니다.</returns>
		public static object ToObject(byte[] bytes, Type type)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type == typeof(short))
			{
				return ToInt16(bytes);
			}
			if (type == typeof(ushort))
			{
				return ToUint16(bytes);
			}
			if (type == typeof(int))
			{
				return ToInt32(bytes);
			}
			if (type == typeof(uint))
			{
				return ToUint32(bytes);
			}
			if (type == typeof(long))
			{
				return ToInt64(bytes);
			}
			if (type == typeof(ulong))
			{
				return ToUint64(bytes);
			}
			if (type == typeof(double))
			{
				return ToDouble(bytes);
			}
			if (type == typeof(float))
			{
				return ToSingle(bytes);
			}
			if (type == typeof(string))
			{
				return ToString(bytes);
			}
			
			return FromUnmanagedBytes(bytes, type);
		}

		/// <summary>
		/// 지정된 개체를 바이트 배열로 변환합니다. 지정된 형식이 IConvertToBytes 인터페이스를 구현하는 경우 해당 구현을 사용합니다.
		/// </summary>
		/// <param name="value">변환할 개체입니다.</param>
		/// <returns>변환된 바이트 배열입니다.</returns>
		public static byte[] ToBytes(object value)
		{
			Type type = null;
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			type = value.GetType();
			if (type == typeof(short))
			{
				return ToBytes((short)value);
			}
			if (type == typeof(ushort))
			{
				return ToBytes((ushort)value);
			}
			if (type == typeof(int))
			{
				return ToBytes((int)value);
			}
			if (type == typeof(uint))
			{
				return ToBytes((uint)value);
			}
			if (type == typeof(long))
			{
				return ToBytes((long)value);
			}
			if (type == typeof(ulong))
			{
				return ToBytes((ulong)value);
			}
			if (type == typeof(double))
			{
				return ToBytes((double)value);
			}
			if (type == typeof(float))
			{
				return ToBytes((float)value);
			}
			if (type == typeof(string))
			{
				return ToBytes((string)value);
			}
			
			return ToUnmanagedBytes(value);
		}

		/// <summary>
		/// 지정된 두 개의 바이트 배열을 연결하여 새로운 바이트 배열을 생성합니다.
		/// </summary>
		/// <param name="first">연결할 첫 번째 바이트 배열입니다.</param>
		/// <param name="second">연결할 두 번째 바이트 배열입니다.</param>
		/// <returns>새로 생성된 바이트 배열입니다.</returns>
		public static byte[] Concat(byte[] first, byte[] second)
		{
			int num = 0;
			if (first == null)
			{
				first = new byte[0];
			}
			if (second == null)
			{
				second = new byte[0];
			}
			num = first.Length;
			Array.Resize(ref first, first.Length + second.Length);
			Array.Copy(second, 0, first, num, second.Length);
			return first;
		}
	}
}
