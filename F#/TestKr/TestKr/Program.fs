// Learn more about F# at http://fsharp.org

open System
open SeqInfinite
open Queue

[<EntryPoint>]
let main argv =
    let queue = new QueueWithPriority<string>()
    queue.Enqueue("12", 1)
    queue.Enqueue("123", 2)
    queue.Enqueue("1234", 2)
    queue.Enqueue("12345", 3)
    let temp = queue.Dequeue()
    printfn "%A" temp
    0 // return an integer exit code
