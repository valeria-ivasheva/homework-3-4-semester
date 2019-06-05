/// Задача 8.1 (Ленивые вычисления)
module LazyTask

open System.Threading
open System

/// Интерфейс, представляющий ленивое вычисление
/// Первый вызов Get() вызывает вычисление и возвращает результат
[<AbstractClassAttribute>]
type ILazy<'a>() =
    abstract member Get: unit -> 'a

/// Класс Lazy, работающий в однопоточном режиме
type Lazy<'a> (suplier : unit -> 'a) = 
    inherit ILazy<'a>()
    let func = suplier
    let mutable result = None
    let mutable isResultCalculated = false
    override this.Get() = 
        if (not isResultCalculated) 
        then 
            result <- Some(func())
            isResultCalculated <- true
        result.Value

/// Класс Lazy, работающий в многопоточном режиме
type MultiThreadLazy<'a> (suplier: unit -> 'a) = 
    inherit ILazy<'a>()
    let func = suplier
    let mutable result = None
    let mutable isResultCalculated = false
    let lockobj  = new Object()
    override this.Get() = 
        if (not isResultCalculated)
        then
            Monitor.Enter lockobj
            try
                if (not isResultCalculated)
                then
                    result <- Some(func())
                    isResultCalculated <- true               
            finally
            Monitor.Exit lockobj
        result.Value

/// Класс Lazy, работающий в многопоточном lock-free режиме
type MultiThreadLockLazy<'a>(suplier: unit -> 'a) = 
    inherit ILazy<'a>()
    let func = suplier
    let mutable result = None
    override this.Get() = 
        let rec loopLockFree () = 
            let current = result
            let newResult = Some(func())
            if not <| obj.ReferenceEquals(current, Interlocked.CompareExchange(&result, newResult, current))
            then loopLockFree ()
            else result.Value
        loopLockFree ()
             
/// Класс, создающий объект Lazy
type LazyFactory = 
    static member CreateSingleThreadedLazy<'a> supplier = new Lazy<'a>(supplier) :> ILazy<'a>
    static member CreateMultiThreadedLazy<'a> supplier = 
        new MultiThreadLazy<'a>(supplier) :> ILazy<'a>
    static member CreateMultiThreadLockLazy<'a> supplier = 
        new MultiThreadLockLazy<'a>(supplier) :> ILazy<'a>
    
