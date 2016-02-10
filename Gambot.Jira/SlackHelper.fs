namespace Gambot.Jira

module SlackHelper =
    open Newtonsoft.Json
    open System.Net.Http
    open System.Net.Http.Headers
    open System.Web
    open Gambot.Jira.JiraTypes
    open Gambot.Jira.SlackApiResponseCodes

    type SlackUser = {
        UserId : string
        FormattedUserId : string
        FullName : string
    }

    type SlackkAttachment = {
        Title : string
        TitleLink : string
        Text : string
        ColorHex : string
    }

    let userInfoUrl = "https://slack.com/api/users.info"
    let userParams token id = sprintf "?token=%s&user=%s" token id

    let getSlackUsername credentials slackUserId = async {
            let client = new HttpClient()
            let acceptJson = new MediaTypeWithQualityHeaderValue("application/json")
            client.DefaultRequestHeaders.Accept.Add acceptJson
            let url = userInfoUrl + (userParams credentials.SlackToken slackUserId)
            let! response = client.GetAsync(url) |> Async.AwaitTask
            if response.IsSuccessStatusCode then
                let! responseBody = response.Content.ReadAsStringAsync() |> Async.AwaitTask
                let userInfoResponse = JsonConvert.DeserializeObject<UserInfo>(responseBody)
                if userInfoResponse.ok = false then
                    return None
                else
                    return Some userInfoResponse.user.profile.email
            else
                return None
        }
