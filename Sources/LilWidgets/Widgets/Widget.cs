using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using SkiaSharp;

namespace LilWidgets.Widgets
{
    public abstract class Widget : INotifyPropertyChanged
    {
        /// <summary>
        /// The background color for all widgets.
        /// </summary>   
        public SKColor BackgroundColor { get; set; } = SKColors.Yellow;
        
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when the chart is invalidated.
        /// </summary>
        public event EventHandler Invalidated;

        /// <summary>
        /// Draws content to canvas.
        /// </summary>
        /// <param name="canvas">The canvas provided by the platform specific project.</param>
        /// <param name="width">Width of the canvas.</param>
        /// <param name="height">Height of the canvas.</param
        public void Draw(SKCanvas canvas, in SKRectI rect)
        {           
            canvas.Clear(BackgroundColor);
            

            DrawContent(canvas, rect);
        }

        public abstract void DrawContent(SKCanvas canvas, in SKRectI rect);

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        /// <summary>
        /// Adds a weak event handler to observe invalidate changes.
        /// </summary>
        /// <param name="target">The target instance.</param>
        /// <param name="onInvalidate">Callback when chart is invalidated.</param>
        /// <typeparam name="TTarget">The target subscriber type.</typeparam>
        public InvalidatedWeakEventHandler<TTarget> ObserveInvalidate<TTarget>(TTarget target, Action<TTarget> onInvalidate)
            where TTarget : class
        {
            var weakHandler = new InvalidatedWeakEventHandler<TTarget>(this, target, onInvalidate);
            weakHandler.Subsribe();
            return weakHandler;
        }
    }
}
