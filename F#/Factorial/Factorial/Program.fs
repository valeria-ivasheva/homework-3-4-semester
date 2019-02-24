let factorial n = 
    if n < 0 then
        -1
    else 
        let rec fact x acc = 
            if x = 0 then acc
            else fact (x - 1) (x * acc) 
        fact n 1

[<EntryPoint>]
let main argv =
    let x = factorial 12
    let print = 
        if x = -1 then printf "Число не должно быть отрицательным\n"
        else printfn "Факториал 12 %d" x
    0 // return an integer exit code