namespace Gadbot

open System.Text.RegularExpressions

type GambotDisResponder() =
    interface MargieBot.Responders.IResponder with
        member this.CanRespond context =
            let m = Regex.Match(context.Message.Text.ToLower(), "gadbot dis gambot")
            m.Success

        member this.GetResponse context =
            MargieBot.Models.BotMessage(Text = "Hey, @gambot. your momma was a snowblower!")

