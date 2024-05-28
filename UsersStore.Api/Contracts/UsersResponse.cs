namespace UsersStore.Api.Contracts
{
    public record UsersResponse(
        Guid id,
        string login,
        string password,
        string name,
        int gender,
        DateTime? birthday,
        bool admin,
        DateTime createdOn,
        string createdBy,
        DateTime modifiedOn,
        string modifiedBy,
        DateTime revokedOn,
        string revokedBy);
}

