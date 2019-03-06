/// Задача 3.1 (количество четных чисел в списке)
module EvenNumber

/// С помощью filter
let oneEvenFun list = List.length (List.filter (fun x -> x % 2 = 0) list)

/// С помощью fold
let twoEvenFun list = List.fold (fun count elem -> count + (elem + 1) % 2) 0 list

/// С помощью map
let threeEvenFun list = List.sum  (List.map (fun elem -> (elem + 1) % 2) list)
