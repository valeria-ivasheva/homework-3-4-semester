module Telephone

open System.IO
open System.Runtime.Serialization.Formatters.Binary
open System

let printInfo  = 
    printf ("1. выйти")
    printf ("2. добавить запись (имя и телефон)")
    printf ("3. найти телефон по имени")
    printf ("4. найти имя по телефону")
    printf ("5. вывести всё текущее содержимое базы")
    printf ("6. сохранить текущие данные в файл")
    printf ("7. считать данные из файла")

let addContact name phone list = (name, phone) :: list

let findPhone name list = 
    List.map (fst) (List.filter (fun (nameContact, _) -> nameContact = name) list)

let findName phone list = 
    List.map (snd) (List.filter (fun (_, phoneContact) -> phoneContact = phone) list)

let printBase list = 
    match list with
    | [] -> printf "END"
    | (name, phone) :: tail -> 
        let result = "Name " + name + "Phone " + phone
        printf "%s" result
 
let saveBase contacts = 
    let formatter = new BinaryFormatter()
    use fsOut = new FileStream("ContactsBase.dat", FileMode.Create)
    formatter.Serialize(fsOut, box contacts)

let openBase = 
    let formatter = new BinaryFormatter()
    use fsIn = new FileStream("ContactsBase.dat", FileMode.Open)
    let res = formatter.Deserialize(fsIn)
    let result: Map<string, string> = unbox res
    Map.toList result

let run contact = 
    printInfo
    let rec interactive contact = 
        let command = Console.ReadLine()
        match command with
        | "1" -> ignore
        | "2" -> 
            printf "Write name"
            let name = Console.ReadLine()
            printf "Wtite phone"
            let phone = Console.ReadLine()
            interactive (addContact name phone contact)
        | "3" -> 
            printf "Write name"
            let name = Console.ReadLine()
            let result = findPhone name contact
            printf "%A" result
            interactive contact
        | "4" ->
            printf "Write phone"
            let phone = Console.ReadLine()
            let result = findName phone contact
            printf "%A" result
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
            printf "Error command"
            printInfo
            interactive contact
    interactive contact






