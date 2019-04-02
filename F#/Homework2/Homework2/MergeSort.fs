/// Задача 2.3 (сортировка слиянием)
module MergeSort

/// Слияние упорядоченное
let rec merge left right =
    match (left, right) with
    | (l, []) -> l
    | ([], l) -> l
    | (l :: left, r :: right) -> 
        if (l <= r) then l :: (merge left (r :: right))
        else r :: (merge (l :: left) right)

/// Сортировка слиянием 
let rec mergesort list = 
    match list with  
    | _ when (List.length list < 2) -> list
    | _ -> 
        let averageLength = (List.length list) / 2 + (List.length list) % 2 
        let left, right = List.splitAt averageLength list
        let sortLeft = mergesort left
        let sortRight = mergesort right
        merge sortLeft sortRight 

