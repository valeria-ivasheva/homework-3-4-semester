module Square

let square n = 
    if (n < 0) then ignore()
    if (n = 0) then [""]
    elif (n = 1) then ["*"]
    else
        let rec granStr count (strGran, strIn) = 
            if (count > 1) then 
               granStr (count - 1) (strGran + "*", strIn + " ")
            else 
                (strGran + "*", strIn + "*")
        let granStr, inStr = granStr (n - 1) ("*","*")
        let rec result count res = 
            if (count = 1) then 
                granStr :: res
            else
                result (count - 1) (inStr :: res)
        result (n - 1) [granStr]

let printSquare n = 
    let squareNow = square n
    let rec print temp = 
        if (List.isEmpty temp) then
            printf "%s" temp.Head
            print (temp.Tail)  
    print squareNow


        
        
