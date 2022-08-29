module EasyWay.Core.Program

open System
open Elmish
open Elmish.WPF
open Types
open Import
open Export
open System.Net.Http




type Msg =
    | SetTextBox of string
    | Parse 
    | ClearTextBlock
    | SetContent of string

type Model = { TextBox: string; Content: string}

let init () = { TextBox = String.Empty; Content = String.Empty }, []

let parser m = 
    fun dispatch ->
        async {
           let path = m.TextBox
           let client = new HttpClient()
           let request = client.GetByteArrayAsync path
           let! result = request|>Async.AwaitTask
           let content = result|>Seq.map (fun x -> x.ToString())|>Seq.toList|>List.reduce (fun x y -> y+x)     
           dispatch (SetContent content)
        } |> Async.StartImmediate


let update msg m  =
    match msg with
    | SetTextBox v   -> { m with TextBox = v }, []
    | Parse          ->   m, [parser m]
    | ClearTextBlock -> { m with Content = String.Empty }, []
    | SetContent v   -> { m with Content = v }, []

let bindings () =
    [ "TextBox"         |> Binding.twoWay ((fun m -> m.TextBox), SetTextBox)
      "Parse"           |> Binding.cmd Parse
      "ClearTextBlock"  |> Binding.cmd ClearTextBlock
      "Content"         |> Binding.oneWay (fun m -> m.Content)
    ]

let Run window = Program.mkProgramWpf init update bindings |> Program.startElmishLoop ElmConfig.Default window
