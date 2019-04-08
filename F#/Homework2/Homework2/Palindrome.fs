/// Задача 2.2 (Проверить является ли строка палиндромом)
module Palindrome

/// Перевернуть список
let reverse list = 
    if (List.length list = 0) then []
    else
        let rec rev lst accList = 
            if (List.length lst = 0) then accList
            else
                rev (List.tail lst) ((List.head lst) :: accList)
        rev list []

/// Проверка является ли строка палиндромом
let isPalindrome str = 
    let stringIntoList = Seq.toList (str.ToString().ToLower())
    let correctList = List.filter (fun x -> x <> ' ') stringIntoList
    let reverseStr = reverse correctList
    reverseStr = correctList


