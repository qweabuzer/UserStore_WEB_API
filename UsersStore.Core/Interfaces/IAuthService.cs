using CSharpFunctionalExtensions;
using UsersStore.Core.Models;

namespace UsersStore.Core.Interfaces
{
    public interface IAuthService
    {
        Task<Result<Users>> Authenticate(string login, string password);
        bool isAdmin(Users user);
    }
}
