open System
open System.IO
open System.Runtime.Serialization.Formatters.Binary 

module PhoneBook =
    let modelFindTelephone listRecords name =
        List.filter (fun (a, _) -> a = name) listRecords

    let modelFindName listRecords telephone =
        List.filter (fun (_, a) -> a = telephone) listRecords

    let printCap() =
        printfn "************ TELEFONE NUMBERS ************"
        printfn "* Input \"exit\" to exit"
        printfn "* Input \"add\" to add new name + telephone"
        printfn "* Input \"find telephone\" to find telephone by name"
        printfn "* Input \"find name\" to find name by telephone"
        printfn "* Input \"show\" to show the database contents"
        printfn "* Input \"save\" to save the database in file"
        printfn "* Input \"load\" to load the database out file"
        printfn "******************************************"

    let addRecord () =
        printfn "Add name:"
        let n = Console.ReadLine()
        printfn "Add telephone:"
        let t = Console.ReadLine()
        (n, t)

    let findTelephone(listRecords) =
        printfn "Input name:"
        modelFindTelephone listRecords (Console.ReadLine())

    let findName(listRecords) =
        printfn "Input telephone:"
        modelFindName listRecords (Console.ReadLine())

    let printCollection(listRec) =
        let rec printCollectionRec listRecord =
            match listRecord with
            | [] -> None
            | (a, b)::_ -> 
                printfn "Name: %s Telephone: %s" a b
                printCollectionRec (List.tail listRecord)
        match listRec with
        | [] -> printfn "Empty collection!"; None
        | _ -> printCollectionRec listRec 

    let saveInFile(listRecord) =
        let formatter = new BinaryFormatter() 
        use stream = new FileStream("Data.dat", FileMode.Create)
        formatter.Serialize(stream, box listRecord)
        stream.Close()

    let loadOutFile() =
        let formatter = new BinaryFormatter() 
        use stream = new FileStream("Data.dat", FileMode.OpenOrCreate)
        let res = unbox (formatter.Deserialize(stream)) 
        stream.Close()
        res

    let rec handler (listRecords : list<string * string>)  (str : string) =
        match str with 
        | "exit" -> exit 0
        | "add" ->
            let res = addRecord()::listRecords
            printfn "Ok!"
            handler res (Console.ReadLine())
        | "find telephone" -> 
            listRecords |> findTelephone |> printCollection |> ignore
            handler listRecords (Console.ReadLine())
        | "show" -> 
            listRecords |> printCollection |> ignore
            handler listRecords (Console.ReadLine())
        | "find name" ->
            listRecords |> findName |> printCollection |> ignore
            handler listRecords (Console.ReadLine())
        | "save" ->
             try
                listRecords |> saveInFile |> ignore
                printfn "Ok!" |> ignore
                handler listRecords (Console.ReadLine())
             with
                | _ ->
                    printfn "Unknown error. Sorry..." |> ignore
                    handler listRecords (Console.ReadLine())
        | "load" ->
            try 
                let coll = loadOutFile()
                printfn "Ok!" |> ignore
                handler (coll) (Console.ReadLine())
            with
                | :? System.Runtime.Serialization.SerializationException -> 
                    printfn "Error! There is nothing to download..." |> ignore
                    handler listRecords (Console.ReadLine())
                | _ ->
                    printfn "Unknown error. Sorry..." |> ignore
                    handler listRecords (Console.ReadLine())
        | _ -> handler listRecords (Console.ReadLine())
    
    let go() =
        printCap()
        handler [] (Console.ReadLine())

[<EntryPoint>]
let main argv =
    0
