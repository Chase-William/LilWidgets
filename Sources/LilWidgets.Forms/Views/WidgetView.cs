/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
*/

using Xamarin.Forms;

using SkiaSharp.Views.Forms;
using SkiaSharp;

using LilWidgets.Widgets;
using LilWidgets.WeakEventHandlers;

namespace LilWidgets.Forms.Views
{
    /// <summary>
    /// A <see cref="WidgetView"/> class that is the highest parent to all other derived widget-view classes.
    /// </summary>
    public abstract class WidgetView : SKCanvasView
    {
        /// <summary>
        /// <see cref="BindableProperty"/> for the <see cref="IsAnimating"/> property.
        /// </summary>
        public static readonly BindableProperty IsAnimatingProperty = BindableProperty.Create(nameof(IsAnimating), typeof(bool), typeof(StrokedEquilateralWidgetView), false, BindingMode.OneWay, propertyChanged: OnAnimatingPropertyChanged);

        private Widget underlyingWidget;
        /// <summary>
        /// Gets or sets Underlying <see cref="Widget"/> that the <see cref="WidgetView"/> interfaces with.
        /// </summary>
        internal Widget UnderlyingWidget
        {
            get => underlyingWidget;
            set
            {
                if (UnderlyingWidget == value) return;
                underlyingWidget = value;
                AttachWidgetToWidgetViewObserving();
            }
        }

        /// <summary>
        /// Indicates the state of the animation.
        /// True == Running, False == Inactive
        /// </summary>
        public bool IsAnimating
        {
            get => (bool)GetValue(IsAnimatingProperty);
            protected set => SetValue(IsAnimatingProperty, value);
        }

        /// <summary>
        /// Propagates <see cref="Widget.Invalidated"/> events from the <see cref="Widget"/> to this class.
        /// </summary>
        private InvalidatedWeakEventHandler<WidgetView> invalidationHandler;
        /// <summary>
        /// Propagates <see cref="Widget.IsAnimatingChanged"/> events from the <see cref="Widget"/> to this class.
        /// </summary>
        private AnimatingWeakEventHandler<WidgetView> animationHandler;
        
        /// <summary>
        /// Gets or sets the <see cref="Xamarin.Forms.Animation"/> controller.
        /// </summary>
        protected Animation Animator { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WidgetView"/> class.
        /// </summary>
        public WidgetView()
        {
            BackgroundColor = Color.Transparent;
            PaintSurface += OnPaintCanvas;
        }

        /// <summary>
        /// Starts the animation.
        /// </summary>
        public virtual void Start() => IsAnimating = true;

        /// <summary>
        /// Starts the <see cref="Animator"/>.
        /// </summary>
        protected abstract void StartInternal();

        /// <summary>
        /// Stops the <see cref="Animator"/>.
        /// </summary>
        protected abstract void StopInternal();      

        /// <summary>
        /// Triggers the underlying libraries draw methods.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            if (UnderlyingWidget != null)
            {
                var rect = e.Info.Rect;
                UnderlyingWidget.Draw(e.Surface.Canvas, in rect);
            }
            else
                e.Surface.Canvas.Clear(SKColors.Transparent);
        }

        /// <summary>
        /// Updates the <see cref="UnderlyingWidget"/>'s <see cref="Widget.IsAnimating"/> property.
        /// </summary>
        /// <param name="bindable"><see cref="WidgetView"/> instance.</param>
        /// <param name="oldValue">Old <see cref="IsAnimating"/> value.</param>
        /// <param name="newValue">New <see cref="IsAnimating"/> value.</param>
        private static void OnAnimatingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => bindable.GetCastedWidgetView<WidgetView>().GetCastedWidget<Widget>().IsAnimating = (bool)newValue;

        /// <summary>
        /// Attaches <see cref="WeakEventHandler{TTarget}"/> to the <see cref="UnderlyingWidget"/>.
        /// </summary>
        private void AttachWidgetToWidgetViewObserving()
        {
            if (UnderlyingWidget != null)
            {
                if (invalidationHandler != null) // Clean up
                {
                    invalidationHandler.Dispose();
                    invalidationHandler = null;
                }                    
                if (animationHandler != null) // Clean up
                {
                    animationHandler.Dispose();
                    animationHandler = null;
                }                
            }           

            InvalidateSurface();

            if (UnderlyingWidget != null) // Bind to the underlying widget's events using our weak handlers
            {
                invalidationHandler = UnderlyingWidget.ObserveChanges<InvalidatedWeakEventHandler<WidgetView>, WidgetView>(this, (v) => v.InvalidateSurface());
                animationHandler = UnderlyingWidget.ObserveChanges<AnimatingWeakEventHandler<WidgetView>, WidgetView>(this, 
                    (v) => {
                        if (!UnderlyingWidget.IsAnimating) // The UnderlyingWidget instructs this class when to start/stop animating
                            v.StopInternal();
                        else
                            v.StartInternal();
                    });
            }                
        }
    }
}
