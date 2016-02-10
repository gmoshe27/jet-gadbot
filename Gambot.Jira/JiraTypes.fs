namespace Gambot.Jira

module JiraTypes =

    type TicketDetails = {
        IssueName : string
        IssueLink : string
        Summary : string
        CreatedBy : string
        Type: string
        TypeIcon : string
        Priority : string
        PriorityIcon : string
        Status: string
        StatusIcon : string
    }

    type IssueType = {
        name : string
        iconUrl : string
    }

    type Creator = {
        name : string
        displayName : string
    }

    type Priority = {
        name : string
        iconUrl : string
    }

    type Status = {
        name : string
        iconUrl : string
    }

    type Fields = {
        summary : string
        priority : Priority
        status : Status
        creator : Creator
        issueType: IssueType
    }

    type Issue = {
        id : int
        self : string
        key : string
        fields: Fields
    }

    type StatusResponse = {
        total : int
        issues : Issue list
    }