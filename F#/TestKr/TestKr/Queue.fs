module Queue

open System.Collections.Generic

/// Очередь с приоритетом
type QueueWithPriority<'T>() = 
    let mutable queue = new List<'T * int>()
    /// Вставить элемент x с приоритетом priority
    member this.Enqueue (x, priority : int) =
        queue.Insert(queue.Count, (x, priority))
    /// Взятие первого элемента с большим приоритетом
    member this.Dequeue() = 
        if (queue.Count = 0) then failwith "queue is empty"
        let findMaxPriority = Seq.maxBy(fun x -> snd x) queue 
        let res = queue.Remove(findMaxPriority)
        fst findMaxPriority
    /// Проверка на пустоту очереди
    member this.IsEmpty() = queue.Count = 0
    