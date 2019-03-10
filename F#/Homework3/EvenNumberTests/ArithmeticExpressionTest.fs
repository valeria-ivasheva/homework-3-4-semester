namespace ArithmeticExpressionTest

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Arithmetic
open FsUnit

[<TestClass>]
type TestClass () =

    let tempExp = Plus (Number(23), Number(2))
    let tempExpOne = Minus (tempExp, Number(3))
    
    [<TestMethod>]
    member this.``Calculate 23 + 2 should equal 25`` () =
        calculate tempExp |> should equal 25
    
    [<TestMethod>]
    member this.``Calculate (23 + 2) - 3 should equal 22`` () =
        calculate tempExpOne |> should equal 22
    
    [<TestMethod>]
    member this.``Calculate ((23 + 2) - 3) / 11 should equal 2`` () =
        calculate (Division(tempExpOne, Number(11))) |> should equal 2

    [<TestMethod>]
    member this.``Calculate ((23 + 2) - 3) / (11 * 2) should equal 1`` () =
        calculate (Division(tempExpOne, Multiple(Number(11), Number(2)))) |> should equal 1

    [<TestMethod>]
    member this.``Calculate ((23 + 2) - 3) / 0 should equal 2`` () =
        (fun () -> calculate (Division(tempExpOne, Number(0))) |> ignore ) |> should throw typeof<System.Exception>