using ScoreApp;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Data;

namespace Converter
{

    public class DoubleToRectConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new Rect(0, 0,
                (int)(MidiManager.vue.model.XZoom * int.Parse(ConfigurationManager.AppSettings["cellWidth"].ToString())),
                (int)(MidiManager.vue.model.YZoom * int.Parse(ConfigurationManager.AppSettings["cellHeigth"].ToString()))
            );
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

    }

}
