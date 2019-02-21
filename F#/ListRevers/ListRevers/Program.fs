let reverse list = 
    if (List.length list = 0) then []
    else
        let rec rev lst accList = 
            if (List.length lst = 0) then accList
            else
                rev (List.tail lst) ((List.head lst) :: accList)
        rev list []

let list = reverse [1; 2; 3 ; 4; 5; 6]
printf "%A" list



