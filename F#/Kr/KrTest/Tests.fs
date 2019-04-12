namespace KrTest

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit
open SumFib

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member this.``SumFib 10 should equal 4`` () =
        sumFib 10 |> should equal 10
            
    [<TestMethod>]
    member this.``Fibonacci 4 should equal 3`` () =
        fibonacci 4 |> should equal 3

    