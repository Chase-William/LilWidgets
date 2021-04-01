/*
 * Copyright (c) Chase Roth <cxr6988@rit.edu>
 * Licensed under the MIT License. See repository root directory for more info.
*/

using System;
using System.Runtime.CompilerServices;

using LilWidgets.Enumerations;

namespace LilWidgets.Exceptions
{
    /// <summary>
    /// A <see cref="PropertyValueOutOfRangeException"/> class replaces <see cref="ArgumentOutOfRangeException"/> but for properties exclusively.
    /// Yes, property setters and getters boil down to methods under the hood, but I want to as clear and concise as possible.
    /// </summary>
    public class PropertyValueOutOfRangeException : Exception
    {
        /// <summary>
        /// Invalid value that caused the exception.
        /// </summary>
        public object Value { get; private set; }
        /// <summary>
        /// Minimum valid value for <see cref="Value"/>.
        /// </summary>
        public object MinValue { get; private set; }
        /// <summary>
        /// Maximum valid value for <see cref="Value"/>.
        /// </summary>
        public object MaxValue { get; private set; }
        /// <summary>
        /// Describes <see cref="MinValue"/> as <see cref="RangeType.Inclusive"/> or <see cref="RangeType.Exclusive"/>.
        /// </summary>
        public RangeType MinRangeType { get; private set; }
        /// <summary>
        /// Describes <see cref="MaxValue"/> as <see cref="RangeType.Inclusive"/> or <see cref="RangeType.Exclusive"/>.
        /// </summary>
        public RangeType MaxRangeType { get; private set; }
        /// <summary>
        /// Sender and who is responsible for the exception.
        /// </summary>
        public string SenderName { get; private set; }

        /// <summary>
        /// Initializes a new <see cref="PropertyValueOutOfRangeException"/> instance.
        /// </summary>
        /// <param name="value">Invalid value that caused the exception.</param>
        /// <param name="minValue">Minimum valid value for <paramref name="value"/>.</param>
        /// <param name="maxValue">Maximum valid value for <paramref name="value"/>.</param>
        /// <param name="minRangeType"><paramref name="minValue"/> is <see cref="RangeType.Inclusive"/> or <see cref="RangeType.Exclusive"/>. Default <see cref="RangeType.Inclusive"/>.</param>
        /// <param name="maxRangeType"><paramref name="maxValue"/> is <see cref="RangeType.Inclusive"/> or <see cref="RangeType.Exclusive"/>. Default <see cref="RangeType.Inclusive"/>.</param>
        /// <param name="senderName">Sender and who is responsible for the exception.</param>
        public PropertyValueOutOfRangeException(object value,
                                                object minValue,
                                                object maxValue,
                                                RangeType minRangeType = RangeType.Inclusive,
                                                RangeType maxRangeType = RangeType.Inclusive,
                                                [CallerMemberName] string senderName = "")
        {
            Value = value;
            MinValue = minValue;
            MaxValue = maxValue;
            MinRangeType = minRangeType;
            MaxRangeType = maxRangeType;
            SenderName = senderName;
        }

        /// <summary>
        /// Get a human readable message describing why this <see cref="PropertyValueOutOfRangeException"/> was thrown.
        /// </summary>
        public override string Message => $"Value {Value} for property {SenderName} is outside the valid range of {MinValue} ({MinRangeType}) to {MaxValue} ({MaxRangeType}).";
    }
}
