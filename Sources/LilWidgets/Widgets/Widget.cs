/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See repository root directory for more info.
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
        #region Constants
        /// <summary>
        /// Default duration for animations.
        /// </summary>
        public const uint DEFAULT_DURATION_VALUE = 2000;
        #endregion

        #region Properties With Backing Fields

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
                    OnInvalidateAnimation();
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
                    OnNotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Callback to be used by the platform specific projects.
        /// </summary>
        public Action<double> AnimateCallback { get; protected set; }
        #endregion

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

        /// <summary>
        /// Restarts a running animation or starts the animation.
        /// </summary>
        protected void RestartAnimation()
        {
            if (IsAnimating)
                OnInvalidateAnimation();
            else
                IsAnimating = true;
        }

        /// <summary>
        /// Draws content to canvas.
        /// </summary>
        /// <param name="canvas">The canvas provided by the platform specific repository.</param>
        /// <param name="width">Width of the canvas.</param>
        /// <param name="height">Height of the canvas.</param>
        public void Draw(SKCanvas canvas, in SKRectI rect)
        {           
            canvas.Clear(SKColors.Transparent);

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
        /// Notifies specific subscribers of a change.
        /// </summary>
        /// <param name="prop">Property that changed.</param>
        protected virtual void OnNotifyPropertyChanged([CallerMemberName] string prop = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        /// <summary>
        /// Notifies the source that the <see cref="IsAnimating"/> property has been changed in the backing library by invoking <see cref="IsAnimatingChanged"/>.
        /// </summary>
        protected virtual void OnInvalidateAnimation()
            => IsAnimatingChanged?.Invoke(this, new IsAnimatingChangedEventArgs(IsAnimating));

        /// <summary>
        /// Invalidates the source's canvas by invoking the <see cref="Invalidated"/> event.
        /// </summary>
        protected virtual void OnInvalidateCanvas()
            => Invalidated?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Updates a targeted field with the given value if they are not already equal. 
        /// This also applies to the calling of <see cref="OnNotifyPropertyChanged(string)"/>, meaning it won't be called if the field was not given a new value.    
        /// </summary>
        /// <typeparam name="T">The target type which is derived from <paramref name="field"/>.</typeparam>
        /// <param name="field">Field to be targeted.</param>
        /// <param name="value">Value to be assigned to <paramref name="field"/>.</param>
        /// <param name="notifyPropertyChanged">Allows the prevention of <see cref="INotifyPropertyChanged"/> being invoked.</param>
        /// <param name="prop">Property responsible for <see cref="INotifyPropertyChanged"/> being raised.</param>        
        /// <returns>Indication if <paramref name="value"/> was assigned to <paramref name="field"/>.</returns>
        protected bool Set<T>(ref T field, T value, bool notifyPropertyChanged = true, [CallerMemberName] string prop = "")
        {
            if (!Equals(field, prop))
            {
                field = value;
                if (notifyPropertyChanged)
                    OnNotifyPropertyChanged(prop);
                return true;
            }
            return false;
        }        
    }
}
