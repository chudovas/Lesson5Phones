open System

open NUnit.Framework
open FsUnit
open Phones
open Phones
open Phones

[<Test>]
let ``test of find phone by name``() =
    (PhoneBook.modelFindName [("a", 1); ("b", 2)] 2) |> should equal [("b", 2)]

[<Test>]
let ``test of find name by phone``() =
    (PhoneBook.modelFindTelephone [("a", 1); ("b", 2)] "b") |> should equal [("b", 2)]

[<Test>]
let ``test of add in files and read out files``() =
    let collection = [("a", 1); ("b", 2)]
    PhoneBook.saveInFile collection
    PhoneBook.loadOutFile() |> should equal collection

[<EntryPoint>]
let main argv =
    ``test of find phone by name``()
    ``test of find name by phone``()
    ``test of add in files and read out files``()
    0 
