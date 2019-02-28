module MergesortTests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit

[<TestClass>]
type MergesortTests () = 

    [<TestMethod>]
    member this.``[1; 2; 3] mergesort should equal [1; 2; 3]`` () =
        MergeSort.mergesort [1; 2; 3] |> should equal [1; 2; 3]
    
    [<TestMethod>]
    member this.``[] mergesort should equal []`` () =
        MergeSort.mergesort [] |> should equal []

    [<TestMethod>]
    member this.``[23] mergesort should equal [23]`` () =
        MergeSort.mergesort [23] |> should equal [23]

    [<TestMethod>]
    member this.``[23; 9; -12; 22; 23; 3; 0] mergesort should equal [-12; 0; 3; 9; 22; 23; 23]`` () =
        MergeSort.mergesort [23; 9; -12; 22; 23; 3; 0] |> should equal [-12; 0; 3; 9; 22; 23; 23]

    [<TestMethod>]
    member this.``[-12; 9; 22; 23; 23] [-3; 0; 9; 26] merge should equal [-12; -3; 0; 9; 9; 22; 23; 23; 26]`` () =
        MergeSort.merge [-12; 9; 22; 23; 23] [-3; 0; 9; 26] |> should equal [-12; -3; 0; 9; 9; 22; 23; 23; 26]

    [<TestMethod>]
    member this.``[-12; -3; 0; 9; 9; 22; 23; 23; 26] split should equal ([-12; -3; 0; 9; 9], [22; 23; 23; 26])`` () =
        MergeSort.split [-12; -3; 0; 9; 9; 22; 23; 23; 26] |> should equal ([-12; -3; 0; 9; 9], [22; 23; 23; 26])

