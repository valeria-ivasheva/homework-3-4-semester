// Интерпретатор лямбда-выражений, выполняющий бета-редукцию по нормальной стратегии
module Interpreter

open System.Linq.Expressions
open System

// Тип λ-терм
type Term = 
    | Variable of string
    | Application of Term * Term
    | Lambda of string * Term

// Переменные терма
let rec findVariable term = 
    match term with
    | Variable(x) -> Set.singleton x
    | Application(S, T) -> Set.union (findVariable S) (findVariable T)
    | Lambda(x, S) -> (findVariable S)

// Найти свободные переменные терма
let rec findFree term = 
    match term with 
    | Variable(x) -> Set.singleton x
    | Application(S, T) -> Set.union (findFree S) (findFree T)
    | Lambda(str, T) -> Set.difference (findFree T) (Set.singleton str)

// Найти новое имя, отличное от элементов set
let findToRename set = 
    let letters = List.map (fun x -> x.ToString()) ['a' .. 'z'] 
    let rec find count = 
        let setNew = letters |> List.map (fun x -> if (count = 0) then x else x + count.ToString())
        let setNew = List.filter (fun x -> not (Set.contains x set)) setNew
        if (List.isEmpty setNew) then find (count + 1) else setNew.[0]
    find 0

// Подстановка
// var --- это то, вместо чего подставляем, termNow --- куда подставляем, termNew --- что подставляем
let rec replace var termNow termNew =
    match (termNow, termNew) with
    | (Lambda(y, term), Variable(x)) -> Lambda(y, term)
    | _ ->
         match termNow with
         | Variable(x) -> 
            if (var = x) then termNew else Variable(x)
         | Application(term1, term2) ->
                Application(replace var term1 termNew, replace var term2 termNew)
         | Lambda(x, term) ->
            let free1 = findFree term
            let free2 = findFree termNew
            let cond = (Set.contains x free2) && (Set.contains var free1) //(not (Set.contains x free2)) && (not (Set.contains var free1)) /// ( (Set.contains x free2)) && ( (Set.contains var free1))
            if (not cond) then Lambda(x, replace var term termNew) else
                let free = Set.union free1 free2
                let newName = findToRename free
                let term1 = replace x term (Variable(newName))
                let term2 = replace var term1 termNew
                Lambda(newName, term2)

// Выполняет бета-редукцию по нормальной стратегии
let betaTransform term = 
    let rec betaRedaction term = 
        match term with 
        | Variable(x) -> Variable(x)
        | Application(Lambda(x, term1), term2) -> replace x term1 term2
        | Application(term1, term2) -> 
            let newTerm1 = betaRedaction term1
            match newTerm1 with 
            | Lambda(x, termNow) -> betaRedaction (replace x termNow term2)
            | _ -> Application (newTerm1, betaRedaction term2)
        | Lambda(x, term1) ->
            let free = findFree term1
            if (Set.contains x free) 
            then 
                let newName = findToRename free
                let newTerm = replace x term1 (Variable(newName))
                Lambda(newName, betaRedaction newTerm)
            else 
                Lambda(x, betaRedaction term1)
    betaRedaction term
