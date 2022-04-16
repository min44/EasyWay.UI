module EasyWay.Core.Program
open EasyWay.Core
//open Import
open System
open EasyWay.Core.Utils
open Elmish
open Elmish.WPF

type Item = { Id: int; Value: string}

type Msg =  
    | SetTextBox of string
    | SetList of Item list
    | AddThing
    | DelThing
    | RemoveItem of obj

type Model =
    { 
      TextBox: string
      ToDoList: Item list }


let init () =
    { 
      TextBox = String.Empty
      ToDoList = [] }, []
 
 
let update msg m =
    match msg with
  
    | SetTextBox v -> { m with TextBox = v }, []
    | SetList v    -> { m with ToDoList = v }, []
    | AddThing     -> { m with ToDoList = {Id = Random().Next(0,999); Value = m.TextBox} :: m.ToDoList }, []
    | DelThing     -> { m with ToDoList = [] }, []
    | RemoveItem b  -> {m with ToDoList = m.ToDoList |> List.filter (fun z -> z.Id <> (b :?> int)) }, []


let bindings () =
    [ "TextBox"     |> Binding.twoWay ((fun m -> m.TextBox), SetTextBox)
      "ToDoList"    |> Binding.subModelSeq ((fun m -> m.ToDoList),(fun y -> y.Id), (fun () -> 
        [ "RemoveItem" |> Binding.cmdParam (fun id -> RemoveItem id ) 
          "Id"         |> Binding.oneWay (fun (m, s) -> s.Id) 
          "Value"      |> Binding.oneWay (fun (m, s) -> s.Value) 
        
        ]))
      "AddThing"    |> Binding.cmdIf (AddThing, (fun m -> not <| String.IsNullOrEmpty(m.TextBox)))
      "DelThing"    |> Binding.cmd DelThing 
    ]
    

let Run window =
    Program.mkProgramWpf
        init
        update
        bindings
    |> Program.startElmishLoop ElmConfig.Default window
