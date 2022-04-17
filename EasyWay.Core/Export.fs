module EasyWay.Core.Export

open SwiftExcel
open Types

let Export (items: Todo list) =
    let dstPath = "C:\Users\MSbook\RiderProjects\EasyWay.UI\EasyWay.Core\Data\Data.xlsx"
    let sheet = Sheet()
    sheet.Name <- "Data"
    use ev = new ExcelWriter(dstPath, sheet)
    ev.Write("Id", 1, 1)
    ev.Write("Value", 2, 1)
    items |> Seq.iteri(fun rn item ->
        ev.Write($"{item.Id}", 1, rn + 2)
        ev.Write($"{item.Value}", 2, rn + 2))