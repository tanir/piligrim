using Piligrim.Core.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace Piligrim.Web.ViewModels.Product
{
    [TypeConverter(typeof(SizeColorTypeConverter))]
    public class SizeColor
    {
        public IEnumerable<Color> Values { get; set; }

        public override string ToString()
        {
            var converter = TypeDescriptor.GetConverter(this);
            if (converter != null && converter.CanConvertTo(typeof(string)))
            {
                return converter.ConvertToString(this);
            }
            return base.ToString();
        }
    }
}
