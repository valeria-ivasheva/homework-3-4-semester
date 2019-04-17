namespace MapTreeTest

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open MapForTree
open FsUnit

[<TestClass>]
type TestClass () =

    let tempTree = (Tree (2, Tree(4, Tip, Tip), Tree(-3, Tip, Tip)))

    let tempTree2 = (Tree ("a", Tree("b", Tip, Tip), Tree("c", Tip, Tip)))

    let temp a = 
        let rec tempStr str size = 
            match size with
            | _ when (size < 0) -> ""
            | _ when (size = 0) -> str
            |  _ -> tempStr (str + a) (size - 1)
        tempStr "" 3

    [<TestMethod>]
    member this.``MapTree tempTree with fun x -> x + 1`` () =
        mapTree tempTree (fun x -> x + 1) |> should equal (Tree (3, Tree(5, Tip, Tip), Tree(-2, Tip, Tip)))

    [<TestMethod>]
    member this.``MapTree tempTree with fun x -> x * x`` () =
        mapTree tempTree (fun x -> x * x) |> should equal (Tree (4, Tree(16, Tip, Tip), Tree(9, Tip, Tip)))

    [<TestMethod>]
    member this.``MapTree tempTree2 with fun temp`` () =
        mapTree tempTree2 temp |> should equal (Tree ("aaa", Tree("bbb", Tip, Tip), Tree("ccc", Tip, Tip)))