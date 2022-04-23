module EasyWay.Core.Import

open FSharp.Interop.Excel.ExcelProvider
open FSharp.Interop.Excel
open Types

module private Data =
    type Todos = ExcelFile<"Data\Data.xlsx", SheetName="Data", ForceString=true>
    let IsNotNull (row: Row) = row.GetValue 0 <> null
    let GetData data = Seq.filter IsNotNull data
    let Todos = new Todos()
    let todos = GetData Todos.Data

open Data
open System
let GetTodos () =
    todos
    |> Seq.map (fun x -> { Id = int x.Id; Value = x.Value; ToDoText = String.Empty })
    |> Seq.toList
