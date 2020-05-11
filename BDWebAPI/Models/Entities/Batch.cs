using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace BDWebAPI.Models.Entities
{
    public class Batch
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int GroupId { get; set; }

        public int BatchId { get; set; }

        public int TotalProcessedItem { get; set; }

        public int TotalRemainingItem { get; set; }

        public int Total { get; set; }
    }
}
