module Hubs

open Subjects

type TweetHub() =
    inherit Microsoft.AspNet.SignalR.Hub()
    member this.Start() = 
        StartAgentsSubject.OnNext true
