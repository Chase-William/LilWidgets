using System;
using System.Collections.Generic;
using System.Text;

namespace LilWidgets.Interfaces
{
    public interface ILoadingWidgetBindables : ICircularWidgetBindables
    {
        short ArcLength { get; set; }
    }
}
