// Learn more about F# at http://fsharp.org

open System

[<EntryPoint>]
let main argv =
    let b = IsPrime.primeSeq
    printfn "%A" (b)
    0 // return an integer exit code
