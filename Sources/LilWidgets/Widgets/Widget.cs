/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
*/

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using SkiaSharp;

using LilWidgets.WeakEventHandlers;
using LilWidgets.EventArguments;

namespace LilWidgets.Widgets
{
    /// <summary>
    /// A <see cref="Widget"/> class that is the highest parent to all other derived widget classes.
    /// </summary>
    public abstract class Widget : INotifyPropertyChanged
    {
        /// <summary>
        /// Default duration for animations.
        /// </summary>
        public const uint DEFAULT_DURATION_VALUE = 2000;

        /// <summary>
        /// Gets or sets the background color for a <see cref="Widget"/>.
        /// </summary>   
        public SKColor BackgroundColor { get; set; } = SKColors.Transparent;

        private bool isAnimating = false;
        /// <summary>
        /// Gets or sets whether the animation is animating.
        /// </summary>
        public bool IsAnimating
        {
            get => isAnimating;
            set
            {
                if (Set(ref isAnimating, value)) // If animation state has changed raise event
                    IsAnimatingChanged?.Invoke(this, new IsAnimatingChangedEventArgs(value));
            }
        }

        private uint duration = DEFAULT_DURATION_VALUE;
        /// <summary>
        /// Gets or sets the time in milliseconds for a one complete cycle of the animation.
        /// </summary>
        public uint Duration
        {
            get => duration;
            set => Set(ref duration, value);
        }

        /// <summary>
        /// Notifies subscribers that a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies subscribers that the drawing canvas has been invalidated.
        /// </summary>
        public event EventHandler Invalidated;

        /// <summary>
        /// Notifies subscribers that the <see cref="IsAnimating"/> property has changed.
        /// </summary>
        public event EventHandler<IsAnimatingChangedEventArgs> IsAnimatingChanged;

        public Action<double> AnimateCallback { get; protected set; }

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
                {
                    drawingRect = value;
                    OnCanvasRectChanged(DrawingRect);
                    NotifyPropertyChanged();
                }
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

        /// <summary>
        /// Handles the canvas changing sizes.
        /// </summary>
        /// <param name="rect"></param>
        protected abstract void OnCanvasRectChanged(in SKRectI rect);

        /// <summary>
        /// Initializes a new <see cref="WeakEventHandler{TTarget}"/> instance of the specified derived type. Before returned, the <see cref="WeakEventHandler{TTarget}"/> is
        /// subscribed to the source to receive updates from the event the <see cref="WeakEventHandler{TTarget}"/> targets.
        /// </summary>
        /// <typeparam name="THandler"><see cref="WeakEventHandler{TTarget}"/> derived type.</typeparam>
        /// <typeparam name="TTarget"><see cref="class"/> that is the source type.</typeparam>
        /// <param name="target"><see cref="class"/> instance that is of the <typeparamref name="TTarget"/> type.</param>
        /// <param name="onChanged"><see cref="Action{T}"/> callback that will be invoked when updates occur.</param>
        /// <returns>Subscribed <see cref="WeakEventHandler{TTarget}"/> of derived type given.</returns>
        public THandler ObserveChanges<THandler, TTarget>(TTarget target, Action<TTarget> onChanged) where TTarget : class where THandler : WeakEventHandler<TTarget>
        {            
            var weakHandler = (THandler)Activator.CreateInstance(typeof(THandler), this, target, onChanged);                             
            weakHandler.Subscribe();
            return weakHandler;
        }

        /// <summary>
        /// Invalidates the source's canvas by invoking the <see cref="Invalidated"/> event.
        /// </summary>
        protected void Invalidate()
            => Invalidated?.Invoke(this, EventArgs.Empty);

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
