var ScriptModule__jq$, ScriptModule__Main$, ScriptModule__DrawTweet$; ScriptModule__DrawTweet$ = (function (input) { var tweet = input; var out = ((((((((((((("\u003cdiv class=\u0027list-group-item\u0027\u003e" + "\u003ch4 class=\u0027list-group-item-heading\u0027\u003e") + "\u003cimg src=\u0027") + tweet.Creator.ProfileImageUrl) + "\u0027/\u003e") + tweet.Creator.Name) + " (") + tweet.Creator.ScreenName) + ")") + "\u003c/h4\u003e") + "\u003cp class=\u0027list-group-item-text\u0027\u003e") + tweet.Text) + "\u003c/p\u003e") + "\u003c/div\u003e"); return (ScriptModule__jq$("#TweetsBox").prepend(out)); }); ScriptModule__Main$ = (function (unitVar0) { var hub = ((((window.$).connection).hub).createHubProxy("tweetHub")); (hub.on("tweetReceived", (function (x) { return ScriptModule__DrawTweet$(x); }))); (((((window.$).connection).hub).start()).done((function (_arg1) { return hub.server.startAgents(); }))); return ((window.window).alert("Hello world!")); }); ScriptModule__jq$ = (function (selector) { return ((window.$)(selector)); }); ScriptModule__Main$();