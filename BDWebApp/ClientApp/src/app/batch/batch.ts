export interface IBatch {
  batchId?: number;
  totalProcessedItem?: number;
  totalRemainingItem?: number;
  total?: number;
  

}


export interface IBatchOutput extends IBatch{
  currentGroupId?: number,
  isProcessCompleted?: boolean,
  batchList: IBatch[]
}
