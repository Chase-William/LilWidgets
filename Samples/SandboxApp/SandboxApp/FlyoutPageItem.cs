using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxApp
{

    public class FlyoutPageItem
    {
        public FlyoutPageItem()
            => TargetType = typeof(FlyoutPageItem);
        
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}