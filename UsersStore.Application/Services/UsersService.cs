using UsersStore.Core.Models;
using UsersStore.DataAccess.Repositories;
using CSharpFunctionalExtensions;

namespace UsersStore.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<List<Users>> GetAllUsers()
        {
            return await _usersRepository.Get();
        }

        public async Task<List<Users>> GetActiveUsers()
        {
            return await _usersRepository.GetActive();
        }

        public async Task<Result<Users>> GetUserByLogin(string login)
        {
            var user = await _usersRepository.GetUser(login);
            if(user == null)
            {
                return Result.Failure<Users>("Пользователь не найден"); ;
            }

            return Result.Success(user);
        }

        public async Task<Result<Users>> GetUserProfile(string login, string password)
        {
            var user = await _usersRepository.GetProfile(login, password);
            if(user == null)
            {
                return Result.Failure<Users>("Ошибка введенных данных или пользователя не существует");
            }

            return Result.Success(user);
        }

        public async Task<List<Users>> GetUsersWithAge(int age)
        {
            return await _usersRepository.GetAged(age);
        }

        public async Task<Guid> CreateUser(Users user)
        {
            return await _usersRepository.Create(user);
        }

        public async Task<Guid> UpdateUserInfo(Guid id, string? name, int? gender, DateTime? birthday, string? login, string? password, string? modifiedBy)
        {
            
            return await _usersRepository.UpdateInfo(id, name, gender, birthday, login, password, modifiedBy);
        }

        public async Task<Guid> DeleteUser(Guid id)
        {
            return await _usersRepository.Delete(id);
        }

        public async Task<Guid> SoftDeleteUser(Guid id, string revokedBy)
        {
            return await _usersRepository.LightDelete(id, revokedBy);
        }

        public async Task<Guid> RestoreUser(Guid id)
        {
            return await _usersRepository.Restore(id);
        }


    }
}
