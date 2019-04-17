namespace EvenNumberTests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member this.``OneEvenFun list should equal 4`` () =
        EvenNumber.oneEvenFun [1; 2; 2; 3; 4; 5; 6] |> should equal 4

    [<TestMethod>]
    member this.``TwoEvenFun list should equal 4`` () =
        EvenNumber.twoEvenFun [1; 2; 2; 3; 4; 5; 6] |> should equal 4

    [<TestMethod>]
    member this.``ThreeEvenFun list should equal 4`` () =
        EvenNumber.threeEvenFun [1; 2; 2; 3; 4; 5; 6] |> should equal 4

    [<TestMethod>]
    member this.``OneEvenFun list should equal 0`` () =
        EvenNumber.oneEvenFun [1; 3; 5] |> should equal 0

    [<TestMethod>]
    member this.``TwoEvenFun list should equal 0`` () =
        EvenNumber.twoEvenFun [1; 3; 5] |> should equal 0
    
    [<TestMethod>]
    member this.``ThreeEvenFun list should equal 0`` () =
        EvenNumber.threeEvenFun [1; 3; 5] |> should equal 0