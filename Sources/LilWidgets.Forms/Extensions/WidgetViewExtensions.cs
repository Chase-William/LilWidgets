using Xamarin.Forms;

using LilWidgets.Forms.Views;
using LilWidgets.Widgets;

namespace LilWidgets.Forms
{
    /// <summary>
    /// A <see cref="WidgetViewExtensions"/> class that provides commonly needed functions.
    /// </summary>
    internal static class WidgetViewExtensions
    {
        /// <summary>
        /// Cast a given <paramref name="bindable"/> to the <typeparamref name="T"/> safely. If the cast fails, null will be returned. 
        /// </summary>
        /// <typeparam name="T">Type to cast <paramref name="bindable"/> to.</typeparam>
        /// <param name="bindable">To be casted.</param>
        /// <returns>Cast result.</returns>
        public static T GetCastedWidgetView<T>(this BindableObject bindable) where T : WidgetView
            => bindable as T;

        /// <summary>
        /// Cast a given <paramref name="widgetView"/> to the <typeparamref name="T"/> safely. If the cast fails, null will be returned.
        /// </summary>
        /// <typeparam name="T">Type to cast <paramref name="widgetView"/> to.</typeparam>
        /// <param name="widgetView">To be casted.</param>
        /// <returns>Cast result.</returns>
        public static T GetCastedWidget<T>(this WidgetView widgetView) where T : Widget
           => widgetView.UnderlyingWidget as T;
    }
}
