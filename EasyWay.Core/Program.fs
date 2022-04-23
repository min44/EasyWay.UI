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
    | Exit of obj
    | SetTodos of Todo list
    | GetTodos
    | SetToDoText of string * int

type Model = { TextBox: string; ToDos: Todo list }


let init () = { TextBox = String.Empty; ToDos = [] }, []

let AddThings m =
    { m with
        ToDos = { Id = Random().Next(0, 999); Value = m.TextBox; ToDoText = String.Empty } :: m.ToDos
        TextBox = String.Empty }

let RemoveToDo m (b: obj) =
    { m with ToDos = m.ToDos |> List.filter (fun z -> z.Id <> (b :?> int)) }

let SaveCmd todos =
    fun dispatch ->
        async {
            Export todos
            ()
        } |> Async.StartImmediate


let GetTodosCmd =
    fun dispatch ->
        async {
            Import.GetTodos() |> SetTodos |> dispatch  
        } |> Async.StartImmediate


let SetToDoText (m: Model) value id = 
    let todos = m.ToDos
    let maper t = if t.Id = id then { t with ToDoText = value } else t
    let todos = List.map maper todos
    { m with ToDos = todos }


let update msg m  =
    match msg with
    | SetTextBox v  -> { m with TextBox = v }, []
    | AddTodo       -> AddThings m, []
    | RemoveTodo b  -> RemoveToDo m b, []
    | Save          -> m, [ SaveCmd m.ToDos ]
    | Exit s        -> m, []
    | SetTodos t    -> { m with ToDos = t }, []
    | GetTodos      -> m, [ GetTodosCmd ]
    | SetToDoText (v, id) -> SetToDoText m v id, []

let TodoBinding() =
    [ "RemoveTodo"  |> Binding.cmdParam RemoveTodo
      "Id"          |> Binding.oneWay (fun (_, s) -> s.Id)
      "Value"       |> Binding.oneWay (fun (_, s) -> s.Value)
      "ToDoText"    |> Binding.twoWay ((fun (_, s) -> s.ToDoText), (fun a (m, s) -> Msg.SetToDoText (a, s.Id))) ]

let bindings () =
    [ "TextBox"         |> Binding.twoWay ((fun m -> m.TextBox), SetTextBox)
      "ToDos"           |> Binding.subModelSeq ((fun m -> m.ToDos), (fun y -> y.Id), TodoBinding)
      "AddTodo"         |> Binding.cmdIf (AddTodo, (fun m -> not <| String.IsNullOrEmpty(m.TextBox)))
      "Save"            |> Binding.cmd Save
      "Rendered"        |> Binding.cmd GetTodos ]

let Run window = Program.mkProgramWpf init update bindings |> Program.startElmishLoop ElmConfig.Default window
