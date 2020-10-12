Stripped-down SAFE stack to contain only a Counter in order to explore how Elmish.Bridge works.

Main things I learned:
1. Client must wait for message from server. It will `init` before the connection is established. Use the server's init to let clients know they are connected
2. Make sure the the socket name you use (endpoint) is matched with CONFIG.devServerProxy in webpack.config.js

I'm still not sure on best practise for handling startup, where client is not yet connected (item 1 from above)

*Domain modelling issue*

See comments in this code from `Client/Index.fs`:
```
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
```
