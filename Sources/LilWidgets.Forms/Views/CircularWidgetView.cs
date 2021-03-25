/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
*/

using Xamarin.Forms;

using SkiaSharp.Views.Forms;

using LilWidgets.Widgets;
using LilWidgets.Forms.Extensions;

namespace LilWidgets.Forms.Views
{
    /// <summary>
    /// A <see cref="CircularWidgetView"/> is a supporting class for circular-widget-views.
    /// </summary>
    public abstract class CircularWidgetView : WidgetView
    {
        #region Constants 
        /// <summary>
        /// Default stroke width used for the arcs.
        /// </summary>
        public const float DEFAULT_STROKE_WIDTH = 15;
        /// <summary>
        /// Default shadow color used with the arcs.
        /// </summary>
        public static readonly Color defaultShadowColor = Color.FromHex("#5555");
        #endregion Constants

        #region Bind-able Properties
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="ArcColorProperty"/> property.
        /// </summary>
        public static readonly BindableProperty ArcColorProperty = BindableProperty.Create(nameof(ArcColor), typeof(Color), typeof(CircularWidgetView), Color.Black, BindingMode.OneWay, propertyChanged: OnArcColorPropertyChanged);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="Duration"/> property.
        /// </summary>
        public static readonly BindableProperty DurationProperty = BindableProperty.Create(nameof(Duration), typeof(uint), typeof(CircularWidgetView), Widget.DEFAULT_DURATION_VALUE, BindingMode.OneWay, propertyChanged: OnDurationPropertyChanged);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="StrokeWidth"/> property.
        /// </summary>
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(float), typeof(CircularWidgetView), DEFAULT_STROKE_WIDTH, BindingMode.OneWay, propertyChanged: OnStrokeWidthPropertyChanged);
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="ShadowColor"/> property.
        /// </summary>
        public static readonly BindableProperty ShadowColorProperty = BindableProperty.Create(nameof(ShadowColor), typeof(Color), typeof(CircularWidgetView), defaultShadowColor, BindingMode.OneWay, propertyChanged: OnShadowColorPropertyChanged);
        #endregion Bind-able Properties

        #region Properties           
        /// <summary>
        /// Gets or sets the color of the primary arc.
        /// </summary>
        public Color ArcColor
        {
            get => (Color)GetValue(ArcColorProperty);
            set => SetValue(ArcColorProperty, value);
        }
        /// <summary>
        /// Gets or sets the color of the primary arc's shadow.
        /// </summary>
        public Color ShadowColor
        {
            get => (Color)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }
        /// <summary>
        /// Gets or sets the time in milliseconds for a one complete cycle of the animation.
        /// </summary>
        public uint Duration
        {
            get => (uint)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }
        /// <summary>
        /// Gets or sets the width of the primary arc.
        /// </summary>
        public float StrokeWidth
        {
            get => (float)GetValue(StrokeWidthProperty);
            set => SetValue(StrokeWidthProperty, value);
        }
        #endregion Properties

        #region OnPropertyChanged Handlers
        /// <summary>
        /// Updates the underlying <see cref="StrokeWidget.ArcColor"/> property to match <see cref="ArcColor"/>.
        /// </summary>
        /// <param name="bindable"><see cref="CircularWidgetView"/> instance.</param>
        /// <param name="oldValue">Old <see cref="ArcColor"/> value.</param>
        /// <param name="newValue">New <see cref="ArcColor"/> value.</param>
        private static void OnArcColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => bindable.GetCastedWidgetView<CircularWidgetView>().GetCastedWidget<StrokeWidget>().ArcColor = ((Color)newValue).ToSKColor();
        /// <summary>
        /// Updates the underlying <see cref="StrokeWidget.ShadowColor"/> property to match <see cref="ShadowColor"/>.
        /// </summary>
        /// <param name="bindable"><see cref="CircularWidgetView"/> instance.</param>
        /// <param name="oldValue">Old <see cref="ShadowColor"/> value.</param>
        /// <param name="newValue">New <see cref="ShadowColor"/> value.</param>
        private static void OnShadowColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => bindable.GetCastedWidgetView<CircularWidgetView>().GetCastedWidget<StrokeWidget>().ShadowColor = ((Color)newValue).ToSKColor();
        /// <summary>
        /// Updates the underlying <see cref="StrokeWidget.Duration"/> property to match <see cref="Duration"/>.
        /// </summary>
        /// <param name="bindable"><see cref="CircularWidgetView"/> instance.</param>
        /// <param name="oldValue">Old <see cref="Duration"/> value.</param>
        /// <param name="newValue">New <see cref="Duration"/> value.</param>
        private static void OnDurationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => bindable.GetCastedWidgetView<CircularWidgetView>().GetCastedWidget<StrokeWidget>().Duration = (uint)newValue;
        /// <summary>
        /// Updates the underlying <see cref="StrokeWidget.StrokeWidth"/> property to match <see cref="StrokeWidth"/>.
        /// </summary>
        /// <param name="bindable"><see cref="CircularWidgetView"/> instance.</param>
        /// <param name="oldValue">Old <see cref="StrokeWidth"/> value.</param>
        /// <param name="newValue">New <see cref="StrokeWidth"/> value.</param>
        private static void OnStrokeWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => bindable.GetCastedWidgetView<CircularWidgetView>().GetCastedWidget<StrokeWidget>().StrokeWidth = (float)newValue;
        #endregion
    }
}
