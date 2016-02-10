namespace Gambot.Jira

module JiraCommon =
    open System
    open System.Net.Http
    open System.Text
    open Gambot.Jira
    open Gambot.Jira.JiraTypes
    open Gambot.Jira.SlackHelper
    open Newtonsoft.Json

    let createAttachment detail =
        let title = sprintf "%s : %s" detail.IssueName detail.Summary
        let issueTextFormat = sprintf "Created By: %s\nType: *%s*  Priority: *%s*  Status: *%s*"
        let issueText = issueTextFormat detail.CreatedBy detail.Type detail.Priority detail.Status

        let attachment = {
            Title = title
            TitleLink = detail.IssueLink
            Text = issueText
            ColorHex = "#8200ff"
        }
        attachment

    let readResponse credentials jsonResponse =
        let resp = JsonConvert.DeserializeObject<Issue> jsonResponse
        let issue = {
            IssueName = resp.key
            IssueLink = sprintf "%s/browse/%s" credentials.JiraUrl resp.key
            Summary = resp.fields.summary
            CreatedBy = resp.fields.creator.displayName
            Type = resp.fields.issueType.name
            TypeIcon = resp.fields.issueType.iconUrl
            Priority = resp.fields.priority.name
            PriorityIcon = resp.fields.priority.iconUrl
            Status = resp.fields.status.name
            StatusIcon = resp.fields.status.iconUrl
        }
        
        issue

    let getTicketDetails creds issue = 
        async {
            let url = sprintf "%s/rest/api/2/issue/%s" creds.JiraUrl issue

            let base64Credentials = 
                sprintf "%s:%s" creds.JiraUserName creds.JiraPassword
                |> Encoding.ASCII.GetBytes 
                |> Convert.ToBase64String

            let client = new HttpClient()
            client.DefaultRequestHeaders.Authorization <- new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64Credentials)
            let! response = client.GetAsync(url) |> Async.AwaitTask
            if not response.IsSuccessStatusCode then
                return None
            else
                let! responseBody = response.Content.ReadAsStringAsync() |> Async.AwaitTask
                let issue = readResponse credentials responseBody
                return Some issue
        }

    let getOpenJiraIssues credentials username =
        let url = sprintf "%s/rest/api/2/search" credentials.JiraUrl
        let jql = sprintf "jql=assignee='%s' and status='Dev in Progress'" username |> Uri.EscapeUriString
        let openTicketsQuery = sprintf "%s?%s" url jql

        let queryJiraTickets (encodedUrl: string) = 
            async {
                let base64Credentials = 
                    sprintf "%s:%s" credentials.JiraUserName credentials.JiraPassword
                    |> Encoding.ASCII.GetBytes 
                    |> Convert.ToBase64String

                let client = new HttpClient()
                client.DefaultRequestHeaders.Authorization <- new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64Credentials)
                let! response = client.GetAsync(encodedUrl) |> Async.AwaitTask
                if not response.IsSuccessStatusCode then
                    return { total = 0; issues = [] }
                else
                    let! responseBody = response.Content.ReadAsStringAsync() |> Async.AwaitTask
                    let queryObj = JsonConvert.DeserializeObject<StatusResponse> responseBody
                    return queryObj
            }

        let ticketResults = queryJiraTickets openTicketsQuery |> Async.RunSynchronously
        let issues = 
            ticketResults.issues
            |> List.map (fun issue -> issue.key)
        issues

    let getIssueDetails credentials issues = async  {
        let getTicketsNonAsync issue = getTicketDetails credentials issue |> Async.RunSynchronously
        let attachments = 
            issues
            |> List.map (fun issue -> getTicketsNonAsync issue)
            |> List.choose id
            |> List.map (fun detail -> createAttachment detail)
        return attachments
    }
