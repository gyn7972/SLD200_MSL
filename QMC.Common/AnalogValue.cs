using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
	public struct AnalogValue
	{
		private double m_Analog;

		private double m_Value;

		public double Analog
		{
			get
			{
				return m_Analog;
			}
			set
			{
				m_Analog = value;
			}
		}

		public double Value
		{
			get
			{
				return m_Value;
			}
			set
			{
				m_Value = value;
			}
		}

		public AnalogValue(double analog, double value)
		{
			m_Analog = analog;
			m_Value = value;
		}
		
	}

	public class AnalogValueCollection : Collection<AnalogValue>
	{
		
	}
}
