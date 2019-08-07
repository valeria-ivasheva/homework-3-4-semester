namespace Tests

open System
open FsUnit
open Interpreter
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type TestClass () =
    let tempExpression = Application(Lambda("x", Variable("x")), Variable("y"))

    [<TestMethod>]
    member this.``BetaTransform (l.x.y)(l.x.x x x) (l.x.x x x) should equal y`` () =
        let tempTerm = Lambda("x", Application(Variable("x"), Application(Variable("x"), Variable("x"))))
        let term = Application(Lambda("x", Variable("y")), Application(tempTerm, tempTerm))
        betaTransform term |> should equal (Variable("y"))

    [<TestMethod>]
    member this.``BetaTransform K I should equal K*`` () =
        let term = Application(Lambda("x", Lambda("y", Variable("x"))), Lambda("x", Variable("x")))
        betaTransform term |> should equal (Lambda("y", Lambda("x", Variable("x"))))

    [<TestMethod>]
    member this.``FindToRename {"x", "a"} should equal "b"`` () =
        let free =  findToRename (Set.empty.Add("x").Add("a"))
        free |> should equal "b"

    [<TestMethod>]
    member this.``FindFree tempExpression should equal {"y"}`` () =
        let free =  findFree tempExpression
        free |> should equal (Set.empty.Add("y"))

    [<TestMethod>]
    member this.``FindVariable tempExpression should equal {"x", "y"}`` () =
        let free =  findVariable tempExpression
        free |> should equal (Set.empty.Add("y").Add("x"))

    [<TestMethod>]
    member this.``Replace one should equal x`` () =
        let one = replace "y" (Variable("x")) (Application(Lambda("x", Application(Variable("a"), Variable("y"))), Variable("y")))
        one |> should equal (Variable("x"))

    [<TestMethod>]
    member this.``Id id should equal id"`` () =
        let term = Lambda("x", Application(Lambda("x", Variable("x")), Variable("x")))
        betaTransform term |> should equal (Lambda("a", Variable("a")))