using BDWebAPI.Models.Entities;
using System;
using System.Collections.Generic;

namespace BDWebAPI.Models
{
    public class BatchOutput
    {
        public BatchOutput()
        {
            BatchList = new List<Batch>();
        }
        public int GroupBatchId { get; set; }

        public List<Batch> BatchList { get; set; }

        public int Total { get; set; }

        public bool IsProcessCompleted { get; set; }

    }
}
