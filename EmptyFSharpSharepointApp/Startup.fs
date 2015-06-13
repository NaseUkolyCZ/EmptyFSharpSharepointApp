namespace Server

open Owin
open Microsoft.Owin
 
type Startup() =
    member this.Configuration(app: Owin.IAppBuilder) =
        app.MapSignalR() |> ignore
        ()

[<assembly: OwinStartup(typeof<Startup>)>]
do ()
