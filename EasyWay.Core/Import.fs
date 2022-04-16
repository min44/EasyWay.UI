module EasyWay.Core.Import
//open FSharp.Interop.Excel.ExcelProvider
//open FSharp.Interop.Excel
//open EasyWay.Core
//open TypesConstructor

//module private Data =   
//    type Names = ExcelFile<"Data\Data.xlsx", SheetName="Sheet1", ForceString=true>
//    type Age   = ExcelFile<"Data\Data.xlsx", SheetName="Sheet2", ForceString=true>
//    let IsNotNull (row: Row) = row.GetValue 0 <> null
//    let GetData data = Seq.filter IsNotNull data    
//    let Names = new Names() 
//    let Age   = new Age()
//    let names = GetData Names.Data
//    let age   = GetData Age.Data
//open Data
//let namesExported = Seq.toList names |> List.map string 

//let persons = 
//    names 
//    |> Seq.map(fun (x) -> Create (int x.Id) x.Name 0)