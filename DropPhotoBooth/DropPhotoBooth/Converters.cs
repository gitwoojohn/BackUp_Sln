using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace DropPhotoBooth
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public Visibility IfTrue { get; set; }

        public object Convert( object value, Type targetType, object parameter, string language )
        {
            bool boolValue = ( value != null ) && ( value.GetType() == typeof( bool ) ) && ( bool )value;
            return boolValue ? IfTrue : ( IfTrue == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible );
        }

        public object ConvertBack( object value, Type targetType, object parameter, string language )
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class NullToVisibilityConverter : IValueConverter
    {
        public Visibility IfNull { get; set; }

        public object Convert( object value, Type targetType, object parameter, string language )
        {
            return ( value == null ) ? IfNull : ( IfNull == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible );
        }

        public object ConvertBack( object value, Type targetType, object parameter, string language )
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
