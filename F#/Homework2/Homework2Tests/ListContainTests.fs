namespace Homework2Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit

[<TestClass>]
type ListContainTests () =

    let tempList = [2; 3; -12; 32; 43; -12]

    [<TestMethod>]
    member this.``Contain empty list should equal None`` () =
        ListContain.contain [] -2 |> should equal None

    [<TestMethod>]
    member this.``Contain [2; 3; -12; 32; 43; -12] 32 should equal 3`` () =
        ListContain.contain tempList 32 |> should equal (Some 3)
    
    [<TestMethod>]
    member this.``Contain [2; 3; -12; 32; 43; -12] -12 should equal 2`` () =
        ListContain.contain tempList -12 |> should equal (Some 2)

    [<TestMethod>]
    member this.``Contain [2; 3; -12; 32; 43; -12] 2 should equal 0`` () =
        ListContain.contain tempList 2 |> should equal (Some 0)
    
    [<TestMethod>]
    member this.``Contain [2; 3; -12; 32; 43; -12] 12 should equal -102`` () =
        ListContain.contain tempList -102 |> should equal None


