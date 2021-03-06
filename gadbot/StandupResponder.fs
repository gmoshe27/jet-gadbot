﻿namespace Gadbot

open Gambot.Jira
open Gambot.Jira.SlackHelper

type StandupResponder() =
    interface MargieBot.Responders.IResponder with
        member this.CanRespond context =
            Standup.canRespond context.Message.Text

        member this.GetResponse context =
            let creds = { SlackToken = Settings.SLACK_TOKEN; JiraUserName = Settings.JIRA_USERNAME; JiraPassword = Settings.JIRA_PASSWORD; JiraUrl = Settings.JIRA_URL }
            let slackUser = { 
                UserId = context.Message.User.ID
                FormattedUserId = context.Message.User.FormattedUserID
                FullName = context.UserNameCache.Item(context.Message.User.ID)
            }
            let message, attachments  = Standup.respond creds slackUser |> Async.RunSynchronously
            let botMessage = MargieBot.Models.BotMessage(Text = message)
            match attachments with
            | Some slackAttachments -> 
                let botAttachments = 
                    slackAttachments 
                    |> List.map (fun attachment ->
                        let sa = new MargieBot.Models.SlackAttachment()
                        sa.Title <- attachment.Title
                        sa.TitleLink <- attachment.TitleLink
                        sa.Text <- attachment.Text
                        sa.ColorHex <- attachment.ColorHex
                        sa)
                    |> Array.ofList
                botMessage.Attachments <- botAttachments
                botMessage
            | _ -> botMessage
