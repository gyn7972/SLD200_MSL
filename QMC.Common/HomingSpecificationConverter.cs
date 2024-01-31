using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    internal class HomingSpecificationConverter : ExpandableObjectConverter
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
			if (value is string)
			{
				string text = value as string;
				string[] array = text.Split(',');
				HomingSpecification homingSpecification = new HomingSpecification();
				homingSpecification.Velocity = double.Parse(array[0]);
				homingSpecification.Acceleration = double.Parse(array[1]);
				homingSpecification.Deceleration = double.Parse(array[2]);
				homingSpecification.NegativePosition = double.Parse(array[3]);
				homingSpecification.PositivePosition = double.Parse(array[4]);
				homingSpecification.EscapeDistance = double.Parse(array[5]);
				homingSpecification.HomePosition = double.Parse(array[6]);
				homingSpecification.Method = (HomingMethod)Enum.Parse(typeof(HomingMethod), array[7]);
				homingSpecification.EnablePreciseSearch = bool.Parse(array[8]);
				homingSpecification.PreciseSearchVelocityPercent = int.Parse(array[9]);
				homingSpecification.EnableIndexSearch = bool.Parse(array[10]);
				return homingSpecification;
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value is HomingSpecification)
			{
				HomingSpecification homingSpecification = value as HomingSpecification;
				return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}", 
					homingSpecification.Velocity, 
					homingSpecification.Acceleration, 
					homingSpecification.Deceleration, 
					homingSpecification.NegativePosition, 
					homingSpecification.PositivePosition, 
					homingSpecification.EscapeDistance, 
					homingSpecification.HomePosition, 
					homingSpecification.Method, 
					homingSpecification.EnablePreciseSearch, 
					homingSpecification.PreciseSearchVelocityPercent, 
					homingSpecification.EnableIndexSearch);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
