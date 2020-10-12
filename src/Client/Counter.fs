module Counter

open Elmish
open Fulma
open Fable.React
open Fable.React.Props
open Shared
open Elmish.Bridge

type Model = Shared.Counter // : int

type Message =
    | Increment         // from [+] button
    | Decrement         // from [-] button

let init n : Model * Cmd<Message> =
    n, Cmd.none

let update msg model =
    match msg with
    | Increment ->
        model, (ServerMsg.Increment model) |> Cmd.bridgeSend
    | Decrement ->
        model, (ServerMsg.Decrement model) |> Cmd.bridgeSend

let centeredColumn (props : Column.Option list) =
    let style = Column.Props [
            Style [
                Display DisplayOptions.Flex
                JustifyContent JustifySelfOptions.Center
                AlignItems AlignItemsOptions.Center
            ]
        ]
    Column.column (style :: props)

let view model dispatch =
    Columns.columns [Columns.Props [ Style [ MarginTop 24 ] ] ] [
        Column.column [ Column.Width (Screen.All, Column.Is1)] [  ]
        centeredColumn [ Column.Width (Screen.All, Column.Is1)] [ Heading.h5 [] [ str "Counter" ] ]
        Column.column [ Column.Width (Screen.All, Column.Is1)] [
            Columns.columns [ Columns.IsCentered ] [
                centeredColumn [] [
                    button [ ClassName "button"; OnClick (fun _ -> dispatch Decrement) ] [ str "-" ]
                ]
                centeredColumn [] [
                    str (sprintf "%d" model)
                ]
                centeredColumn [] [
                    button [ ClassName "button"; OnClick (fun _ -> dispatch Increment) ] [ str "+" ]
                ]
            ]
        ]
    ]
