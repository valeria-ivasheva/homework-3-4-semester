// Learn more about F# at http://fsharp.org

open Arithmetic

[<EntryPoint>]
let main argv =
    let temp = Plus (Number(23), Number(2))
    printfn "%A" (calculate temp)
    0 // return an integer exit code
    