namespace Agents 
 
open NamelessInteractive.FSharp.OAuth
open System.IO
open Subjects
open Library
 
type Agent<'T> = MailboxProcessor<'T>
     
 
module ControllingAgent = 
    let BaseFilterStreamUrl = "https://stream.twitter.com/1.1/statuses/filter.json?language=en&track="
    let TrackVariables = "jsinsa"
 
    let BuildFilteredStreamUrl() = 
        BaseFilterStreamUrl + TrackVariables
    type System.Net.WebRequest with
        member x.GetResponseAsyncWorkflow() =
            Async.FromBeginEnd(x.BeginGetResponse, x.EndGetResponse)
 
    let MyTwitterCredentials = 
        {
            OAuthCredentials.AccessToken = "Your Twitter Access Token"
            OAuthCredentials.AccessTokenSecret = "Your Twitter Access Token Secret"
            OAuthCredentials.ConsumerKey = "Your Twitter Consumer Key"
            OAuthCredentials.ConsumerSecret = "Your Twitter Consumer Secret"
        }
 
    let Log message = 
        (System.Diagnostics.Debug.WriteLine(message))
 
    let StreamTweets() =
        async 
            {
                try
                    let url = BuildFilteredStreamUrl()
                    let request = GenerateOAuthWebRequest url Get MyTwitterCredentials
                    use! response = request.GetResponseAsyncWorkflow()
                    use stream = response.GetResponseStream()
                    use reader = new StreamReader(stream)
                    while not reader.EndOfStream do
                        let line = reader.ReadLine() 
                        StreamLineReceivedSubject.OnNext line
                with
                | _ as e -> 
                    Log e.Message
                    Log e.StackTrace
            }
            |> Async.StartImmediate
 
 
    let ControlFunction (inbox: Agent<bool>) =  
        let isStarted = false
        let rec loop started =
            async 
                {
                    let! cmd = inbox.Receive()
                    match cmd with
                    | false -> return! loop started
                    | true ->
                        if started then
                            return! loop started
                        else
                            // Fire up the other agents
                            StreamTweets()
                            return! loop true
                }
        loop false

    let ControllingAgent = new Agent<bool>(ControlFunction)
    StartAgentsSubject.Subscribe ControllingAgent.Post |> ignore 


module MessagingAgent = 
    open Microsoft.AspNet.SignalR
    open FSharp.Interop.Dynamic
    let ProcessMessage(inbox: Agent<Message>) =
        let hub = GlobalHost.ConnectionManager.GetHubContext<Hubs.TweetHub>()
        let rec loop() =
            async 
                {
                    let! cmd = inbox.Receive()
                    match cmd with
                    | TweetReceived tweet -> 
                        hub.Clients.All?tweetReceived(tweet)
                    | _ -> failwith "No processing specified yet"
                    return! loop()
                }
        loop()
    let MessagingAgent = new Agent<Message>(ProcessMessage)
    MessageReceivedSubject.Subscribe MessagingAgent.Post |> ignore
