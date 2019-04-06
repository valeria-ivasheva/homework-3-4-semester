///Записать в point-free стиле func x l = List.map (fun y -> y * x) l
module Task2

let func x l = List.map (fun y -> y * x) l

let func1 x = List.map (fun y -> y * x)

let func2 = List.map << (*) 

