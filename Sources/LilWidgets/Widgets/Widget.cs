/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
*/

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SkiaSharp;

namespace LilWidgets.Widgets
{
    /// <summary>
    /// A <see cref="Widget"/> class that is the highest parent to all other derived widget classes.
    /// </summary>
    public abstract class Widget : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the background color for a <see cref="Widget"/>.
        /// </summary>   
        public SKColor BackgroundColor { get; set; } = SKColors.Yellow;

        /// <summary>
        /// Gets or sets whether the animation is animating.
        /// </summary>
        public bool IsAnimating { get; set; }

        /// <summary>
        /// Notifies subscribers that a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies subscribers that the drawing canvas has been invalidated.
        /// </summary>
        public event EventHandler Invalidated;


        private SKRectI drawingRect;
        /// <summary>
        /// Dimensions of the <see cref="SKCanvas"/> that is checked for updates each draw call.
        /// </summary>
        protected SKRectI DrawingRect
        {
            get => drawingRect;
            set
            {
                if (drawingRect != value) // Update if different
                    drawingRect = value;
                OnCanvasRectChanged(DrawingRect);
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Draws content to canvas.
        /// </summary>
        /// <param name="canvas">The canvas provided by the platform specific project.</param>
        /// <param name="width">Width of the canvas.</param>
        /// <param name="height">Height of the canvas.</param
        public void Draw(SKCanvas canvas, in SKRectI rect)
        {           
            canvas.Clear(BackgroundColor);

            // Update rect prop
            DrawingRect = rect;

            DrawContent(canvas, rect);
        }

        /// <summary>
        /// To be overridden by base classes to provide custom drawing implementation.
        /// </summary>
        /// <param name="canvas">To be drawn on.</param>
        /// <param name="rect">Dimensions of the canvas.</param>
        public abstract void DrawContent(SKCanvas canvas, in SKRectI rect);

        /// <summary>
        /// Notifies specific subscribers of a change.
        /// </summary>
        /// <param name="prop">Property that changed.</param>
        protected void NotifyPropertyChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        protected abstract void OnCanvasRectChanged(in SKRectI rect);

        /// <summary>
        /// Adds a weak event handler to observe invalidate changes.
        /// Based off dotnet-ad's microcharts implementation.
        /// </summary>
        /// <param name="target">The target instance.</param>
        /// <param name="onInvalidate">Callback when chart is invalidated.</param>
        /// <typeparam name="TTarget">The target subscriber type.</typeparam>
        public InvalidatedWeakEventHandler<TTarget> ObserveInvalidate<TTarget>(TTarget target, Action<TTarget> onInvalidate)
            where TTarget : class
        {
            var weakHandler = new InvalidatedWeakEventHandler<TTarget>(this, target, onInvalidate);
            weakHandler.Subscribe();
            return weakHandler;
        }

        /// <summary>
        /// Set the <paramref name="field"/> to the given <paramref name="value"/> if they are different.
        /// Will raise the <see cref="PropertyChanged"/> by invoking <see cref="NotifyPropertyChanged(string)"/> if the field's current value is
        /// different than the new.
        /// Based off dotnet-ad's microcharts implementation.
        /// </summary>
        /// <typeparam name="T">Type of <paramref name="field"/> and <paramref name="value"/>.</typeparam>
        /// <param name="field">To be assigned <paramref name="value"/>.</param>
        /// <param name="value">Value to be assigned to <paramref name="field"/>.</param>
        /// <param name="prop">Calling property.</param>
        /// <param name="notifyPropertyChanged">Should call <see cref="NotifyPropertyChanged(string)"/> which invokes <see cref="PropertyChanged"/>.</param>
        /// <returns>Indication of success or failure.</returns>
        protected bool Set<T>(ref T field, T value, [CallerMemberName] string prop = "", bool notifyPropertyChanged = true)
        {
            if (!Equals(field, prop))
            {
                field = value;
                if (notifyPropertyChanged)
                    NotifyPropertyChanged(prop);
                return true;
            }
            return false;
        }
    }
}
