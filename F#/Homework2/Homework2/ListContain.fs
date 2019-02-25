module ListContain

let contain list item =
    if (List.isEmpty list) then None
    else
        let rec findItem list number=
            match list with
            | [] -> None
            | _ when List.head list = item -> Some(number)
            | _ -> findItem list.Tail (number + 1) 
        findItem list 0
