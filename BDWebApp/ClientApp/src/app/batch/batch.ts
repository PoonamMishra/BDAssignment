export interface IBatch {
  batchId?: number;
  totalProcessedItem?: number;
  totalRemainingItem?: number;
  total?: number;

}


export interface IBatchOutput extends IBatch{ 

  isProcessCompleted?: boolean,
  BatchList: IBatch[]
}
