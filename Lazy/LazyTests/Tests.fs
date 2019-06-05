namespace LazyTests

open System
open LazyTask
open System.Threading
open FsUnit
open Microsoft.VisualStudio.TestTools.UnitTesting
open System.Threading.Tasks
open System.ComponentModel

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member this.SimpleOneThreadTest () =
        let lazyTemp = LazyFactory.CreateSingleThreadedLazy(fun () -> 21)
        lazyTemp.Get() |> should equal 21

    [<TestMethod>]
    member this.CalculateOneThreadTest () =
        let x = 4
        let lazyTemp = LazyFactory.CreateSingleThreadedLazy(fun () -> x * x)
        lazyTemp.Get() |> should equal 16

    [<TestMethod>]
    member this.GetCalculateOnlyOnceTest () =
        let mutable count = 0
        let lazyTemp = LazyFactory.CreateSingleThreadedLazy(fun () -> 
                count <- count + 1
                16)
        lazyTemp.Get() |> should equal 16
        count |> should equal 1
        lazyTemp.Get() |> should equal 16
        count |> should equal 1

    [<TestMethod>]
    member this.RaceTest () =
        let worker = new BackgroundWorker()
        let numIterations = 100
        worker.DoWork.Add(fun args -> 
            let lazyTemp = LazyFactory.CreateMultiThreadedLazy(fun () -> 21)
            args.Result <- box (lazyTemp.Get()))
        worker.RunWorkerCompleted.Add(fun args ->
            args.Result |> should equal 12)
        worker.RunWorkerAsync()

    [<TestMethod>]
    member this.GetCalculateOnlyOnceMultiTest () =
        let mutable count = 0
        let lazyTemp = LazyFactory.CreateMultiThreadedLazy(fun () -> 
                count <- count + 1
                16)
        lazyTemp.Get() |> should equal 16
        count |> should equal 1
        lazyTemp.Get() |> should equal 16
        count |> should equal 1