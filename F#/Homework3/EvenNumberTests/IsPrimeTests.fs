module IsPrimeTests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member this.``PrimeSeq 3 should equal seq [2; 3; 5]`` () =
        IsPrime.primeSeq |> Seq.take 3 |> should equal (seq [2; 3; 5])

    [<TestMethod>]
    member this.``PrimeSeq 10 should equal seq [2; 3; 5; 7; 11; 13; 17; 19; 23; 29]`` () =
        IsPrime.primeSeq |> Seq.take 10 |> should equal (seq [2; 3; 5; 7; 11; 13; 17; 19; 23; 29])

    [<TestMethod>]
    member this.``SeqPrime 3 should equal seq [2; 3; 5]`` () =
        IsPrime.seqPrime |> Seq.take 3 |> should equal (seq [2; 3; 5])

    [<TestMethod>]
    member this.``SeqPrime 10 should equal seq [2; 3; 5; 7; 11; 13; 17; 19; 23; 29]`` () =
        IsPrime.seqPrime |> Seq.take 10 |> should equal (seq [2; 3; 5; 7; 11; 13; 17; 19; 23; 29])
