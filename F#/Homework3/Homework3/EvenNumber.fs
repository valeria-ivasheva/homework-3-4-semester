module EvenNumber

let oneEvenFun list = List.length (List.filter (fun x -> x % 2 = 0) list)

let twoEvenFun list = List.fold (fun count elem -> count + (elem + 1) % 2) 0 list

let threeEvenFun list = List.sum  (List.map (fun elem -> (elem + 1) % 2) list)
