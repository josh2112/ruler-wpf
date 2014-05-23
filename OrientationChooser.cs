using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Rule
{
    public class OrientationChooser : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value.GetType() != typeof( Orientation ) ) throw new ArgumentException( "'value' must be an Orientation" );
            if( ( parameter as String ) == null ) throw new ArgumentException( "'parameter' must be a non-null string" );

            var choices = ( parameter as string ).Split( '|' );
            if( choices.Count() != 2 ) throw new ArgumentException( "'parameter' must contain exactly two choices separated by the '|' character" );

            return (Orientation )value == Orientation.Horizontal ? choices.ElementAt( 0 ) : choices.ElementAt( 1 );
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotSupportedException();
        }
    }
}
