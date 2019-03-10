/// Задача 3.3 (Посчитать значение дерева разбора арифметического выражения)
module Arithmetic

/// АТД арифметическое выражение
type Expression = 
    | Number of int
    | Plus of Expression * Expression
    | Multiple of Expression * Expression
    | Division of Expression * Expression
    | Minus of Expression * Expression

/// Посчитать значение арифметического выражения
let rec calculate (exp : Expression) =
    match exp with
    | Number(n) -> n
    | Plus(a, b) -> (calculate a) + (calculate b)
    | Multiple (a, b) -> (calculate a) * (calculate b)
    | Minus (a, b) -> (calculate a) - (calculate b)
    | Division (a, b) -> 
        let denominator = calculate b
        let numerator = calculate a
        if (denominator = 0) then failwith "Divisor cannot be zero." 
        else (numerator / denominator)
    




