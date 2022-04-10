module EasyWay.Core.Program

open Elmish
open Elmish.WPF


type Msg =
    |SetCount
    |SetMinus
    |SetState of string
    |AddThing
    |DelThing
   
type Model = {Count: int; State:string; ToDoList: string list}

let init () = { Count = 0
                State = "10"
                ToDoList = []}, []
let update msg m =
    match msg with 
    |SetCount -> {m with Count = m.Count + 1}, []
    |SetMinus -> {m with Count = m.Count - 1}, []
    |SetState v -> {m with State = v}, []
    |AddThing -> {m with ToDoList = m.State :: m.ToDoList }, []
    |DelThing -> {m with ToDoList = List.Empty},[]
    
    
   
let bindings () = 
    ["Count" |> Binding.oneWay (fun m -> m.Count)
     "SetCount" |> Binding.cmd SetCount
     "SetMinus" |> Binding.cmd SetMinus
     "State" |> Binding.twoWay((fun m -> m.State.ToString()), SetState)
     "ToDoList" |> Binding.oneWay (fun m -> m.ToDoList)
     "AddThing" |> Binding.cmd AddThing
     "DelThing" |> Binding.cmd DelThing]
    

let Run window =
    Program.mkProgramWpf
        init
        update
        bindings
    |> Program.startElmishLoop ElmConfig.Default window

