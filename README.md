# jet-gadbot
Gadbot is a simple bot for slack that outputs your jira ticket statuses when they are mentioned in a channel.

Gadbot extends [margiebot](https://github.com/jammerware/margiebot) to connect to slack and respond with the status of Jira tickets
when they are mentioned. There are two responders in Gadbot.

## Jira Responder
This responder is always listening to the channel for the mention of a jira ticket. It is currently hardcoded to MIR-# and GAM-#, but it
should be possible to look for items within a list, or just with a specific regex query, like "(?i)[a-zA-Z]+-\d+". (future thought)

## Standup Responder
This responder specifically looks for any message that starts with "standup:". Gadbot then proceeds to query Jira for all open tickets.
Currently it is hardcoded to search for jira tickets that are labeled as "Dev in Progress". It would be better to have the url and
status defined at the program level.

## Setup

You will need to either setup environment variables or add your credentials to the Gadbot.Settings module. Jira's api uses basic 
authentication over https for each query to the api. Slack requires a token that you must generate from the slack website to be able
to authenticate your app. 

Set these environment variables
```
GADBOT_SLACK_TOKEN
GADBOT_JIRA_USERNAME
GADBOT_JIRA_PASSWORD
```
If you don't feel like adding the credentials as an environment variable, you can add them to the Gadbot.Settings module
```fsharp
let SLACK_TOKEN = getSystemCredential "GADBOT_SLACK_TOKEN" "YOUR-SLACK-TOKEN"
let JIRA_USERNAME = getSystemCredential "GADBOT_JIRA_USERNAME" "YOUR-JIRA-USERNAME"
let JIRA_PASSWORD = getSystemCredential "GADBOT_JIRA_PASSWORD" "YOUR-JIRA-PASSWORD"
```
  
