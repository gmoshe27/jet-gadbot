open MargieBot
open Gadbot

[<EntryPoint>]
let main argv = 
    try
        let bot = new Bot()
        let standupResponder = new StandupResponder()
        let gambotDisResponder = new GambotDisResponder()
        let jiraResponder = new JiraResponder()

        bot.Responders.Add standupResponder
        bot.Responders.Add gambotDisResponder
        bot.Responders.Add jiraResponder
        bot.Connect Settings.SLACK_TOKEN |> ignore

        let output = System.Console.ReadLine()
        0
    with
        | ex -> printfn "there was an exception %s" ex.InnerException.Message
                let output = System.Console.ReadLine()
                0