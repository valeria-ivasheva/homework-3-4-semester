namespace PowerTestProject

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member this.``ListPower -3 3 should equal None`` () =
        Func.listPower -3 3 |> should equal None
    
    [<TestMethod>]
    member this.``ListPower 3, 5 should be equal [8; 16; 32; 64; 128: 256]`` () = 
        let expected = Some [8; 16; 32; 64; 128; 256]
        Func.listPower 3 5 |> should equal expected
        [1; 2; 4; 8]
    [<TestMethod>]
    member this.``ListPower 0, 3 should equal [1; 2; 4; 8]`` () = 
        let expected = Some [1; 2; 4; 8];
        Func.listPower 0 3 |> should equal expected

    [<TestMethodAttribute>]
    member this.``Power 2^(-3) should equal None`` () = 
        Func.power -3 2 |> should equal None

    [<TestMethodAttribute>]
    member this.``Power 2^3 should equal 8`` () = 
        Func.power 3 2 |> should equal (Some 8)

    [<TestMethodAttribute>]
    member this.``Power 133^0 should equal 1`` () = 
        Func.power 0 133 |> should equal (Some 1)
   
