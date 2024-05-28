using CSharpFunctionalExtensions;
using UsersStore.Core.Models;

namespace UsersStore.Application.Services
{
    public interface IUsersService
    {
        Task<Guid> CreateUser(Users user);
        Task<Guid> DeleteUser(Guid id);
        Task<Guid> SoftDeleteUser(Guid id, string revokedBy);
        Task<List<Users>> GetAllUsers();
        Task<List<Users>> GetActiveUsers();
        Task<Result<Users>> GetUserByLogin(string login);
        Task<Result<Users>> GetUserProfile(string login, string password);
        Task<List<Users>> GetUsersWithAge(int age);
        Task<Guid> UpdateUserInfo(Guid id, string? name, int? gender, DateTime? birthday, string? login, string? password, string? modifiedBy);
        Task<Guid> RestoreUser(Guid id);
    }
}