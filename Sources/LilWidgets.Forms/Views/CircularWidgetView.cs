using LilWidgets.Interfaces;
using LilWidgets.Widgets;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LilWidgets.Forms.Views
{
    /// <summary>
    /// Generic class for circular widgets that contains front-end bindings for Xamarin.Forms applications.
    /// </summary>
    public abstract class CircularWidgetView : WidgetView, ICircularWidgetBindables
    {
        #region Constants
        /// <summary>
        /// The default value for the duration of the progress animation.
        /// </summary>
        public const uint DEFAULT_ANIMATION_DURATION = 1000;
        /// <summary>
        /// The default stroke width used for the arcs.
        /// </summary>
        public const float DEFAULT_STROKE_WIDTH = 15;
        /// <summary>
        /// The default shadow color used with the arcs.
        /// </summary>
        public static readonly Color defaultShadowColor = Color.FromHex("#5555");
        #endregion Constants

        #region Bind-able Properties
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="ArcColorProperty"/> property.
        /// </summary
        public static readonly BindableProperty ArcColorProperty = BindableProperty.Create(nameof(ArcColor), typeof(Color), typeof(CircularWidgetView), Color.Black, BindingMode.OneWay);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="Duration"/> property.
        /// </summary>
        public static readonly BindableProperty DurationProperty = BindableProperty.Create(nameof(Duration), typeof(uint), typeof(CircularWidgetView), DEFAULT_ANIMATION_DURATION, BindingMode.OneWay);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="StrokeWidth"/> property.
        /// </summary>
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(float), typeof(CircularWidgetView), DEFAULT_STROKE_WIDTH, BindingMode.OneWay);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="ShadowColor"/> property.
        /// </summary>
        public static readonly BindableProperty ShadowColorProperty = BindableProperty.Create(nameof(ShadowColor), typeof(Color), typeof(CircularWidgetView), defaultShadowColor, BindingMode.OneWay);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="IsAnimating"/> property.
        /// </summary>
        public static readonly BindableProperty IsAnimatingProperty = BindableProperty.Create(nameof(IsAnimating), typeof(bool), typeof(CircularWidgetView), false, BindingMode.OneWay);        
        #endregion Bind-able Properties

        #region Properties           
        /// <summary>
        /// The color of the arc.
        /// </summary>
        public Color ArcColor
        {
            get => (Color)GetValue(ArcColorProperty);
            set 
            {
                SetValue(ArcColorProperty, value);                
            }
        }
        /// <summary>
        /// The color of the shadow to be used with the arcs.
        /// </summary>
        public Color ShadowColor
        {
            get => (Color)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }
        /// <summary>
        /// The time in milliseconds it takes for 1 complete cycle of the animation.
        /// </summary>
        public uint Duration
        {
            get => (uint)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }
        /// <summary>
        /// The target stroke width value to be used for all arcs that make up the widget.
        /// </summary>
        public float StrokeWidth
        {
            get => (float)GetValue(StrokeWidthProperty);
            set => SetValue(StrokeWidthProperty, value);
        }
        /// <summary>
        /// Determines the state of the animation.
        /// True == Running, False == Inactive
        /// </summary>
        public bool IsAnimating
        {
            get => (bool)GetValue(IsAnimatingProperty);
            set => SetValue(IsAnimatingProperty, value);
        }
        SKColor ICircularWidgetBindables.ArcColor { get; set; }
        SKColor ICircularWidgetBindables.ShadowColor { get; set; }
        #endregion Properties
    }
}
