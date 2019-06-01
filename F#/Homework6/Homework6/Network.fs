/// Модуль сети с компьютерами
module Network

open System

/// Типы операционных систем
type OS = Mac | Windows | Linux

/// Тип компьютер, typeOs --- операционная система комп., isInfected --- заражен ли ?
type Computer(typeOs : OS, isInfected) = 
    let mutable isInfected = isInfected
    member this.IsInfected: bool = isInfected
    member this.ProbabilityOfInfection: double = 
        match typeOs with
        | Mac -> 0.7
        | Linux -> 0.8
        | Windows -> 0.5
    member this.SetInfected(): unit = 
        isInfected <- true
    member this.TypeOfOS: OS = typeOs

/// Тип сети, computers --- компьютеры в сети, connectionMatrix --- матрица связей между копм., rndGen --- генератор рандомных чисел
type Network(computers: Computer list, connectionMatrix: (bool list) list, rndGen : Random) = 
    let mutable comps = computers
    let matrix = connectionMatrix
    let getInfectedComps (comps : Computer list ) = List.map (fun (x : Computer) -> x.IsInfected) comps
    let infection (n : int) = 
        let listToInfect = matrix |> List.item n |> List.zip ([0.. comps.Length - 1]) |> List.filter (fun x -> snd x) |> List.unzip |> fst
        let random = rndGen.NextDouble()
        printfn "%f" random
        let rec infect listToInfect = 
             match listToInfect with 
             | h::tail -> 
                let comp = (List.item h comps) : Computer
                if (comp.ProbabilityOfInfection <= random) then
                    let mutable arrayComp = List.toArray comps
                    arrayComp.[h].SetInfected()
                    comps <- Array.toList arrayComp
                infect tail
             | [] -> ()
        infect listToInfect
    let rec infectionStep infectionComps = 
        match infectionComps with 
        | h :: tail -> 
            infection h
            infectionStep tail
        | _ -> ()           
    member this.CheckInfectedComputers = getInfectedComps comps
    member this.NextStep() = 
        let numberList = [0.. comps.Length - 1]
        let infectionComps = List.zip (getInfectedComps comps) numberList |> List.filter (fun x -> fst x) |> List.unzip |> snd
        infectionStep infectionComps
        
        