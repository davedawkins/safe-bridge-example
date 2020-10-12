module Shared

open System

// Ensure that this is matched with CONFIG.devServerProxy in webpack.config.js
let endpoint = "/socket"

type Counter = int

// Messages sent to server from client
type ServerMsg =
    | Increment of Counter // responds with SetCounter back to client
    | Decrement of Counter // responds with SetCounter back to client

// Messages sent to client from server
type ClientMsg =
    | SetCounter of Counter
