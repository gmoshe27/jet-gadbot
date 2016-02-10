// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

//https://github.com/jammerware/margiebot/wiki

open MargieBot
open Gadbot.Responders
open Gadbot.Settings

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
        bot.Connect SLACK_TOKEN |> ignore

        let output = System.Console.ReadLine()
        0 // return an integer exit code
    with
        | ex -> printfn "there was an exception %s" ex.InnerException.Message
                let output = System.Console.ReadLine()
                0