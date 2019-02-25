open System

[<EntryPoint>]
let main argv =
    let str = "a n N a"
    let res = Palindrome.isPalindrome str
    printf "%A" (res)
    0 
