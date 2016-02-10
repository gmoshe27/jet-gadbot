namespace Gambot.Jira

module Standup =
    open System.Text
    open System.Text.RegularExpressions
    open Gambot.Jira.JiraCommon
    open Gambot.Jira.SlackHelper

    type SlackResponse =
        | Message
        | MessageWithAttachments

    let canRespond (message:string) = 
        let m = Regex.Match(message, "^(?i)standup:")
        m.Success

    let respond credentials slackUser = async {
        let! slackUsername = getSlackUsername credentials slackUser.UserId
        match slackUsername with
        | Some username ->
            let user = slackUser.FullName
            let openIssues = getOpenJiraIssues credentials username
            let issueList = 
                openIssues
                |> List.map (fun issue -> sprintf "*[%s]*" issue)
                |> String.concat " "
            let message = sprintf "Open issues for %s: %s" username issueList
            let! attachments = getIssueDetails credentials openIssues
            return message, Some attachments
        | None -> 
            let message = sprintf "Sorry, %s, I'm having trouble finding your profile information." slackUser.FormattedUserId
            return message, None
    }
