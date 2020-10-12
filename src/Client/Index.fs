module Index

open Elmish
open Fable.React
open Shared
open Elmish.Bridge

type Model =
    | Loading  // Initial state, wait for server to send a counter to work on
    | Counter of Counter.Model // The current counter

type Message =
    | FromServer    of ClientMsg       // from server: eg , new/updated counter
    | FromCounterUI of Counter.Message // from UI, eg, [+], [-] button presses

// Don't attempt to send a bridge message here - we're not connected yet
// Use the Server's init to push initial state to us, in the meantime we're (passively) "loading"
let init() =
    Loading, Cmd.none

let update msg model =
    match msg, model with

    | FromServer (SetCounter c), _ -> Counter c,Cmd.none

    | FromCounterUI cmsg, Counter c ->
        let (newCounter,cmd) = Counter.update cmsg c
        Counter newCounter, Cmd.map FromCounterUI cmd

    // Kind of disappointed that this needs to be enumerated. My types aren't expressive enough?
    // Can it be eliminated with a different set of type declarations?
    // If I leave it out compile will complain (correctly)
    // If I wildcard as (_,_) then I am setting myself up for an unhandled message later
    //
    // Ideally I want illegal (and impossible) states to be unrepresentable.
    | FromCounterUI _, Loading -> failwith "Unexpected UI message when counter not yet loaded"

let view model dispatch =
    match model with
    | Loading -> str "Loading..."
    | Counter counter -> Counter.view counter (FromCounterUI >> dispatch)

