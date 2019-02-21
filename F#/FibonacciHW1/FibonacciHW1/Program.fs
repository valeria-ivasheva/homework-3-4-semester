let Fibonacci n = 
    if n <= 0 then 
        -1
    else 
        let rec fib x temp acc = 
            if x <= 2 then acc
            else 
                fib (x - 1) acc (acc + temp)
        fib n 1 1

[<EntryPoint>]
let main argv =
    let f = Fibonacci 22
    printf "Фибоначчи номер 22 %d" f
    0