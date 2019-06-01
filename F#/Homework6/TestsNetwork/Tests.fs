namespace TestsNetwork

open System
open Network
open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit
open System.Drawing
open System.Drawing

type TempRandom (x : double) =
       inherit Random()
       override is.NextDouble() = x

[<TestClass>]
type TestClass () =
    let matrix = [ [true; false; true];
               [false; true; true];
               [true; true; true ] ]
    let compMac = new Computer(Mac, false)
    let compLinux = new Computer(Linux, true)
    let compWindows = new Computer(Windows, false)
    let computers = [compLinux; compMac; compWindows]

    [<TestMethod>]
    member this.``CheckInfectedComputers test`` () =
        let rndGen = new System.Random(int DateTime.Now.Ticks)
        let net = new Network(computers, matrix, rndGen)
        let result = net.CheckInfectedComputers
        result |> should equal [true; false; false]

    [<TestMethod>]
    member this.``Network infection test`` () =
        let rnd = new TempRandom(0.9)
        let net = new Network(computers, matrix, rnd)
        net.NextStep()
        let result = net.CheckInfectedComputers
        result |> should equal [true; false; true]
        net.NextStep()
        let result = net.CheckInfectedComputers
        result |> should equal [true; true; true]
