module EasyWay.Core.Program

open Elmish
open Elmish.WPF


type Msg =
    |SetCount
    |SetMinus
    |SetState of string
type Count = {Count: int; State:int}

let init () = { Count = 0
                State = 10 }, []
let update msg m =
    match msg with
    |SetCount -> {m with Count = m.Count + m.State }, []
    |SetMinus -> {m with Count = m.Count - m.State}, []
    |SetState v -> {m with State = (int v) + m.Count }, []
    
   
let bindings (): Binding<Count, Msg> list =
    ["Count" |> Binding.oneWay (fun m -> m.Count)
     "SetCount" |> Binding.cmd SetCount
     "SetMinus" |> Binding.cmd SetMinus
     "State" |> Binding.twoWay((fun m -> m.State.ToString()), SetState)] 

let Run window =
    Program.mkProgramWpf
        init
        update
        bindings
    |> Program.startElmishLoop ElmConfig.Default window

