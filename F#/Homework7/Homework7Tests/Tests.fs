namespace Homework7Tests

open System
open FsUnit
open CalculatorWorkflow
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member this.``Calculate "11 * 2" should equal 22`` () =
        let calculate = new CalculatorBuilder()
        let result = calculate {
            let! x = "11"
            let! y = "2"
            let z = x * y
            return z}
        result.Value |> should equal 22

    [<TestMethod>]
     member this.``Calculate "1 + 2" should equal 3`` () = 
        let calculate = new CalculatorBuilder()
        let result = calculate {
            let! x = "1"
            let! y = "2"
            let z = x + y
            return z}
        result.Value |> should equal 3

    [<TestMethodAttribute>]
    member this.``Calculate "1 + Ú" should equal None`` () = 
        let calculate = new CalculatorBuilder()
        let res = calculate {
            let! x = "1"
            let! y = "Ú"
            let z = x + y
            return z}
        res.IsNone |> should be True