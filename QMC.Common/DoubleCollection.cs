using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
	[Serializable]
	public class DoubleCollection : Collection<double>
	{
		/// <summary>
		/// 읽기 전용인지 여부를 가져옵니다.
		/// </summary>
		public bool IsReadOnly => base.Items.IsReadOnly;

		/// <summary>
		/// DoubleCollection 클래스의 새 인스턴스를 초기화합니다.
		/// </summary>
		/// <param name="list">래핑할 목록입니다.</param>
		public DoubleCollection(IList<double> list)
			: base(list)
		{
		}

		/// <summary>
		/// DoubleCollection 클래스의 새 인스턴스를 초기화합니다.
		/// </summary>
		public DoubleCollection()
			: this(new List<double>())
		{
		}

		/// <summary>
		/// 지정된 배열을 컬렉션으로 변환합니다.
		/// </summary>
		/// <param name="array">변환할 배열입니다.</param>
		/// <returns>변환된 컬렉션입니다.</returns>
		public static DoubleCollection Parse(double[] array)
		{
			if (array == null)
			{
				return null;
			}
			return new DoubleCollection(array);
		}

		/// <summary>
		/// 지정된 값을 가지는 컬렉션으로 변환합니다.
		/// </summary>
		/// <param name="value">컬렉션에 추가할 값입니다.</param>
		/// <returns>변환된 컬렉션입니다.</returns>
		public static DoubleCollection Parse(double value)
		{
			return Parse(new double[1] { value });
		}

		/// <summary>
		/// 지정된 문자열을 컬렉션으로 변환합니다.
		/// </summary>
		/// <param name="text">변환할 문자열입니다. 값이 여러 개인 경우 콤마로 분리된 문자열입니다.</param>
		/// <param name="style">문자열에 포함된 값의 스타일입니다.</param>
		/// <returns>변환된 컬렉션입니다.</returns>
		public static DoubleCollection Parse(string text, NumberStyles style)
		{
			double[] array = new double[0];
			string[] array2 = null;
			if (!string.IsNullOrEmpty(text))
			{
				array2 = text.Split(',');
				array = new double[array2.Length];
				for (int i = 0; i < array2.Length; i++)
				{
					array[i] = double.Parse(array2[i].Trim(), style);
				}
			}
			return Parse(array);
		}

		/// <summary>
		/// 지정된 문자열을 컬렉션으로 변환합니다.
		/// </summary>
		/// <param name="text">변환할 문자열입니다. 값이 여러 개인 경우 콤마로 분리된 문자열입니다.</param>
		/// <returns>변환된 컬렉션입니다.</returns>
		public static DoubleCollection Parse(string text)
		{
			return Parse(text, NumberStyles.Float | NumberStyles.AllowThousands);
		}

		/// <summary>
		/// 지정된 문자열을 컬렉션으로 변환합니다.
		/// </summary>
		/// <param name="text">변환할 문자열입니다. 값이 여러 개인 경우 콤마로 분리된 문자열입니다.</param>
		/// <param name="style">문자열에 포함된 값의 스타일입니다.</param>
		/// <param name="collection">변환된 컬렉션입니다.</param>
		/// <returns>성공하면 0이고, 그렇지 않으면 0이 아닌 값입니다.</returns>
		public static int TryParse(string text, NumberStyles style, out DoubleCollection collection)
		{
			int result = 0;
			collection = null;
			try
			{
				collection = Parse(text, style);
				return result;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return -1;
			}
		}

		/// <summary>
		/// 지정된 문자열을 컬렉션으로 변환합니다.
		/// </summary>
		/// <param name="text">변환할 문자열입니다. 값이 여러 개인 경우 콤마로 분리된 문자열입니다.</param>
		/// <param name="collection">변환된 컬렉션입니다.</param>
		/// <returns>성공하면 0이고, 그렇지 않으면 0이 아닌 값입니다.</returns>
		public static int TryParse(string text, out DoubleCollection collection)
		{
			int result = 0;
			collection = null;
			try
			{
				collection = Parse(text);
				return result;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return -1;
			}
		}

		/// <summary>
		/// 컬렉션을 배열로 변환합니다.
		/// </summary>
		/// <returns>변환된 배열입니다.</returns>
		public double[] ToArray()
		{
			double[] array = new double[base.Count];
			CopyTo(array, 0);
			return array;
		}

		/// <summary>
		/// 변경이 가능한 컬렉션으로 변환합니다.
		/// </summary>
		/// <returns>변환된 컬렉션입니다.</returns>
		public DoubleCollection WritableClone()
		{
			DoubleCollection doubleCollection = new DoubleCollection();
			using (IEnumerator<double> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					double current = enumerator.Current;
					doubleCollection.Add(current);
				}
			}
			return doubleCollection;
		}

		/// <summary>
		/// 개체를 문자열로 변환합니다.
		/// </summary>
		/// <param name="format">값을 변환할 때 적용할 포맷 문자열입니다.</param>
		/// <returns>변환된 문자열입니다.</returns>
		public string ToString(string format)
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerator<double> enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					double current = enumerator.Current;
					if (0 < stringBuilder.Length)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(current.ToString(format));
				}
			}
			finally
			{
				if (enumerator != null)
				{
					enumerator.Dispose();
				}
			}
			return stringBuilder.ToString();
		}

		/// <summary>
		/// 재정의 되었습니다.
		/// </summary>
		/// <returns>콤마로 구분되는 문자열입니다.</returns>
		public override string ToString()
		{
			return ToString(null);
		}

		public static implicit operator DoubleCollection(double value)
		{
			return Parse(value);
		}

		public static implicit operator DoubleCollection(double[] array)
		{
			return Parse(array);
		}

		public static implicit operator double[](DoubleCollection collection)
		{
			if (collection == null)
			{
				return null;
			}
			return collection.ToArray();
		}

		public static explicit operator DoubleCollection(string text)
		{
			return Parse(text);
		}

		public static explicit operator string(DoubleCollection collection)
		{
			if (collection == null)
			{
				return "";
			}
			return collection.ToString();
		}
	}
}
