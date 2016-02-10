namespace Gambot.Jira

module JiraIssue =
    open System.Text.RegularExpressions
    open Gambot.Jira.JiraCommon

    let projectIds = ["MIR-"; "GAM-"]
    let projectIdIsInMessage message projectId =
        let m = Regex.Match(message, "(?i)" + projectId)
        m.Success
    let listedIds (message: string) = projectIds |> List.filter (fun projectId -> projectIdIsInMessage message projectId)

    // creates the pattern "MIR-\d+|GAM-\d+|..."
    let pattern = projectIds |> List.map (fun projectId -> sprintf "(?i)%s\d+" projectId) |> String.concat "|"

    let canRespond message =
        let m = Regex.Matches(message, pattern)
        m.Count > 0

    let respond credentials (message:string) = async {
        let getTicketsNonAsync issue = getTicketDetails credentials issue |> Async.RunSynchronously
        let tickets = Regex.Matches(message, pattern)
        let attachments = 
            tickets 
            |> Seq.cast 
            |> Seq.map (fun (m : Match) -> getTicketsNonAsync m.Value)
            |> Seq.choose id
            |> Seq.map (fun detail -> createAttachment detail)
            |> Array.ofSeq
        
        return attachments
    }