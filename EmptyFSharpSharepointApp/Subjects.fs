module Subjects

open System.Reactive.Subjects
 
let StreamLineReceivedSubject = new Subject<string>()
let StartAgentsSubject = new Subject<bool>()

let MessageReceivedSubject = new Subject<Library.Message>()
