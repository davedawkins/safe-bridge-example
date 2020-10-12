Stripped-down SAFE stack to contain only a Counter in order to explore how Elmish.Bridge works.

Main things I learned:
1. Client must wait for message from server. It will `init` before the connection is established. Use the server's init to let clients know they are connected
2. Make sure the the socket name you use (endpoint) is matched with CONFIG.devServerProxy in webpack.config.js

I'm still not sure on best practise for handling startup, where client is not yet connected (item 1 from above)
