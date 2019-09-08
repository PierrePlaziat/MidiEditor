using ScoreApp;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Data;

namespace Converters
{

    /// Permits cell tiling zoom binding
    public class DoubleToRectConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new Rect(0, 0,
                (MidiManager.attachedView.Model.XZoom * int.Parse(ConfigurationManager.AppSettings["cellWidth"])),
                (MidiManager.attachedView.Model.YZoom * int.Parse(ConfigurationManager.AppSettings["cellHeigth"]))
            );
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

    }

}
