# Pinewood Labs
A full-stack tech demo for testing Pinewood Derby racer designs!
# Tech Info
This application is composed of Unity and React front-ends that communicate via a Socket.IO server.

1. The racecar editor draws to a canvas using Konva, and all changes are emitted via socket. BestHTTP was used as the C# API to Socket.IO.
2. Unity listens for updated shape data from the server, and creates procedural geometry for the car body. I wrote the custom triangle-strip winding code, and the triangulation algorithm is from the Unity wiki. 
3. A car race (in Unity) is kicked off from React. The Unity front-end has no direct user-interactivity. It is entirely controlled by server events that drive an ultra-lightweight state machine.
4. The car's speed during the race is continually sent from Unity to React and graphed live using Highcharts.

# Stack
- React
  - Highcharts
  - Konva (Canvas)
- Unity
  - BestHTTP
  - procedural geometry
  - SimpleState (State Machine)
- Socket.IO


# Considerations
Being a weekend tech demo there are some allowed impractical aspects to this I'd like to aknowledge. Unity and React can communicate directly via client-side Javascript, so Socket.IO isn't really needed for the current feature set. I had intended to make it more multi-user, but could only fit so much in a weekend. Currently it's a little weird if two people are using it at once.
The Socket.IO server is embeded in the React project. In production software you'd want it separate; probably running in Express.
My attempt at calculating aerodynamic for the player's car currently end up giving it advantage in every race. 
I moddled and textured everything except for the locker and bench. I configured the Unity renderer to push WebGL for visual fidelity, but it could use some performance tuning.
Not enough time yet put in to environment config, tests, or documentation.
The API payloads are not optimized. In production the geometry and graph data should be sent as updates and diffs.
UV calculations on the procedural mesh are also still a bit off.
Can't fault me for honesty.
# Thanks
