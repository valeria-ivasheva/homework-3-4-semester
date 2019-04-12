module SquareTest

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit
open Square

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member this.``square 0 should equal [""]`` () =
        square 0 |> should equal [""]
    
    [<TestMethod>]
    member this.``square 0 should equal ["*"]`` () =
        square 1 |> should equal ["*"]

    
    

            

