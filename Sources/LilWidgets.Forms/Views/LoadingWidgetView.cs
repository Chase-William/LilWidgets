﻿/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
*/

using Xamarin.Forms;

using LilWidgets.Widgets;

namespace LilWidgets.Forms.Views
{
    public class LoadingWidgetView : CircularWidgetView
    {
        #region Bind-able Properties
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="ArcLength"/> property.
        /// </summary>
        public static readonly BindableProperty ArcLengthProperty = BindableProperty.Create(nameof(ArcLength), typeof(short), typeof(CircularWidgetView), LoadingWidget.DEFAULT_ARC_LENGTH, BindingMode.OneWay);

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
        {
            var widget = new LoadingWidget();
            UnderlyingWidget = widget;
            Animator = new Animation(UnderlyingWidget.AnimateCallback,
                                    widget.BaseSweepAngle,
                                    widget.BaseSweepAngle + LoadingWidget.FULL_REVOLUTION);            
        }
    }
}