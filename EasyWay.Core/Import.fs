module EasyWay.Core.Import
open FSharp.Interop.Excel.ExcelProvider
open FSharp.Interop.Excel

module private Data =   
    type Names      = ExcelFile<"Data\Data.xlsx", SheetName="Name", ForceString=true>
    let IsNotNull (row: Row) = row.GetValue 0 <> null
    let GetData data = Seq.filter IsNotNull data    
    let Names = new Names() 
    let names = GetData Names.Data

open Data
let namesExported = Seq.toList names |> List.map (fun m -> m.Name) 