using Xamarin.Forms;

using LilWidgets.Widgets;

namespace LilWidgets.Forms.Views
{
    public class LoadingWidgetView : CircularWidgetView
    {
        #region Constant Properties
        /// <summary>
        /// Default length of the arc.
        /// </summary>
        const short DEFAULT_ARC_LENGTH = 90;
        #endregion

        #region Bind-able Properties
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="ArcLength"/> property.
        /// </summary>
        public static readonly BindableProperty ArcLengthProperty = BindableProperty.Create(nameof(ArcLength), typeof(short), typeof(CircularWidgetView), DEFAULT_ARC_LENGTH, BindingMode.OneWay);

        #endregion

        #region Properties
        /// <summary>
        /// Length of the arc in degrees relative to it's start position. 
        /// For example the value of <see cref="DEFAULT_ARC_LENGTH"/> is 90 and that would be a quarter of a full 360 degree arc.
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
    }
}