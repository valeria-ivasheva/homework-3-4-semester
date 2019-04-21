/// Задача 7.2
module CalculatorWorkflow

open System

/// Конвертировать число из строки
let convertToInt (x : string) = 
    match Int32.TryParse x with 
        | true, res -> Some(res)
        | _ -> None

/// Workflowд, выполняющий вычисления с числами, заданными в виде строк
type CalculatorBuilder() = 
    member this.Bind(x, f) = 
        let res = convertToInt x
        match res with
        | Some(x) -> f x
        | None -> None
    member this.Return(x) = Some(x)


