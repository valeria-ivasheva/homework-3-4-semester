namespace Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit
open SeqInfinite

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member this.``Seq.take 6 seqNeed should equal [1, -2, 3, -4, 5, -6]`` () =
        Seq.take 6 seqNeed |> should equal [1; -2; 3; -4; 5; -6]

    [<TestMethod>]
    member this.``Seq.take 6 seqSimple should equal [1; -1; 1; -1; 1; -1]`` () =
        Seq.take 6 seqSimple |> should equal [1; -1; 1; -1; 1; -1]
