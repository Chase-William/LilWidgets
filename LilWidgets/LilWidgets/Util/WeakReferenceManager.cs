using System;
using System.Collections.Generic;
using System.Text;

namespace LilWidgets.Util
{
    public static class WeakReferenceManager
    {
        public static List<WeakReference<object>> references = new List<WeakReference<object>>();
    }
}
