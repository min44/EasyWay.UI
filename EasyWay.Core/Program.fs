module EasyWay.Core.Program
open EasyWay.Core
open Import
open System
open EasyWay.Core.Utils
open Elmish
open Elmish.WPF

type Msg =
    | SetCount
    | SetMinus
    | SetState of string
    | SetList of string list
    | AddThing
    | DelThing


type Model =
    { Count: int
      State: string
      ToDoList: string list }


let init () =
    { Count = 0
      State = List.head namesExported
      ToDoList = [] }, []


let SetAsyncCmd some =
    fun dispatch ->
        async {
            let start = DateTime.Now
            Logger.Info($"SetAsyncCmd Start: {start}")
            do! Async.Sleep(3000)
            dispatch DelThing
            do! Async.Sleep(2000)
            dispatch <| SetList some
            let final = DateTime.Now
            Logger.Info($"SetAsyncCmd Stop: {start}")
            Logger.Info($"Total time: {final - start}")
        } |> Async.StartImmediate
       

let update msg m =
    match msg with
    | SetCount   -> { m with Count = m.Count + 1 }, []
    | SetMinus   -> { m with Count = m.Count - 1 }, []
    | SetState v -> { m with State = List.head namesExported }, []
    | SetList v ->  { m with ToDoList = v }, []
    | AddThing   -> { m with ToDoList = m.State :: m.ToDoList }, [ SetAsyncCmd m.ToDoList ]
    | DelThing   -> { m with ToDoList = List.Empty }, []


let bindings () =
    [ "Count"     |> Binding.oneWay (fun m -> m.Count)
      "SetCount"  |> Binding.cmd SetCount
      "SetMinus"  |> Binding.cmd SetMinus
      "State"     |> Binding.twoWay ((fun m -> m.State.ToString()), SetState)
      "ToDoList"  |> Binding.oneWay (fun m -> m.ToDoList)
      "AddThing"  |> Binding.cmd AddThing
      "DelThing"  |> Binding.cmd DelThing ]


let Run window =
    Program.mkProgramWpf
        init
        update
        bindings
    |> Program.startElmishLoop ElmConfig.Default window
