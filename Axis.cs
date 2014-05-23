using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    ///     <MyNamespace:Axis/>
    ///
    /// </summary>
    public class Axis : Control
    {
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

        public int MinorTickSize
        {
            get { return (int)this.GetValue( MinorTickSizeProperty ); }
            set { this.SetValue( MinorTickSizeProperty, value ); }
        }

        public int MajorTickSize
        {
            get { return (int)this.GetValue( MajorTickSizeProperty ); }
            set { this.SetValue( MajorTickSizeProperty, value ); }
        }
        
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
          "Orientation", typeof( Orientation ), typeof( Axis ), new FrameworkPropertyMetadata( Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsRender ) );

        public static readonly DependencyProperty ToothSizeProperty = DependencyProperty.Register(
          "ToothSize", typeof( int ), typeof( Axis ), new FrameworkPropertyMetadata( 10, FrameworkPropertyMetadataOptions.AffectsRender ) );

        public static readonly DependencyProperty MinorTickSizeProperty = DependencyProperty.Register(
          "MinorTickSize", typeof( int ), typeof( Axis ), new FrameworkPropertyMetadata( 10, FrameworkPropertyMetadataOptions.AffectsRender ) );

        public static readonly DependencyProperty MajorTickSizeProperty = DependencyProperty.Register(
          "MajorTickSize", typeof( int ), typeof( Axis ), new FrameworkPropertyMetadata( 15, FrameworkPropertyMetadataOptions.AffectsRender ) );


        static Axis()
        {
            DefaultStyleKeyProperty.OverrideMetadata( typeof( Axis ), new FrameworkPropertyMetadata( typeof( Axis ) ) );
        }

        protected override void OnRender( DrawingContext ctx )
        {
            base.OnRender( ctx );
            
            var thinPen = new Pen( this.Foreground, 1 );
            var thickPen = new Pen( this.Foreground, 3 );

            var halfPenWidth = thinPen.Thickness / 2;

            var guidelines = new GuidelineSet();
            if( Orientation == Orientation.Horizontal ) guidelines.GuidelinesX.Add( 0 + halfPenWidth );
            else guidelines.GuidelinesY.Add( 0 + halfPenWidth );
            ctx.PushGuidelineSet( guidelines );

            var typeface = new Typeface( this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch );

            if( Orientation == Orientation.Horizontal )
            {
                for( int x = 0; x < RenderSize.Width; x += 5 )
                {
                    if( x % 5 == 0 ) ctx.DrawLine( thinPen, new Point( x, RenderSize.Height ), new Point( x, RenderSize.Height - ToothSize ) );
                    if( x % 10 == 0 ) ctx.DrawLine( thickPen, new Point( x, RenderSize.Height - ToothSize ),
                        new Point( x, RenderSize.Height - ToothSize - MinorTickSize ) );
                    if( x % 50 == 0 )
                    {
                        ctx.DrawLine( thickPen, new Point( x, RenderSize.Height - ToothSize ), new Point( x, RenderSize.Height - ToothSize - MajorTickSize ) );

                        var axisLabel = new FormattedText( x.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                            typeface, this.FontSize, this.Foreground ) { TextAlignment = TextAlignment.Right };

                        ctx.DrawText( axisLabel, new Point( x - 5, RenderSize.Height - ToothSize - MajorTickSize - axisLabel.Baseline + axisLabel.Height / 2 ) );
                    }
                }
            }
            else
            {
                for( int y = 0; y < RenderSize.Height; y += 5 )
                {
                    if( y % 5 == 0 ) ctx.DrawLine( thinPen, new Point( 0, y ), new Point( ToothSize, y ) );
                    if( y % 10 == 0 ) ctx.DrawLine( thickPen, new Point( ToothSize, y ), new Point( ToothSize + MinorTickSize, y ) );
                    if( y % 50 == 0 )
                    {
                        ctx.DrawLine( thickPen, new Point( ToothSize, y ), new Point( ToothSize + MajorTickSize, y ) );

                        var axisLabel = new FormattedText( y.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                            typeface, this.FontSize, this.Foreground );

                        ctx.DrawText( axisLabel, new Point( ToothSize + MinorTickSize + 3, y - 2 ) );
                    }
                }
            }
        }
    }
}
