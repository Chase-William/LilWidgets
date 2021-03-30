/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
*/

using Xamarin.Forms;

using LilWidgets.Widgets;

namespace LilWidgets.Forms.Views
{
    public class ProgressWidgetView : StrokedEquilateralWidgetView
    {
        public static readonly BindableProperty ProgressPercentageProperty = BindableProperty.Create(nameof(ProgressPercentage), typeof(float), typeof(ProgressWidgetView), ProgressWidget.DEFAULT_PROGRESS_PERCENTAGE, BindingMode.OneWay, propertyChanged: OnProgressPercentageChanged);

        public static readonly BindableProperty IsTextVisibleProperty = BindableProperty.Create(nameof(IsTextVisible), typeof(bool), typeof(ProgressWidgetView), ProgressWidget.DEFAULT_IS_TEXT_VISIBLE, BindingMode.OneWay, propertyChanged: OnIsTextVisibleChanged);

        public static readonly BindableProperty AutoAnimateProperty = BindableProperty.Create(nameof(AutoAnimate), typeof(bool), typeof(ProgressWidgetView), ProgressWidget.DEFAULT_AUTO_ANIMATE, BindingMode.OneWay, propertyChanged: OnAutoAnimateChanged);

        public float ProgressPercentage
        {
            get => (float)GetValue(ProgressPercentageProperty);
            set => SetValue(ProgressPercentageProperty, value);
        }

        public bool IsTextVisible
        {
            get => (bool)GetValue(IsTextVisibleProperty);
            set => SetValue(IsTextVisibleProperty, value);
        }

        public bool AutoAnimate
        {
            get => (bool)GetValue(AutoAnimateProperty);
            set => SetValue(AutoAnimateProperty, value);
        }

        /// <summary>
        /// Initializes a new <see cref="ProgressWidgetView"/> instance.
        /// </summary>
        public ProgressWidgetView()
            => UnderlyingWidget = new ProgressWidget();               

        /// <summary>
        /// Initializes a new <see cref="Animation"/> and calls <see cref="Animation.Commit(IAnimatable, string, uint, uint, Easing, System.Action{double, bool}, System.Func{bool})"/> to start the animation.
        /// </summary>
        protected override void StartInternal()
        {
            if (this.AnimationIsRunning(nameof(ProgressWidget))) // Check if animation is already running.s
                StopInternal(); // Abort current animation to start new

            var widget = (ProgressWidget)UnderlyingWidget;
            var test = widget.GetRelativeDuration();
            Animator = new Animation(callback: widget.AnimateCallback, start: widget.CurrentProgressPercentage, end: ProgressPercentage);
            Animator.Commit(this, nameof(ProgressWidget), length: widget.GetRelativeDuration());
        }

        /// <summary>
        /// Aborts the current running animation.
        /// </summary>
        protected override void StopInternal()
            => this.AbortAnimation(nameof(ProgressWidget));        

        /// <summary>
        /// Updates the underlying <see cref="ProgressWidget.ProgressPercentage"/> property to match <see cref="ProgressPercentage"/>.
        /// </summary>
        /// <param name="bindable"><see cref="ProgressWidgetView"/> instance.</param>
        /// <param name="oldValue">Old <see cref="ProgressPercentage"/> value.</param>
        /// <param name="newValue">New <see cref="ProgressPercentage"/> value.</param>
        private static void OnProgressPercentageChanged(BindableObject bindable, object oldValue, object newValue)
            => bindable.GetCastedWidgetView<ProgressWidgetView>().GetCastedWidget<ProgressWidget>().ProgressPercentage = (float)newValue;
        /// <summary>
        /// Updates the underlying <see cref="ProgressWidget.IsTextVisible"/> property to match
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnIsTextVisibleChanged(BindableObject bindable, object oldValue, object newValue)
            => bindable.GetCastedWidgetView<ProgressWidgetView>().GetCastedWidget<ProgressWidget>().IsTextVisible = (bool)newValue;

        /// <summary>
        /// Todo: Fillout Comment
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnAutoAnimateChanged(BindableObject bindable, object oldValue, object newValue)
            => bindable.GetCastedWidgetView<ProgressWidgetView>().GetCastedWidget<ProgressWidget>().AutoAnimate = (bool)newValue;
    }
}            