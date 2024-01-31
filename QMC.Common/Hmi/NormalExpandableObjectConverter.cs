using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Hmi
{
    public class NormalExpandableObjectConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return "";
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}

	public class XyPositionExpandableObjectConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
			if( value is string)
            {
				string text = value as string;
				string[] array = text.Split(',');
				XyCoordinate xyCoordinate = new XyCoordinate();
				xyCoordinate.X = double.Parse(array[0]);
				xyCoordinate.Y = double.Parse(array[1]);

				return xyCoordinate;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
			if(destinationType == typeof(string) && value is XyCoordinate)
            {
				XyCoordinate xyCoordinate = (XyCoordinate)value;
				return string.Format("{0},{1}", xyCoordinate.X, xyCoordinate.Y);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
}
