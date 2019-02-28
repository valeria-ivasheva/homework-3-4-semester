module MergeSort

let split lst = 
    let averageLength = (List.length lst) / 2 + (List.length lst) % 2 
    let list = List.chunkBySize averageLength lst
    let left = List.item 0 list
    let rigth = List.item 1 list
    (left, rigth)

let rec merge left right =
    match (left, right) with
    | (l, []) -> l
    | ([], l) -> l
    | (l :: left, r :: right) -> 
        if (l <= r) then l :: (merge left (r :: right))
        else r :: (merge (l :: left) right)


let rec mergesort list = 
    match list with  
    | _ when (List.length list < 2) -> list
    | _ -> 
        let left, right = (split list)
        let sortLeft = mergesort left
        let sortRight = mergesort right
        merge sortLeft sortRight 

