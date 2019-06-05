open System

[<EntryPoint>]
let main argv =
    let list = [1; 2; 3]
    let res = MergeSort.mergesort list
    printf "%A" (res)
    0 
