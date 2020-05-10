using System;

namespace BDWebAPI.Models
{
    public class BatchInput
    {
        public int BatchSize { get; set; }

        public int ItemsPerBatch { get; set; }
    }

}
