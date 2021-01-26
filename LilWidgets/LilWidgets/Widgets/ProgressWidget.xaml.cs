using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Forms.BindableProperty;

namespace LilWidgets.Widgets
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProgressWidget : ContentView
    {
        #region Constants
        const float ALLOWED_DEVIATION = 0.0002f;
        #endregion

        #region Bindable Properties
        /// <summary>
        /// Bindable Property for the <see cref="PercentValue"/> property.
        /// </summary>        
        public static readonly BindableProperty PercentValueProperty = BindableProperty.Create(nameof(PercentValue), typeof(double), typeof(ProgressWidget), 0d, BindingMode.OneWay, ValidatePercentage, PercentValuePropertyChanged);

        public static readonly BindableProperty BackRingColorProperty = BindableProperty.Create(nameof(BackRingColor), typeof(Color), typeof(ProgressWidget), Color.Black, BindingMode.OneWay, null, BackgroundRingColorPropertyChanged);

        public static readonly BindableProperty ProgressRingColorProperty = BindableProperty.Create(nameof(ProgressRingColor), typeof(Color), typeof(ProgressWidget), Color.White, BindingMode.OneWay, null, ProgressRingColorPropertyChanged);
        #endregion

        #region Properties
        /// <summary>
        /// Contains the value of progress. Expects a value from 0.0 to 1.0 representing the progress.
        /// </summary>
        public double PercentValue
        {
            get => (double)GetValue(PercentValueProperty);
            set => SetValue(PercentValueProperty, value);
        }
        public Color BackRingColor
        {
            get => (Color)GetValue(BackRingColorProperty);
            set => SetValue(BackRingColorProperty, value);
        }
        public Color ProgressRingColor
        {
            get => (Color)GetValue(ProgressRingColorProperty);
            set => SetValue(ProgressRingColorProperty, value);
        }
        #endregion

        #region Fields
        private SKPaint progressPaint = new SKPaint
        {
            Color = SKColors.Black,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true
        };

        private SKPaint backgroundPaint = new SKPaint
        {
            Color = SKColors.LightGray,
            StrokeWidth = 1,
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            ImageFilter = SKImageFilter.CreateDropShadow(0, 0, 4, 4, SKColor.Parse("#5555"))
        };

        /// <summary>
        /// Used to determine when the animation should stop
        /// </summary>
        private Func<bool> comparer = null;

        //private SKPaint textPaint = new SKPaint
        //{
        //    Color = SKColors.Blue,
        //    Style = SKPaintStyle.Fill,
        //    IsAntialias = true
        //};

        double nValue = 0;
        double oValue = 0;
        float millisecondDuration = 2000; // 2 seconds
        float totalFrames = 0;

        // Get the signed change needed each frame
        double changePerFrame = 0;

        bool animating;

        double currentPercentageValue = 0;
        float centerXOffset = 0;
        float centerYOffset = 0;
        float sweepStart = -90;
        #endregion
        public ProgressWidget()
        {
            InitializeComponent();            
        }

        private void canvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            var info = e.Info;          

            canvas.Clear();

            //canvas.DrawColor(SKColors.LightBlue);

            centerXOffset = info.Width / 2;
            centerYOffset = info.Height / 2;


            SKRect rect = new SKRect();
            if (centerXOffset > centerYOffset)
            {
                rect.Size = new SKSize(centerYOffset, centerYOffset);
            }
            else
            {
                rect.Size = new SKSize(centerXOffset, centerXOffset);
            }            
            rect.Location = new SKPoint(centerXOffset / 2, centerYOffset - centerXOffset / 2); // Centering to the center of the parent view x and y

            //float sweepAngle = 0;// = 75f / 100f * 360f;  // (90 / 100) * 360 -- percentage to drawing angle
            //PercentageToSweepAngle(sweepAngle);
            //canvas.DrawCircle(new SKPoint(centerXOffset, centerYOffset), centerXOffset, paint);
            SKPath progressPath = new SKPath();            
            progressPath.AddArc(rect, sweepStart, PercentageToSweepAngle(currentPercentageValue));
            SKPath backgroundPath = new SKPath();
            backgroundPath.AddArc(rect, sweepStart, 360f);

            SKPaint textPaint = new SKPaint
            {
                Color = SKColors.Black,
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            string str = currentPercentageValue.ToString("P");

            // Adjust TextSize property so text is 75% of screen width
            float textWidth = textPaint.MeasureText(str);
            textPaint.TextSize = 0.75f * rect.Width * textPaint.TextSize / textWidth;
            var strokeWidth = 0.9f * rect.Size.Width / textWidth;
            progressPaint.StrokeWidth = strokeWidth;
            backgroundPaint.StrokeWidth = strokeWidth;
            // Find the text bounds
            SKRect textBounds = new SKRect();
            textPaint.MeasureText(str, ref textBounds);

            // Draw Calls
            canvas.DrawPath(backgroundPath, backgroundPaint); // Background Arc
            canvas.DrawPath(progressPath, progressPaint); // Progress Arc            
            canvas.DrawText(str, centerXOffset - textBounds.MidX, centerYOffset - textBounds.MidY, textPaint); // Progress Text
        }    
        
        private float PercentageToSweepAngle(double percentage)
            => (float)percentage * 100 / 100f * 360f;
        
        /// <summary>
        /// Validates the given value of the <see cref="PercentValue"/> property.
        /// </summary>
        /// <param name="bindable">Instance of declaring type.</param>
        /// <param name="value">Value of <see cref="PercentValue"/></param>
        /// <returns>Indication if successful validation.</returns>
        private static bool ValidatePercentage(BindableObject bindable, object value)
        {
            double? percentage = value as double?;

            if (percentage == null)
                return false;
            else if (percentage < 0 || percentage > 1)
                return false;

            return true;
        }      

        /// <summary>
        /// A callback function to the <see cref="PercentValue"/> being changed.
        /// </summary>
        /// <param name="bindable">Declaring type.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">Newly assigned value.</param>
        private static void PercentValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {            
            ProgressWidget widget = (ProgressWidget)bindable;            
            widget.nValue = (double)newValue;
            widget.oValue = (double)oldValue;
            widget.millisecondDuration = 2000; // 2 seconds
            widget.totalFrames = 60f * (widget.millisecondDuration / 1000f);
            
            // Get the signed change needed each frame
            widget.changePerFrame = (widget.nValue - widget.oValue) / widget.totalFrames;            

            // No use of locking variables for thread safety should be needed because all modifications from timer are queue in the mainthread dispatcher

            if (widget.changePerFrame < 0) // If decreasing            
                widget.comparer = () => widget.currentPercentageValue > widget.PercentValue + ALLOWED_DEVIATION; // When decreasing, if the value is larger than the target keep decreasing.         
            else // If inreasing    
                widget.comparer = () => widget.currentPercentageValue < widget.PercentValue - ALLOWED_DEVIATION; // When increasing, if the value is smaller than the target keep increasing.  

            if (widget.animating) // Prevent another timer being started if the bindable widget is already animating itself
                return;
            else // If the widget animating state is false then assign true because we shall start the animation
                widget.animating = true;

            Device.StartTimer(TimeSpan.FromSeconds(1f / 60f), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    widget.currentPercentageValue += widget.changePerFrame;
                    // Informing the SKCanvasView that it must redraw itself
                    widget.canvas.InvalidateSurface();
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"currentPercentageValue: {widget.currentPercentageValue} | Target: {widget.PercentValue}");
#endif             
                    widget.animating = widget.comparer.Invoke();

                    if (!widget.animating) // If not animating anymore assign the currentPercentageValue the exact desired amount
                        widget.currentPercentageValue = widget.PercentValue;
                });                

                // Continue while we haven't reached the target value
                return widget.animating;
            });
        }


        private static void BackgroundRingColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ProgressWidget widget = (ProgressWidget)bindable;
            widget.backgroundPaint.Color = ((Color)newValue).ToSKColor();
            if (!widget.animating)            
                widget.canvas.InvalidateSurface();            
        }

        private static void ProgressRingColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ProgressWidget widget = (ProgressWidget)bindable;
            widget.progressPaint.Color = ((Color)newValue).ToSKColor();
            if (!widget.animating)            
                widget.canvas.InvalidateSurface();            
        }
    }
}