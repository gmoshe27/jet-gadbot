namespace Gambot.Jira

module SlackApiResponseCodes =

    type ProfileDetails = {
        first_name: string
        last_name: string
        real_name: string
        email: string
        skype: string
        phone: string
        image_24: string
        image_32: string
        image_48: string
        image_72: string
        image_192: string
    }

    type UserDetails = {
        id: string
        name: string
        deleted: bool
        color: string
        profile: ProfileDetails
        is_admin: bool
        is_owner: bool
        is_primary_owner: bool
        is_restricted: bool
        is_ultra_restricted: bool
        has_2fa: bool
        two_factor_type: string
        has_files: bool
    }

    type UserInfo = {
        ok: bool
        user: UserDetails
    }