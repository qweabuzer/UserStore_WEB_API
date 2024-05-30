using CSharpFunctionalExtensions;
using UsersStore.Core.Models;
using UsersStore.Core.Interfaces;
using UsersStore.DataAccess.Repositories;

namespace UsersStore.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsersRepository _usersRepository;

        public AuthService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<Result<Users>> Authenticate(string login, string password)
        {
            var user = await _usersRepository.GetProfile(login, password);

            if (user == null)
            {
                return Result.Failure<Users> ("Неправильный логин или пароль");
            }

            return Result.Success(user);
        }

        public bool isAdmin(Users user)
        {
            return user.Admin == true;
        }
    }
}
