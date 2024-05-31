namespace UsersStore.Api.Contracts
{
    public record UserGetResponse
        (
            string name,
            int gender,
            DateTime? birthday,
            DateTime RevokedOn,
            string RevokedBy);
}
