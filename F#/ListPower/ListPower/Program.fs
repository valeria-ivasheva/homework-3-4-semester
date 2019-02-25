module Func

exception InputException of string

let power deg x = 
    if (deg < 0) then raise (InputException("Степень должна быть натуральной"))
    else
        let rec pow a deg acc = 
            if deg = 0 then acc
            else
                if deg % 2 = 0 then
                   pow (a * a) (deg / 2) acc
                else
                    pow a (deg - 1) (acc * a)
        pow x deg 1
        

let listPower m n = 
    if (m < 0 || n < 0) then None
    else
        let rec lstPow count acc = 
            if count = -1 then acc
            else 
                let x = power (m + count) 2
                lstPow (count - 1) (x :: acc)
        Some(lstPow n [])

let result = listPower 5 5
match result with
| Some result -> printf "%A" result
| None -> printf "Числа m или n должны быть неотрицательными"
    