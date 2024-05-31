using UsersStore.Core.Models;

namespace UsersStore.DataAccess.Repositories
{
    public interface IUsersRepository
    {
        Task<Guid> Create(Users user);
        Task<Guid> Delete(Guid id);
        Task<Guid> LightDelete(Guid id, string revokedBy);
        Task<List<Users>> GetActive();
        Task<Users> GetUser(string login);
        Task<Users> GetProfile(string login, string password);
        Task<List<Users>> GetAged(int age);
        Task<Guid> UpdateInfo(Guid id, string? name, int? gender, DateTime? birthday, string? login, string? password, string? modifiedBy);
        Task<Guid> Restore(Guid id);
        bool AdminExists();
    }
}