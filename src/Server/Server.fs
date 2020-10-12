module Server

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn

open Shared
open Giraffe.Core


open Elmish
open Elmish.Bridge
open Shared

type ServerState = Nothing
type ServerMessage =
    | FromClient of ServerMsg // From client
    | Error of exn

let init clientDispatch () =
    clientDispatch (SetCounter 10) // Do this via Cmd.OfFunc.attempt to thread into message loop?
    Nothing, Cmd.none

let update clientDispatch msg model =
    match msg with
    | FromClient msg ->
        match msg with
        | Increment n ->
            model, Cmd.OfFunc.attempt clientDispatch (SetCounter <| n+1) Error
        | Decrement n ->
            model, Cmd.OfFunc.attempt clientDispatch (SetCounter <| n-1) Error
    | Error x ->
        System.Console.WriteLine("Error {0}", x)
        model, Cmd.none

let server =
  Bridge.mkServer "" init update
  |> Bridge.withConsoleTrace
  |> Bridge.register FromClient
  |> Bridge.run Giraffe.server

let webApp = router {
    forward "/socket" server
}

let app =
    application {
        url "http://0.0.0.0:8085"
        use_router webApp
        memory_cache
        use_static "public"
        use_gzip
        app_config Giraffe.useWebSockets
    }

run app
