using Microsoft.AspNetCore.Mvc;

namespace UsersStore.Api.Helpers
{
    public class RevokedResult: ObjectResult
    {
        public RevokedResult(string message) : base(new { Message = message })
        {
            StatusCode = StatusCodes.Status403Forbidden;
        }
    }
}
