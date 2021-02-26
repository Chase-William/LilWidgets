using System;
using System.Collections.Generic;
using System.Text;

namespace LilWidgets.Exceptions
{
    public class UnassignedException : Exception
    {
        public UnassignedException(string propName) : base($"The property {propName} must be assigned a proper value.") { }
    }
}
