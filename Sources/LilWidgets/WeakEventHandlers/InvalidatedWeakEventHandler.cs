/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
*/

/// <summary>
/// Source of this file is from https://github.com/dotnet-ad/Microcharts/blob/ceb087633a730f238794827d71281ebd8a3e2066/Sources/Microcharts/WeakEventHandlers/InvalidatedWeakEventHandler.cs
/// </summary>
namespace LilWidgets.WeakEventHandlers
{
    using LilWidgets.Widgets;
    using System;

    /// <summary>
    /// A lightweight proxy instance that will subscribe to a given event with a weak reference to the subscribed target.
    /// If the subscriber is garbage collected, then only this WeakEventHandler will remain subscribed and keeped
    /// in memory instead of the actual subscriber.
    /// This could be considered as an acceptable solution in most cases.
    /// </summary>
    public class InvalidatedWeakEventHandler<TTarget> : WeakEventHandler<TTarget> where TTarget : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidatedWeakEventHandler{TTarget}"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="targetMethod">The target method.</param>
        public InvalidatedWeakEventHandler(Widget source, TTarget target, Action<TTarget> targetMethod)
            : base(source, target, targetMethod) { }
             
        /// <summary>
        /// Subscribe this handler to the source.
        /// </summary>
        public override void Subscribe()
        {
            if (!isSubscribed && sourceReference.TryGetTarget(out Widget source))
            {
                source.Invalidated += OnEvent;
                this.isSubscribed = true;
            }
        }

        /// <summary>
        /// Unsubscribe this handler from the source.
        /// </summary>
        public override void Unsubscribe()
        {
            if (isSubscribed)
            {
                if (sourceReference.TryGetTarget(out Widget source))                
                    source.Invalidated -= OnEvent;                

                isSubscribed = false;
            }
        }                
    }
}