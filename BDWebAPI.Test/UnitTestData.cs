using BDWebAPI.Models;
using BDWebAPI.Models.Entities;
using System;
using System.Collections.Generic;

namespace BDWebAPI.Test
{
    public abstract class UnitTestData
    {
        
        protected readonly List<Batch> BatchItems;

        protected readonly BatchOutput BatchOutput;

        protected readonly BatchInput BatchInput;

        private static readonly Batch Batch1 = new Batch() { Id = new Guid("799089b2-ec50-4d9e-9dd2-c68a6c481d8b"), GroupId = 1, BatchId = 1, TotalProcessedItem = 2, TotalRemainingItem = 0, Total = 100 };
        private static readonly Batch Batch2 = new Batch() { Id = new Guid("3a3b557e-043d-45e6-885a-dfb3d5ad269c"), GroupId = 1, BatchId = 3, TotalProcessedItem = 4, TotalRemainingItem = 0, Total = 300 };
        private static readonly Batch Batch3 = new Batch() { Id = new Guid("ef293f3a-ac68-4461-9a05-3160c3dc28aa"), GroupId = 1, BatchId = 2, TotalProcessedItem = 3, TotalRemainingItem = 0, Total = 200 };

        private static readonly Batch Batch4 = new Batch() { Id = new Guid("32b3e5fa-c0f2-4ada-bd48-83bb5e524397"), GroupId = 2, BatchId = 1, TotalProcessedItem = 5, TotalRemainingItem = 0, Total = 400 };
        private static readonly Batch Batch5 = new Batch() { Id = new Guid("5b7d26c3-e3d6-480f-abeb-6bf8078e02b2"), GroupId = 2, BatchId = 2, TotalProcessedItem = 2, TotalRemainingItem = 0, Total = 500 };
        private static readonly Batch Batch6 = new Batch() { Id = new Guid("7f712dd5-b07d-4bf4-8463-122b82484b5e"), GroupId = 3, BatchId = 1, TotalProcessedItem = 3, TotalRemainingItem = 0, Total = 600 };



        protected UnitTestData()
        {
            BatchItems = new List<Batch>() { Batch1, Batch2, Batch3, Batch4, Batch5, Batch6 };
            BatchOutput = new BatchOutput()
            {
                BatchList = BatchItems,
                CurrentGroupId = 1,
                IsProcessCompleted = true
            };
            BatchInput = new BatchInput() { BatchSize = 2, ItemsPerBatch = 2 };

            

        }
    }
}
