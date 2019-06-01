// Learn more about F# at http://fsharp.org

open System
open DownloadAsync

[<EntryPoint>]
let main argv =
    let res = downloadAsync "http://spisok.math.spbu.ru/" 
    printfn "%d" res.Length
    0
