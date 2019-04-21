module RoundeTests

open FsUnit
open RoundeWorkflow
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member this.``Rounding 3 2.0/12.0/3.5 should equal 0.048`` () =
        let rounding = new RoundeBuilder(3.0)
        let result =  rounding {
            let! a = 2.0 / 12.0
            let! b = 3.5
            return a / b}
        result |> should equal 0.048

    [<TestMethod>]
     member this.``Rounding 2 2.0/12.0/3.5 should equal 0.05`` () =
        let rounding = new RoundeBuilder(2.0)
        let result =  rounding {
            let! a = 2.0 / 12.0
            let! b = 3.5
            return a / b}
        result |> should equal 0.05