namespace DownloadTests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open DownloadAsync
open FsUnit

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member this.``Check download is works test`` () =
        let res = downloadAsync "http://se.math.spbu.ru" 
        res.Length |> should equal 1
        (List.item 0 res).Value |> String.length |> should equal 217


        
