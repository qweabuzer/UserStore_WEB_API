using Microsoft.AspNetCore.Mvc;

namespace UsersStore.Api.Helpers
{
    public class ForbiddenResult : ObjectResult
    {
        public ForbiddenResult(string message) : base(new {Message = message})
        {
            StatusCode = StatusCodes.Status403Forbidden;
        }
    }
}
