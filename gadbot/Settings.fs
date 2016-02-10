namespace Gadbot

module Settings =
    open System

    let getEnvironmentVariable environmentVariable defaultValue = 
        let credential = Environment.GetEnvironmentVariable environmentVariable
        match credential with
        | null -> defaultValue
        | _ -> credential

    let SLACK_TOKEN = getEnvironmentVariable "GADBOT_SLACK_TOKEN" "YOUR-SLACK-TOKEN"
    let JIRA_USERNAME = getEnvironmentVariable "GADBOT_JIRA_USERNAME" "YOUR-JIRA-USERNAME"
    let JIRA_PASSWORD = getEnvironmentVariable "GADBOT_JIRA_PASSWORD" "YOUR-JIRA-PASSWORD"
    let JIRA_URL = getEnvironmentVariable "GADBOT_JIRA_URL" "https://your-company.atlassian.net"