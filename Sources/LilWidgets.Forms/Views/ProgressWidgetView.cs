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

        public static readonly BindableProperty IsTextVisibleProperty = BindableProperty.Create(nameof(IsTextVisibleProperty), typeof(bool), typeof(ProgressWidgetView), ProgressWidget.DEFAULT_IS_TEXT_VISIBLE, BindingMode.OneWay, propertyChanged: OnIsTextVisibleChanged);

        public float ProgressPercentage
        {
            get => (float)GetValue(ProgressPercentageProperty);
            set => SetValue(ProgressPercentageProperty, value);
        }
    
        /// <summary>
        /// Initializes a new <see cref="ProgressWidgetView"/> instance.
        /// </summary>
        public ProgressWidgetView()
            => UnderlyingWidget = new ProgressWidget();        
       
        /// <summary>
        /// Starts the animation.
        /// </summary>
        public void Start()
            => IsAnimating = true;

        protected override void StartInternal()
        {            
            var widget = (ProgressWidget)UnderlyingWidget;
            var test = widget.GetRelativeDuration();
            Animator = new Animation(widget.AnimateCallback, widget.CurrentProgressPercentage, ProgressPercentage);
            Animator.Commit(this, nameof(ProgressWidget), length: widget.GetRelativeDuration());
        }

        protected override void StopInternal()
            => this.AbortAnimation(nameof(ProgressWidget));        

        private static void OnProgressPercentageChanged(BindableObject bindable, object oldValue, object newValue)
            => bindable.GetCastedWidgetView<ProgressWidgetView>().GetCastedWidget<ProgressWidget>().ProgressPercentage = (float)newValue;

        private static void OnIsTextVisibleChanged(BindableObject bindable, object oldValue, object newValue)
            => bindable.GetCastedWidgetView<ProgressWidgetView>().GetCastedWidget<ProgressWidget>().IsTextVisible = (bool)newValue;
    }
}            