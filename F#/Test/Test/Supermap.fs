///Map, для каждого значения исходного списка можно задать не одно, а несколько значений, на которые его надо заменить
module Supermap

/// list список, значения которого меняем
/// func функция, на значения которой меняем
let supermap list func = 
    let rec mapTemp listNow listResult = 
        match listNow with 
        | h::tail -> 
            let tempRes = func h
            mapTemp tail (listResult @ tempRes)
        | [] -> listResult
    mapTemp list []

