/// Задача 3.4 (бесконечная последовательность простых чисел)
module IsPrime

/// Проверка на простоту числа x
let isPrime x = 
    if (x < 2) then false
    else
        let count = int (float x ** (0.5))
        let rec isPrimeRec i = 
            if (count < i ) then true
            else 
                if (x % i = 0) then false
                else isPrimeRec (i + 1)
        isPrimeRec 2

/// Получить бесконечную последовательность простых чисел
let primeSeq = 
    let rec prime n = 
        seq {
        if isPrime n then yield n
        yield! prime (n + 1)
        }
    prime 2

/// Получить бесконечную последовательность простых чисел через initInfinite
let seqPrime = 
    Seq.initInfinite(fun x -> x) |> Seq.filter isPrime