using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Property Change Notification
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged( string propertyName )
        {
            var handler = PropertyChanged;
            if( handler != null ) handler( this, new PropertyChangedEventArgs( propertyName ) );
        }
        #endregion // Property Change Notification

        private Orientation _orientation = Orientation.Horizontal;
        private int _mousePosition;

        public Orientation Orientation
        {
            get { return _orientation; }
            set { if( value != _orientation ) { _orientation = value; OnPropertyChanged( "Orientation" ); } }
        }

        public int MousePosition
        {
            get { return _mousePosition; }
            set { _mousePosition = value; OnPropertyChanged( "MousePosition" ); }
        }

        public MainWindow()
        {
            InitializeComponent();
            
            needle.PropertyChanged += ( s, e ) => { if( e.PropertyName.Equals( "MousePosition" ) ) this.MousePosition = needle.MousePosition; };

            var props = Properties.Settings.Default;
            var rect = props.WindowRect;
            if( rect.Size.Width > 0 && rect.Size.Height > 0 )
            {
                this.Top = rect.Top;
                this.Left = rect.Left;
                this.Width = rect.Width;
                this.Height = rect.Height;
            }
            
            Orientation = props.Orientation;
            
            this.DataContext = this;

            this.Closing += ( s, e ) =>
            {
                props.WindowRect = new Rect( this.Left, this.Top, this.Width, this.Height );
                props.Orientation = Orientation;
                props.Save();
            };
        }

        private void Grid_MouseDown( object sender, MouseButtonEventArgs e )
        {
            if( e.LeftButton == MouseButtonState.Pressed ) this.DragMove();
        }

        private void ToggleOrientation( object sender, ExecutedRoutedEventArgs e )
        {
            Orientation = Orientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;
            FlipWindowDimensions();
        }

        private void FlipWindowDimensions()
        {
            var tmp = this.Width;
            this.Width = this.Height;
            this.Height = tmp;

            var adj = ( Math.Max( this.Width, this.Height ) / 2 - Math.Min( this.Width, this.Height ) / 2 ) *
                (Orientation == Orientation.Vertical ? 1 : -1 );
            this.Top -= adj;
            this.Left += adj;

            if( this.Top + this.Height > SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight )
                this.Top = SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight - this.Height;
            if( this.Left + this.Width > SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth )
                this.Left = SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth - this.Width;

            this.Top = Math.Max( this.Top, SystemParameters.VirtualScreenTop );
            this.Left = Math.Max( this.Left, SystemParameters.VirtualScreenLeft );
        }

        private void Quit( object sender, ExecutedRoutedEventArgs e )
        {
            this.Close();
        }

        private void Grid_PreviewMouseMove( object sender, MouseEventArgs e )
        {
            var pos = e.GetPosition( this );
            tooltip.HorizontalOffset = pos.X;
            tooltip.VerticalOffset = pos.Y - 24;
        }
    }
}
