module EasyWay.Core.Program

open System
open Elmish
open Elmish.WPF
open Types
open Import
open Export

type Msg =
    | SetTextBox of string
    | AddTodo
    | RemoveTodo of obj
    | Save

type Model = { TextBox: string; ToDos: Todo list }

let init () = { TextBox = String.Empty; ToDos = GetTodos() }, []

let AddThings m =
    { m with
        ToDos = { Id = Random().Next(0, 999); Value = m.TextBox } :: m.ToDos
        TextBox = String.Empty }

let SaveCmd todos =
    fun dispatch ->
        async {
            Export todos
        } |> Async.StartImmediate

let update msg m =
    match msg with
    | SetTextBox v  -> { m with TextBox = v }, []
    | AddTodo       -> AddThings m, []
    | RemoveTodo b  -> { m with ToDos = m.ToDos |> List.filter (fun z -> z.Id <> (b :?> int)) }, []
    | Save          -> m, [ SaveCmd m.ToDos ]


let bindings () =
    [ "TextBox"         |> Binding.twoWay ((fun m -> m.TextBox), SetTextBox)
      "ToDos"           |> Binding.subModelSeq ((fun m -> m.ToDos), (fun y -> y.Id), (fun () ->
        [ "RemoveTodo"  |> Binding.cmdParam RemoveTodo
          "Id"          |> Binding.oneWay (fun (_, s) -> s.Id)
          "Value"       |> Binding.oneWay (fun (_, s) -> s.Value) ]))
      "AddTodo"         |> Binding.cmdIf (AddTodo, (fun m -> not <| String.IsNullOrEmpty(m.TextBox)))
      "Save"            |> Binding.cmd Save
      ]

let Run window = Program.mkProgramWpf init update bindings |> Program.startElmishLoop ElmConfig.Default window
