// Learn more about F# at http://fsharp.org

open System

[<EntryPoint>]
let main argv =
    let str = "a n N a"
    let res = Palindrome.isPalindrome str
    printf "%A" (res)
    0 // return an integer exit code
