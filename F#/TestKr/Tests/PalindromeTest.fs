module PalindromeTest

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsUnit

[<TestClass>]
type PalindromeTests () = 
    [<TestMethod>]
    member this.``Need Number is 906609`` () = 
        Palindrome.maxPalindromeNumber = 906609

    [<TestMethod>]
    member this.``Is palindrome 1111 should equal true`` () =
        Palindrome.isPalindrome 1111 |> should equal true

    [<TestMethod>]
    member this.``Is palindrome "Anna" should equal true`` () =
        Palindrome.isPalindrome "Anna" |> should equal true

    [<TestMethod>]
    member this.``Is palindrome "A n Na" should equal true`` () =
        Palindrome.isPalindrome "A n Na" |> should equal true

    [<TestMethodAttribute>]
    member this.``Is palindrome "A tn Na" should equal false`` () =
        Palindrome.isPalindrome "A tn Na" |> should equal false

    [<TestMethod>]
    member this.``Is palindrome "я иду с мечем судия" should equal true`` () =
        Palindrome.isPalindrome "я иду с мечем судия" |> should equal true

    [<TestMethod>]
    member this.``Is palindrome "я идУ с мЕЧем судиЯ" should equal true`` () =
        Palindrome.isPalindrome "я идУ с мЕЧем судиЯ" |> should equal true

    [<TestMethod>]
    member this.``Empty string should equal true`` () =
        Palindrome.isPalindrome "" |> should equal true

    [<TestMethod>]
    member this.``Reverse empty list should equal []`` () = 
        Palindrome.reverse [] |> should equal []

    [<TestMethod>]
    member this.``Reverse [1; 2; 3; 4; 5] should equal [5; 4; 3; 2; 1]`` () = 
        Palindrome.reverse [1; 2; 3; 4; 5] |> should equal [5; 4; 3; 2; 1] 