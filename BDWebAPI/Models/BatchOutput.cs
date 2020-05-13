using BDWebAPI.Models.Entities;
using System.Collections.Generic;

namespace BDWebAPI.Models
{
    public class BatchOutput
    {
        public BatchOutput()
        {
            BatchList = new List<Batch>();
        }
        public int CurrentGroupId { get; set; }

        public IEnumerable<Batch> BatchList { get; set; }

        public bool IsProcessCompleted { get; set; }

    }
}
