namespace HomeworkTests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Task2
open FsUnit
open FsCheck

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member this.``Check func2`` () = 
        Check.Quick (func2)

    [<TestMethod>]
    member this.``Check func1`` () = 
        Check.Quick (func1)
    
    [<TestMethod>]
    member this.``Check func`` () = 
        Check.Quick (func)

    