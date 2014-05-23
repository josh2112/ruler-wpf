using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Rule
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Rule"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Rule;assembly=Rule"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:Needle/>
    ///
    /// </summary>
    public class Needle : Control, INotifyPropertyChanged
    {
        #region Property Change Notification
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged( string propertyName )
        {
            var handler = PropertyChanged;
            if( handler != null ) handler( this, new PropertyChangedEventArgs( propertyName ) );
        }
        #endregion // Property Change Notification

        private int _mousePos;
        private Typeface tooltipTypeface;

        public Orientation Orientation
        {
            get { return (Orientation)this.GetValue( OrientationProperty ); }
            set { this.SetValue( OrientationProperty, value ); }
        }

        public int ToothSize
        {
            get { return (int)this.GetValue( ToothSizeProperty ); }
            set { this.SetValue( ToothSizeProperty, value ); }
        }

        public int NeedleSize
        {
            get { return (int)this.GetValue( NeedleSizeProperty ); }
            set { this.SetValue( NeedleSizeProperty, value ); }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
          "Orientation", typeof( Orientation ), typeof( Needle ), new FrameworkPropertyMetadata( Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsRender ) );

        public static readonly DependencyProperty ToothSizeProperty = DependencyProperty.Register(
          "ToothSize", typeof( int ), typeof( Needle ), new FrameworkPropertyMetadata( 10, FrameworkPropertyMetadataOptions.AffectsRender ) );


        public static readonly DependencyProperty NeedleSizeProperty = DependencyProperty.Register(
          "NeedleSize", typeof( int ), typeof( Needle ), new FrameworkPropertyMetadata( 30, FrameworkPropertyMetadataOptions.AffectsRender ) );

        public int MousePosition
        {
            get { return _mousePos; }
            private set { _mousePos = value; OnPropertyChanged( "MousePosition" ); }
        }

        static Needle()
        {
            DefaultStyleKeyProperty.OverrideMetadata( typeof( Needle ), new FrameworkPropertyMetadata( typeof( Needle ) ) );
        }

        public Needle() : base()
        {
            if( DesignerProperties.GetIsInDesignMode( this ) )
            {
                MousePosition = 20;
            }

            Background = Brushes.Transparent;

            tooltipTypeface = new Typeface( FontFamily, FontStyle, FontWeight, FontStretch );

            this.PreviewMouseMove += ( s, e ) =>
            {
                var pos = e.GetPosition( this );

                if( Orientation == Orientation.Horizontal ) MousePosition = (int)pos.X;
                else MousePosition = (int)pos.Y;
                
                InvalidateVisual();
                e.Handled = false;
            };
        }

        protected override void OnRender( DrawingContext ctx )
        {
            base.OnRender( ctx );

            var thinPen = new Pen( this.Foreground, 1 );
            var thickPen = new Pen( this.Foreground, 3 );

            var halfPenWidth = thinPen.Thickness / 2;

            var guidelines = new GuidelineSet();
            guidelines.GuidelinesX.Add( 0 + halfPenWidth );
            guidelines.GuidelinesY.Add( 0 + halfPenWidth );
            ctx.PushGuidelineSet( guidelines );

            Point pt1, pt2, pt3;
            if( Orientation == Orientation.Horizontal )
            {
                pt1 = new Point( MousePosition, RenderSize.Height );
                pt2 = new Point( MousePosition, RenderSize.Height - ToothSize );
                pt3 = new Point( MousePosition, RenderSize.Height - ToothSize - NeedleSize );
            }
            else
            {
                pt1 = new Point( 0, MousePosition );
                pt2 = new Point( ToothSize, MousePosition );
                pt3 = new Point( ToothSize + NeedleSize, MousePosition );
            }

            ctx.DrawLine( new Pen( Brushes.White, 3 ), pt1, pt2 );
            ctx.DrawLine( thinPen, pt1, pt2 );
            ctx.DrawLine( thickPen, pt2, pt3 );
                
           // var axisLabel = new FormattedText( MousePosition.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
           //         tooltipTypeface, FontSize, Foreground );

            //ctx.DrawText( axisLabel, new Point( ToothSize + MinorTickSize + 3, y - 2 ) );;
        }
    }
}
