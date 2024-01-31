using System;
using System.ComponentModel;
using System.Globalization;

namespace QMC.Common.Hmi
{
    internal class FractionConverter : ExpandableObjectConverter
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
                string[] array = text.Split('/');
                Fraction fraction = default(Fraction);
                fraction.Numerator = double.Parse(array[0]);
                fraction.Denominator = double.Parse(array[1]);
                return fraction;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            int num;
            if (destinationType == typeof(string))
            {
                num = ((value is Fraction) ? 1 : 0);
            }
            else
            {
                num = 0;
            }
            if (num != 0)
            {

                Fraction fraction = (Fraction)value;
                return string.Format("{0} / {1}", fraction.Numerator.ToString(), fraction.Denominator.ToString());

            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
