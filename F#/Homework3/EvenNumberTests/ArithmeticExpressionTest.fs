namespace ArithmeticExpressionTest

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Arithmetic
open FsUnit

[<TestClass>]
type TestClass () =

    let tempExpression = Plus (Number(23), Number(2))
    let tempExpressionOne = Minus (tempExpression, Number(3))
    
    [<TestMethod>]
    member this.``Calculate 23 + 2 should equal 25`` () =
        calculate tempExpression |> should equal 25
    
    [<TestMethod>]
    member this.``Calculate (23 + 2) - 3 should equal 22`` () =
        calculate tempExpressionOne |> should equal 22
    
    [<TestMethod>]
    member this.``Calculate ((23 + 2) - 3) / 11 should equal 2`` () =
        calculate (Division(tempExpressionOne, Number(11))) |> should equal 2

    [<TestMethod>]
    member this.``Calculate ((23 + 2) - 3) / (11 * 2) should equal 1`` () =
        calculate (Division(tempExpressionOne, Multiple(Number(11), Number(2)))) |> should equal 1

    [<TestMethod>]
    member this.``Calculate ((23 + 2) - 3) / 0 should equal 2`` () =
        (fun () -> calculate (Division(tempExpressionOne, Number(0))) |> ignore ) |> should throw typeof<System.Exception>