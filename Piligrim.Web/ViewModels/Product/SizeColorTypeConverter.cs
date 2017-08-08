using Piligrim.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Piligrim.Web.ViewModels.Product
{
    public class SizeColorTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string val)
            {
                var vals = val.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(x =>
                                    {
                                        var chunks = x.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                                        return new Color
                                        {
                                            Value = chunks[0],
                                            Sizes = chunks[1]
                                                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                                .Select(y => new Size { Value = y })
                                                .ToList()
                                        };
                                    })
                                    .ToList();

                var duplicateMerged = new Dictionary<string, List<string>>();

                foreach (var item in vals)
                {
                    if (duplicateMerged.ContainsKey(item.Value))
                    {
                        duplicateMerged[item.Value] = duplicateMerged[item.Value].Union(item.Sizes.Select(x => x.Value)).Distinct().ToList();
                    }
                    else
                    {
                        duplicateMerged[item.Value] = item.Sizes.Select(x => x.Value).ToList();
                    }
                }

                return new SizeColor
                {
                    Values = duplicateMerged.Select(x => new Color
                    {
                        Value = x.Key,
                        Sizes = x.Value.Select(y => new Size { Value = y }).ToList()
                    })
                };
            };

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                var colors = ((SizeColor)value).Values;

                return string.Join(";", colors
                    .Select(x => x.Value + ":" + string.Join(",", x.Sizes.Select(y => y.Value))));
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
