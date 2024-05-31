using UsersStore.Core.Interfaces;
using UsersStore.DataAccess.Repositories;
using UsersStore.Core.Models;

namespace UsersStore.Application.Services
{

    public class StartUpService : IStartUpService
    {
        private readonly IUsersRepository _usersRepository;

        public StartUpService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task InitializeAdminUser()
        {
            if (!_usersRepository.AdminExists())
            {
                var asdminUser = Users.Create(
                    Guid.NewGuid(),
                    "admin",
                    "admin",
                    "admin",
                    1,
                    null,
                    true,
                    DateTime.Now,
                    "admin");

                await _usersRepository.Create(asdminUser.Value);
            }
        }
    }
}
