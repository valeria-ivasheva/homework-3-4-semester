/// Задача 8.1 (Ленивые вычисления)
module LazyTask

open System.Threading
open System

/// Интерфейс, представляющий ленивое вычисление
/// Первый вызов Get() вызывает вычисление и возвращает результат
type ILazy<'a> =
    abstract member Get: unit -> 'a

/// Класс Lazy, работающий в однопоточном режиме
type Lazy<'a> (suplier : unit -> 'a) = 
    let mutable result = None
    interface ILazy<'a> with
        member this.Get() = 
            if (result.IsNone) 
            then 
                result <- Some(suplier())
                result.Value
            else 
                result.Value

/// Класс Lazy, работающий в многопоточном режиме
type MultiThreadLazy<'a> (suplier: unit -> 'a) = 
    let func = suplier
    let mutable result = None
    [<VolatileField>]
    let mutable isResultCalculated = false
    let lockobj  = new Object()
    interface ILazy<'a> with
         member this.Get() = 
            if (not isResultCalculated)
            then
                lock lockobj (fun() -> 
                if (not isResultCalculated)
                    then
                        result <- Some(func())
                        isResultCalculated <- true  
                )
            result.Value
        
/// Класс Lazy, работающий в многопоточном lock-free режиме
type MultiThreadLockLazy<'a>(suplier: unit -> 'a) = 
    let func = suplier
    let mutable result = None
    interface ILazy<'a> with
          member this.Get() = 
            let current = result
            let newResult = Some(func())
            Interlocked.CompareExchange(&result, newResult, current) |> ignore
            result.Value
             
/// Класс, создающий объект Lazy
type LazyFactory = 
    static member CreateSingleThreadedLazy<'a> supplier = new Lazy<'a>(supplier) :> ILazy<'a>
    static member CreateMultiThreadedLazy<'a> supplier = 
        new MultiThreadLazy<'a>(supplier) :> ILazy<'a>
    static member CreateMultiThreadLockLazy<'a> supplier = 
        new MultiThreadLockLazy<'a>(supplier) :> ILazy<'a>
    
