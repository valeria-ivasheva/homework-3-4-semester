module Palindrome

/// Переворачивает список
let reverse list = 
    if (List.length list = 0) then []
    else
        let rec rev lst accList = 
            if (List.length lst = 0) then accList
            else
                rev (List.tail lst) ((List.head lst) :: accList)
        rev list []

/// Проверка является ли нечто палиндромом
let isPalindrome str = 
    let str = str.ToString()
    let stringIntoList = Seq.toList (str.ToString().ToLower())
    let correctList = List.filter (fun x -> x <> ' ') stringIntoList
    let reverseStr = reverse correctList
    reverseStr = correctList

/// Последовательность чисел палиндромов
let seqNum = 
    seq {for oneNumber in 100..999 do
          for two in 100..999 do
             if isPalindrome((oneNumber * two)) then yield (oneNumber * two)}

/// Нужное число
let maxPalindromeNumber = Seq.max seqNum