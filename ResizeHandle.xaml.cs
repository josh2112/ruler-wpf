using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rule
{
    public enum ResizerPositions
    {
        Top,
        Bottom,
        Left,
        Right
    }

    /// <summary>
    /// Interaction logic for ResizeHandle.xaml
    /// </summary>
    public partial class ResizeHandle : Thumb
    {
        private Window window;

        public ResizerPositions ResizerPosition
        {
            get { return (ResizerPositions)this.GetValue( ResizerPositionProperty ); }
            set { this.SetValue( ResizerPositionProperty, value ); }
        }

        public static readonly DependencyProperty ResizerPositionProperty = DependencyProperty.Register(
          "ResizerPosition", typeof( ResizerPositions ), typeof( ResizeHandle ), new PropertyMetadata( ResizerPositions.Top,
              ( obj, args ) => ( obj as ResizeHandle ).UpdateResizerPosition( (ResizerPositions)args.NewValue ) ) );

        public ResizeHandle() : base()
        {
            InitializeComponent();
            UpdateResizerPosition( ResizerPosition );
        }

        private void UpdateResizerPosition( ResizerPositions direction )
        {
            Cursor = (new [] { ResizerPositions.Top, ResizerPositions.Bottom } ).Contains( direction ) ? Cursors.SizeNS : Cursors.SizeWE;
        }

        private void Thumb_DragStarted( object sender, DragStartedEventArgs e )
        {
            var control = VisualTreeHelper.GetParent( this );
            while( !( control is Window ) ) control = VisualTreeHelper.GetParent( control );
            window = control as Window;
        }

        private void Thumb_DragDelta( object sender, DragDeltaEventArgs e )
        {
            switch( ResizerPosition )
            {
                case ResizerPositions.Top:
                    if( window.Height - e.VerticalChange > ActualHeight )
                    {
                        window.Top += e.VerticalChange;
                        window.Height -= e.VerticalChange;
                    }
                    break;

                case ResizerPositions.Left:
                    if( window.Width - e.HorizontalChange > ActualWidth )
                    {
                        window.Left += e.HorizontalChange;
                        window.Width -= e.HorizontalChange;
                    }
                    break;

                case ResizerPositions.Bottom:
                    if( window.Height + e.VerticalChange > ActualHeight )
                    {
                        window.Height += e.VerticalChange;
                    }
                    break;

                case ResizerPositions.Right:
                    if( window.Width + e.HorizontalChange > ActualWidth )
                    {
                        window.Width += e.HorizontalChange;
                    }
                    break;
            }
        }

        private void Thumb_DragCompleted( object sender, DragCompletedEventArgs e )
        {

        }
    }
}
