module TelephoneTest

open Microsoft.VisualStudio.TestTools.UnitTesting
open Telephone
open FsUnit

[<TestClass>]
type TestClass () =
    let list = [("Olya", "12-12-12"); ("Vitya", "21-43-90"); ("Sveta", "43-56-94"); ("Roma", "04-43-78"); ("Slava","85-45-32"); ("Olya","23-44-76")]

    [<TestMethod>]
    member this.``Telephone findPhone correct work`` () = 
        findPhone "Olya" list |> should equal ["12-12-12"; "23-44-76"]


    [<TestMethod>]
    member this.``Telephone findName correct work`` () = 
        Telephone.findName "12-12-12" list |> should equal ["Olya"]
        Telephone.findName "43-56-94" list |> should equal ["Sveta"]

    