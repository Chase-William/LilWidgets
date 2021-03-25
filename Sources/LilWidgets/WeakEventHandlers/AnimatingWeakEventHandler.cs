/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
*/

using System;

using LilWidgets.Widgets;

namespace LilWidgets.WeakEventHandlers
{
    public class AnimatingWeakEventHandler<TTarget> : WeakEventHandler<TTarget> where TTarget : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidatedWeakEventHandler{TTarget}"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="targetMethod">The target method.</param>
        public AnimatingWeakEventHandler(Widget source, TTarget target, Action<TTarget> targetMethod)
            : base(source, target, targetMethod) { }

        /// <summary>
        /// Subscribe this handler to the source.
        /// </summary>
        public override void Subscribe()
        {
            if (!isSubscribed && sourceReference.TryGetTarget(out Widget source))
            {
                source.IsAnimatingChanged += OnEvent;
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
                    source.IsAnimatingChanged -= OnEvent;

                isSubscribed = false;
            }
        }
    }
}
