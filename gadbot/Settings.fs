module Gadbot.Settings
    open System

    let getSystemCredential environmentVariable defaultValue = 
        let credential = Environment.GetEnvironmentVariable environmentVariable
        match credential with
        | null -> defaultValue
        | _ -> credential

    let Token = getSystemCredential "GADBOT_JIRA_TOKEN" "YOUR-JIRA-TOKEN"
    let JIRA_USERNAME = getSystemCredential "GADBOT_JIRA_USERNAME" "YOUR-JIRA-USERNAME"
    let JIRA_PASSWORD = getSystemCredential "GADBOT_JIRA_PASSWORD" "YOUR-JIRA-PASSWORD"
