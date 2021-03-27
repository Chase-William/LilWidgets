/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
*/

using Xamarin.Forms;

using LilWidgets.Widgets;
using LilWidgets.Forms.Extensions;

namespace LilWidgets.Forms.Views
{
    public class LoadingWidgetView : StrokedEquilateralWidgetView
    {
        #region Bind-able Properties
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="ArcLength"/> property.
        /// </summary>
        public static readonly BindableProperty ArcLengthProperty = BindableProperty.Create(nameof(ArcLength), typeof(short), typeof(StrokedEquilateralWidgetView), LoadingWidget.DEFAULT_ARC_LENGTH, BindingMode.OneWay, propertyChanged: OnArcLengthPropertyChanged);

        #endregion

        #region Properties
        /// <summary>
        /// Length of the arc in degrees relative to it's start position. 
        /// For example the value of <see cref="LoadingWidget.DEFAULT_ARC_LENGTH"/> is 90 and that would be a quarter of a full 360 degree arc.
        /// </summary>
        public short ArcLength
        {
            get => (short)GetValue(ArcLengthProperty);
            set => SetValue(ArcLengthProperty, value);
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadingWidgetView"/> class.
        /// </summary>
        public LoadingWidgetView()
            => UnderlyingWidget = new LoadingWidget();

        private static void OnArcLengthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => bindable.GetCastedWidgetView<LoadingWidgetView>().GetCastedWidget<LoadingWidget>().ArcLength = (short)newValue;

        protected override void Start()
        {
            // Create a new animation object each time we start because it needs to be relative to the last position when stopped.
            Animator = new Animation(UnderlyingWidget.AnimateCallback,
                                    ((LoadingWidget)UnderlyingWidget).BaseSweepAngle,
                                    ((LoadingWidget)UnderlyingWidget).BaseSweepAngle + LoadingWidget.FULL_REVOLUTION);
            Animator.Commit(this, nameof(WidgetView), length: UnderlyingWidget.Duration, repeat: () => true);
        }

        protected override void Stop()
            => this.AbortAnimation(nameof(WidgetView));
    }
}