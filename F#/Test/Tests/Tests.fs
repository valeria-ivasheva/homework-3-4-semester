namespace Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit
open Supermap

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member this.``Supermap [2; 3; 5] [fun x -> x + 1] [3; 4; 6]`` () =
        let res = Supermap.supermap [2; 3; 5] (fun x -> [x + 1])
        res |> should equal [3; 4; 6]

    [<TestMethod>]
    member this.``Supermap [2; 3; 5] (fun x -> [x + 1; x * 2])`` () =
        let res = Supermap.supermap [2; 3; 5] (fun x -> [x + 1; x * 2])
        res |> should equal [3; 4; 4; 6; 6; 10]

    [<TestMethod>]
    member this.``Supermap [aaa;aaa] should equal seq [2; 3; 5]`` () =
        let res = Supermap.supermap ["aaa";"bb"] (fun x -> [x + "qqq"; x + "123"])
        res |> should equal ["aaaqqq";"aaa123";"bbqqq";"bb123"]

    [<TestMethod>]
    member this.``Supermap [1.0; 2.0; 3.0] (fun x -> [sin x; cos x])`` () =
        let res = supermap [1.0; 2.0; 3.0] (fun x -> [sin x; cos x])
        res |> should equal [sin 1.0; cos 1.0; sin 2.0; cos 2.0; sin 3.0; cos 3.0]
        
        