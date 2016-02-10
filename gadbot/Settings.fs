namespace Gadbot

module Settings =
    open System

    let getSystemCredential environmentVariable defaultValue = 
        let credential = Environment.GetEnvironmentVariable environmentVariable
        match credential with
        | null -> defaultValue
        | _ -> credential

    let SLACK_TOKEN = getSystemCredential "GADBOT_SLACK_TOKEN" "YOUR-SLACK-TOKEN"
    let JIRA_USERNAME = getSystemCredential "GADBOT_JIRA_USERNAME" "YOUR-JIRA-USERNAME"
    let JIRA_PASSWORD = getSystemCredential "GADBOT_JIRA_PASSWORD" "YOUR-JIRA-PASSWORD"
