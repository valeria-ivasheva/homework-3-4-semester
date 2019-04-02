/// Задача 2.1 (выдает первую позицию вхождения заданного числа в список)
module ListContain

/// Выдает первую позицию вхождения заданного числа в список
let contain list item =
    if (List.isEmpty list) then None
    else
        let rec findItem list number =
            match list with
            | [] -> None
            | h::t when h = item -> Some(number)
            | _ -> findItem list.Tail (number + 1) 
        findItem list 0
