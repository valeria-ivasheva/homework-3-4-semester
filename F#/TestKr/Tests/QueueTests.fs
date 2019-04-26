module QueueTests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit
open Queue


[<TestClass>]
type QueueTests () = 
    let queue = QueueWithPriority<string>()

    [<TestMethod>]
    member this.``Queue is Empty`` () = 
        queue.IsEmpty() |> should equal true

    [<TestMethod>]
    member this.``Queue.Dequeue fail with empty queue`` () =
        (fun () -> queue.Dequeue() |> ignore) |> should throw typeof<System.Exception>

    [<TestMethod>]
    member this.``Queue.Enqueue tests`` () =
        queue.Enqueue("12", 1)
        queue.Enqueue("123", 2)
        queue.Enqueue("1234", 2)
        queue.Enqueue("12345", 3)
        queue.Dequeue() |> should equal "12345"

    [<TestMethod>]
    member this.``Queue.Dequeue tests`` () =
        queue.Enqueue("12", 1)
        queue.Enqueue("123", 2)
        queue.Enqueue("1234", 2)
        queue.Enqueue("12345", 3)
        queue.Dequeue() |> should equal "12345"
        queue.Dequeue() |> should equal "123"