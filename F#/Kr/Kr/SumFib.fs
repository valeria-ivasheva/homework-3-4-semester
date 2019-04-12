module SumFib

let fibonacci n = 
    if n <= 0 then 
        -1
    else 
        let rec fib x temp acc = 
            if x <= 2 then acc
            else 
                fib (x - 1) acc (acc + temp)
        fib n 1 1

let seqFibonacci = Seq.initInfinite(fun n -> fibonacci n)

let sumFib max = seqFibonacci |> Seq.filter (fun x -> x % 2 = 0 && x < max) |> Seq.fold (+) 0

let sumFibLessMillion = sumFib 1000000
