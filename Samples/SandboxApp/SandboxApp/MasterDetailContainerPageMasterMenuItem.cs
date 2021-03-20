using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxApp
{

    public class MasterDetailContainerPageMasterMenuItem
    {
        public MasterDetailContainerPageMasterMenuItem()
            => TargetType = typeof(MasterDetailContainerPageMasterMenuItem);
        
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}