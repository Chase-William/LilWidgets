/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See project root directory for more info.
*/

using System;

using LilWidgets.Widgets;

namespace LilWidgets.WeakEventHandlers
{
    public abstract class WeakEventHandler<TTarget> : IDisposable where TTarget : class
    {

        protected readonly WeakReference<Widget> sourceReference;

        protected readonly WeakReference<TTarget> targetReference;

        protected readonly Action<TTarget> targetMethod;

        protected bool isSubscribed;


        /// <summary>
        /// Gets a value indicating whether this <see cref="WeakEventHandler{TTarget}"/> is alive.
        /// </summary>
        /// <value><c>true</c> if is alive; otherwise, <c>false</c>.</value>
        public bool IsAlive => this.sourceReference.TryGetTarget(out Widget s) && this.targetReference.TryGetTarget(out TTarget t);

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakEventHandler{TTarget}"/> class.
        /// </summary>
        /// <param name="_source">The source.</param>
        /// <param name="_target">The target.</param>
        /// <param name="_targetMethod">The target method.</param>
        public WeakEventHandler(Widget _source, TTarget _target, Action<TTarget> _targetMethod)
        {
            sourceReference = new WeakReference<Widget>(_source);
            targetReference = new WeakReference<TTarget>(_target);
            targetMethod = _targetMethod;
        }

        /// <summary>
        /// Unsubscribe this handler from the source.
        /// </summary>
        public abstract void Unsubscribe();

        /// <summary>
        /// Subscribe this handler to the source.
        /// </summary>
        public abstract void Subscribe();

        /// <summary>
        /// Releases all resource used by the <see cref="T:LilWidgets.InvalidatedWeakEventHandler`1"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the
        /// <see cref="T:Microcharts.InvalidatedWeakEventHandler`1"/>. The <see cref="Dispose"/> method leaves the
        /// <see cref="T:Microcharts.InvalidatedWeakEventHandler`1"/> in an unusable state. After calling
        /// <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:Microcharts.InvalidatedWeakEventHandler`1"/> so the garbage collector can reclaim the memory
        /// that the <see cref="T:Microcharts.InvalidatedWeakEventHandler`1"/> was occupying.</remarks>
        public void Dispose() => this.Unsubscribe();

        protected void OnEvent(object sender, EventArgs args)
        {
            var type = this.GetType();

            if (this.targetReference.TryGetTarget(out TTarget t))
                this.targetMethod(t);
            else
                this.Unsubscribe();
        }
    }
}
