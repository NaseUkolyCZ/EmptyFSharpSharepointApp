module Hubs

open Subjects

type TweetHub() =
    inherit Microsoft.AspNet.SignalR.Hub()
    member this.StartAgents() = 
        StartAgentsSubject.OnNext true
