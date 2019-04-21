/// Задача 7.1
module RoundeWorkflow

open System

/// Округлить число x до accuracy
let rounde x accuracy =
    let temp = Math.Pow(10.0, accuracy)
    let res = Math.Round (x * temp)
    res / temp

/// Workflow, выполняющий математические вычисления с заданной точностью
type RoundeBuilder(accuracy : float) = 
    member this.Bind(x, f) = 
        let res = f x
        rounde res accuracy
    member this.Return(x) = x