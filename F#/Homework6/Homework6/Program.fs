// Learn more about F# at http://fsharp.org

open System
open Network

let rec print list = 
    match list with 
    | (h):: tail -> 
        Console.Write(h.ToString() + " ")
        print tail
    | [] -> ()


[<EntryPoint>]
let main argv =
    let matrix = [ [true; false; true];
                    [false; true; true];
                    [true; true; true ] ]
    let compMac = new Computer(Mac, false)
    let compLinux = new Computer(Linux, true)
    let compWindows = new Computer(Windows, false)
    let computers = [compLinux; compMac; compWindows]
    let rndGen = new Random(int DateTime.Now.Ticks)
    let net = new Network(computers, matrix, rndGen)
    net.NextStep()
    let temp = net.CheckInfectedComputers
    print temp
    0
    


