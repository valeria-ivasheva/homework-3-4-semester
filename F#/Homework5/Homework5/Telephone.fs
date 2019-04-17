/// Задача 5.3 - телефонный справочник
module Telephone

open System.IO
open System.Runtime.Serialization.Formatters.Binary
open System

/// Распечатать информацию об операциях
let printInfo () = 
    printfn ("1. выйти")
    printfn ("2. добавить запись (имя и телефон)")
    printfn ("3. найти телефон по имени")
    printfn ("4. найти имя по телефону")
    printfn ("5. вывести всё текущее содержимое базы")
    printfn ("6. сохранить текущие данные в файл")
    printfn ("7. считать данные из файла")

/// Добавить запись (имя и телефон)
let addContact name phone list = (name, phone) :: list

/// Найти телефон по имени
let findPhone name list = 
    List.map (snd) (List.filter (fun (nameContact, _) -> nameContact = name) list)

/// Найти имя по телефону
let findName phone list = 
    List.map (fst) (List.filter (fun (_, phoneContact) -> phoneContact = phone) list)

/// Вывести всё текущее содержимое базы"
let rec printBase list = 
    match list with
    | [] -> printfn "END"
    | (name, phone) :: tail -> 
        let result = "Name " + name + " Phone " + phone
        printfn "%s" result
        printBase tail

/// Сохранить текущие данные в файл
let saveBase contacts = 
    let formatter = new BinaryFormatter()
    use fsOut = new FileStream("ContactsBase.dat", FileMode.Create)
    formatter.Serialize(fsOut, box contacts)

/// Считать данные из файла
let openBase = 
    let formatter = new BinaryFormatter()
    try
        use fsIn = new FileStream("ContactsBase.dat", FileMode.Open)
        let res = formatter.Deserialize(fsIn)
        match res with
        | :? ((string * string)list) as l -> l
        | _ -> raise(ArgumentException("Error type"))
    with
       | :? ArgumentException -> raise(ArgumentException("Error"))
       | :? FileNotFoundException -> raise(FileNotFoundException("Can't find file"))

/// Запускает программу в интерактивном режиме со списком контактов contact
let run contact = 
    printInfo()
    let rec interactive contact = 
        let command = Console.ReadLine()
        match command with
        | "1" -> ignore
        | "2" -> 
            printfn "Write name "
            let name = Console.ReadLine()
            printfn "Wtite phone "
            let phone = Console.ReadLine()
            interactive (addContact name phone contact)
        | "3" -> 
            printfn "Write name "
            let name = Console.ReadLine()
            let result = findPhone name contact
            printfn "%A" result
            interactive contact
        | "4" ->
            printfn "Write phone "
            let phone = Console.ReadLine()
            let result = findName phone contact
            printfn "%A" result
            interactive contact
        | "5" ->
            printBase contact
            interactive contact
        | "6" ->
            saveBase contact
            interactive contact
        | "7" ->
            interactive (openBase)
        | _ -> 
            printfn "Error command"
            printInfo()
            interactive contact
    interactive contact






