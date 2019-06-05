module LazyTask

open System.Threading
open System

[<AbstractClassAttribute>]
type ILazy<'a>() =
    abstract member Get: unit -> 'a

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

type MultiThreadLazyLazy<'a> (suplier: unit -> 'a) = 
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

type MultiThreadLockLazy<'a>(suplier: unit -> 'a) = 
    inherit ILazy<'a>()
    let func = suplier
    override this.Get() = func()

type LazyFactory = 
    static member CreateSingleThreadedLazy<'a> supplier = new Lazy<'a>(supplier) :> ILazy<'a>
    static member CreateMultiThreadedLazy<'a> supplier = 
        new MultiThreadLazyLazy<'a>(supplier) :> ILazy<'a>
    static member CreateMultiThreadLockLazy<'a> supplier = 
        new MultiThreadLockLazy<'a>(supplier) :> ILazy<'a>
    
