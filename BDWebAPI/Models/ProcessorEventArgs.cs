using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDWebAPI.Models
{
    public class ProcessorEventArgs
    {
        public int BatchId { get; set; }
        public int ComputedNumber { get; set; }

    }
}
