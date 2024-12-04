using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TransStarter_Task.WPFApplication.Modules.MainApp.Model
{
    internal class AnnualReportEntryBackgroundConverter : IValueConverter
    {
        public object? Convert (object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var sValue = value as string;
            if (!double.TryParse(sValue, out var converted)) 
                return "Transparent";

            return converted > 25000000 ? "#40ffffff" : "Transparent";
        }

        public object? ConvertBack (object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
