/// Задача 5.1 (проверка корректности скобок)
module Brackets

/// Возвращает нужную открывающую скобку
let returnOpenBrackets brk = 
    match brk with 
    |')' -> Some('(')
    |'}' -> Some('{')
    |']' -> Some('[')
    | _  -> None

///  Проверка на корректность последовательности скобок
let isCorrectBrackets str = 
    let listStr = Seq.toList str
    let rec correct listStr (stack) = 
        let closeBcktFun bct =
            if (List.isEmpty stack) then false
            else
                (List.head stack) = (returnOpenBrackets bct).Value
        let openBcktFun bct = 
            let stack = bct :: stack
            correct (List.tail listStr) stack
        if (List.isEmpty listStr)
        then List.isEmpty stack
        elif (List.head listStr = '{' ||  List.head listStr = '(' || List.head listStr = '[')
        then 
            openBcktFun (List.head listStr)
        elif (List.head listStr = '}' || List.head listStr = ')' || List.head listStr = ']')
        then 
            let result = closeBcktFun (List.head listStr)
            if (result) 
            then correct listStr.Tail stack.Tail
            else false
        else
            correct listStr.Tail stack
    correct listStr []


        