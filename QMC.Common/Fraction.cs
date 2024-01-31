using QMC.Common.Hmi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
	[Serializable]
	[TypeConverter(typeof(FractionConverter))]
	public class Fraction
    {
		private const string NumeratorXmlElementName = "Numerator";

		private const string DenominatorXmlElementName = "Denominator";

		private double m_Numerator;

		private double m_Denominator;

		/// <summary>
		/// 분자의 값을 가져오거나 설정합니다.
		/// </summary>
		public double Numerator
		{
			get
			{
				return m_Numerator;
			}
			set
			{
				m_Numerator = value;
			}
		}

		/// <summary>
		/// 분모의 값을 가져오거나 설정합니다.
		/// </summary>
		public double Denominator
		{
			get
			{
				return m_Denominator;
			}
			set
			{
				CheckDenominator(value);
				m_Denominator = value;
			}
		}

		/// <summary>
		/// Fraction 구조체의 새 인스턴스를 초기화합니다.
		/// </summary>
		/// <param name="numerator">분자 값입니다.</param>
		/// <param name="denominator">분모 값입니다.</param>
		public Fraction(double numerator, double denominator)
		{
			CheckDenominator(denominator);
			m_Numerator = numerator;
			m_Denominator = denominator;
		}

		/// <summary>
		/// 역수를 반환합니다.
		/// </summary>
		/// <returns>역수로 변환된 Fraction 개체입니다.</returns>
		public Fraction Reverse()
		{
			return new Fraction(Denominator, Numerator);
		}

		private static void CheckDenominator(double denominator)
		{
			if (denominator == 0.0)
			{
				throw new ArgumentOutOfRangeException("denominator");
			}
		}
		/// <summary>
		/// 주어진 분수를 double형으로 반환합니다.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static implicit operator double(Fraction value)
		{
			if(Object.ReferenceEquals(value, null))
				return 0.0;
            if (value.Denominator == 0.0)
			{
				return double.NaN;
			}
			
			
			return value.Numerator / value.Denominator;
		}

		/// <summary>
		/// 주어진 double형을 분수로 반환합니다.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static implicit operator Fraction(double value)
		{
			double num = 0.0;
			double num2 = 0.0;
			string text = value.ToString();
			int num3 = text.IndexOf('.');
			int num4 = 0;
			if (0 < num3)
			{
				num4 = text.Length - num3 - 1;
			}
			num2 = Math.Pow(10.0, num4);
			num = value * num2;
			return new Fraction(num, num2);
		}

		public static bool operator ==(Fraction a, Fraction b)
		{
			return (double)a == (double)b;
		}

		public static bool operator !=(Fraction a, Fraction b)
		{
			return !(a == b);
		}

		public static Fraction Parse(string text)
		{
			double num = 0.0;
			double num2 = 0.0;
			string[] array = null;
			array = text.Split('/');
			num = double.Parse(array[0]);
			num2 = double.Parse(array[1]);
			return new Fraction(num, num2);
		}

		/// <summary>
		/// 재정의 되었습니다.
		/// </summary>
		/// <param name="obj">값을 비교할 개체입니다.</param>
		/// <returns>값이 같으면 true이고, 그렇지 않으면 false입니다.</returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is Fraction))
			{
				return false;
			}
			Fraction value = (Fraction)obj;
			return Equals(value);
		}

		/// <summary>
		/// 지정된 Fraction 개체를 현재 개체와 비교합니다.
		/// </summary>
		/// <param name="value">비교할 Fraction 개체입니다.</param>
		/// <returns>값이 같으면 true이고, 그렇지 않으면 false입니다.</returns>
		public bool Equals(Fraction value)
		{
			if ((object)value == null)
			{
				return false;
			}
			return this == value;
		}

		/// <summary>
		/// 재정의 되었습니다.
		/// </summary>
		/// <returns>this.Numerator.GetHashCode() ^ this.Denominator.GetHashCode() 연산의 결과입니다.</returns>
		public override int GetHashCode()
		{
			return Numerator.GetHashCode() ^ Denominator.GetHashCode();
		}

		/// <summary>
		/// 재정의 되었습니다.
		/// </summary>
		/// <returns>"Numerator / Denominator = Double 형식의 값" 포맷의 문자열입니다.</returns>
		public override string ToString()
		{
			return string.Format("{0} / {1} = {2}", Numerator, Denominator, (double)this);
		}
	}
}
