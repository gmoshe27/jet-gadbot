﻿namespace Gadbot

open System
open System.Collections.Generic
open Gambot.Jira

type JiraResponder() =
   
    interface MargieBot.Responders.IResponder with
        member this.CanRespond context =
            JiraIssue.canRespond context.Message.Text

        member this.GetResponse context = 
            let creds = { SlackToken = Settings.SLACK_TOKEN; JiraUserName = Settings.JIRA_USERNAME; JiraPassword = Settings.JIRA_PASSWORD; JiraUrl = Settings.JIRA_URL }
            let attachments = JiraIssue.respond creds context.Message.Text |> Async.RunSynchronously
            let slackAttachments =
                attachments 
                |> Array.map (fun attachment ->
                    let sa = new MargieBot.Models.SlackAttachment()
                    sa.Title <- attachment.Title
                    sa.TitleLink <- attachment.TitleLink
                    sa.Text <- attachment.Text
                    sa.ColorHex <- attachment.ColorHex
                    sa)
            MargieBot.Models.BotMessage(Attachments = slackAttachments)
