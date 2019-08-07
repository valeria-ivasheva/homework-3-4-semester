module TestsBrackets

open Microsoft.VisualStudio.TestTools.UnitTesting
open Brackets
open FsUnit
open FsCheck

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member this.``IsCorrectBrackets ({}) should be True`` () = 
        isCorrectBrackets "({})" |> should be True

    [<TestMethod>]
    member this.``IsCorrectBrackets (({} should be False`` () = 
        isCorrectBrackets "(({}" |> should be False
    
    [<TestMethod>]
    member this.``IsCorrectBrackets (} should be False`` () = 
        isCorrectBrackets "(}" |> should be False

    [<TestMethod>]
    member this.``IsCorrectBrackets (ss{dsa})[] should be True`` () = 
        isCorrectBrackets "(ss{dsa})[]" |> should be True
  