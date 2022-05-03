module EasyWay.Core.Program

open System
open Elmish
open Elmish.WPF


type Gender = 
    |Male
    |Female

type Msg =
    | SetTextBox of string
    | SetGender of Gender
    | SetSome
    

type Model = 
    { TextBox: string; 
      Gender: Gender; 
      Itog: string }

let init () = 
    { TextBox = "Hello"; 
      Gender = Male; 
      Itog = "Init" }, []

let itog m = $"My name is {m.TextBox}, and i am {m.Gender} person"

let some =
    fun dispatch -> 
        async { 
            do! Async.Sleep 500
            dispatch SetSome 
        } |> Async.StartImmediate 

let update msg model  =
    match msg with
    | SetTextBox v -> { model with TextBox = v }, []
    | SetGender v  -> { model with Gender = v},  [ some ]
    | SetSome ->  { model with Itog = itog model }, []
   

let bindings () =
    [ "TextBox"         |> Binding.twoWay ((fun m -> m.TextBox), SetTextBox)
      "IsMale"          |> Binding.twoWay ((fun m -> m.Gender = Male), (fun x -> SetGender Male))
      "IsFemale"        |> Binding.twoWay ((fun m -> m.Gender = Female), (fun x -> SetGender Female))
      "Itog"            |> Binding.oneWay (fun m -> m.Itog) ]
      
let Run window = Program.mkProgramWpf init update bindings |> Program.startElmishLoop ElmConfig.Default window
