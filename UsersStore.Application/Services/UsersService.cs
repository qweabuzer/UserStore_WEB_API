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

        public async Task<Result<Guid>> CreateUser(Users user)
        {
            var userId= await _usersRepository.Create(user);
            if (userId == Guid.Empty)
                return Result.Failure<Guid>("Пользователь с таким логином уже существует.");

            return Result.Success(userId);
        }

        public async Task<Result<Guid>> UpdateUserInfo(Guid id, string? name, int? gender, DateTime? birthday, string? login, string? password, string? modifiedBy)
        {
            var result = await _usersRepository.UpdateInfo(id, name, gender, birthday, login, password, modifiedBy);

            if (result == Guid.Empty)
                return Result.Failure<Guid>("Этот логин уже занят");

            return Result.Success(result);
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
