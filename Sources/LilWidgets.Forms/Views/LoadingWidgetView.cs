using LilWidgets.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LilWidgets.Forms.Views
{
    public class LoadingWidgetView : CircularWidgetView, ILoadingWidgetBindables
    {
        #region Bind-able Properties
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="ArcLength"/> property.
        /// </summary>
        public static readonly BindableProperty ArcLengthProperty = BindableProperty.Create(nameof(ArcLength), typeof(short), typeof(CircularWidgetView), DEFAULT_ARC_LENGTH, BindingMode.OneWay, ValidateValueDelegate, ArcLengthPropertyChanged);

        #endregion

        #region Properties
        /// <summary>
        /// The length of the arc in degrees relative to it's start position. 
        /// For example the value of <see cref="DEFAULT_ARC_LENGTH"/> is 90 and that would be a quarter of a full 360 degree arc.
        /// </summary>
        public short ArcLength
        {
            get => (short)GetValue(ArcLengthProperty);
            set => SetValue(ArcLengthProperty, value);
        }
        #endregion

    }
}
