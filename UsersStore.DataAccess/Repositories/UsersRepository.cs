using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UsersStore.Core.Models;
using UsersStore.DataAccess.Entities;

namespace UsersStore.DataAccess.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UsersStoreDbContext _context;
        private readonly ILogger<UsersRepository> _logger;

        public UsersRepository(UsersStoreDbContext context, ILogger<UsersRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Users>> Get()
        {
            var userEntities = await _context.Users
                .AsNoTracking()
                .ToListAsync();

            var users = userEntities
                .Select(u => Users.TransferCreate(u.Id, u.Login, u.Password, u.Name, u.Gender, u.Birthday, u.Admin, u.CreatedOn, u.CreatedBy, u.ModifiedOn, u.ModifiedBy, u.RevokedOn, u.RevokedBy))
                .ToList();

            return users;
        }
        public async Task<Users> GetUser(string login)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login);

            if (userEntity == null)
            {
                return null;
            }

            var user = Users.TransferCreate(userEntity.Id, userEntity.Login, userEntity.Password, userEntity.Name, userEntity.Gender, userEntity.Birthday, userEntity.Admin, userEntity.CreatedOn, userEntity.CreatedBy, userEntity.ModifiedOn, userEntity.ModifiedBy, userEntity.RevokedOn, userEntity.RevokedBy);

            return user;
        }

        public async Task<List<Users>> GetActive()
        {
            var userEntities = await _context.Users
                .AsNoTracking()
                .Where(u => u.RevokedBy == string.Empty)
                .Where(u => u.RevokedOn == DateTime.MinValue)
                .OrderBy(u => u.CreatedOn)
                .ToListAsync();

            var users = userEntities
                .Select(u => Users.TransferCreate(u.Id, u.Login, u.Password, u.Name, u.Gender, u.Birthday, u.Admin, u.CreatedOn, u.CreatedBy, u.ModifiedOn, u.ModifiedBy, u.RevokedOn, u.RevokedBy))
                .ToList();

            return users;
        }

        public async Task<Users> GetProfile(string login, string password)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == login && u.Password == password);

            if (userEntity == null)
            {
                return null;
            }

            var user = Users.TransferCreate(userEntity.Id, userEntity.Login, userEntity.Password, userEntity.Name, userEntity.Gender, userEntity.Birthday, userEntity.Admin, userEntity.CreatedOn, userEntity.CreatedBy, userEntity.ModifiedOn, userEntity.ModifiedBy, userEntity.RevokedOn, userEntity.RevokedBy);

            return user;
        }

        public async Task<List<Users>> GetAged(int age)
        {
            var yearsDiff = DateTime.Now.AddYears(-age);

            var userEntities = await _context.Users
                .AsNoTracking()
                .Where(u => u.Birthday <= yearsDiff)
                .ToListAsync();

            var users = userEntities
                .Select(u => Users.TransferCreate(u.Id, u.Login, u.Password, u.Name, u.Gender, u.Birthday, u.Admin, u.CreatedOn, u.CreatedBy, u.ModifiedOn, u.ModifiedBy, u.RevokedOn, u.RevokedBy))
                .ToList();

            return users;
        }

        public async Task<Guid> Create(Users user)
        {
            var userEntity = new UserEntity
            {
                Id = user.Id,
                Login = user.Login,
                Password = user.Password,
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                Admin = user.Admin,
                CreatedOn = user.CreatedOn,
                CreatedBy = user.CreatedBy,
            };

            await _context.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return userEntity.Id;
        }

        public async Task<Guid> UpdateInfo(Guid id, string? name, int? gender, DateTime? birthday, string? login, string? password, string? modifiedBy)
        {
            /*await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Name, u => name)
                .SetProperty(u => u.Gender, u => gender)
                .SetProperty(u => u.Birthday, u => birthday));*/

            if (!string.IsNullOrEmpty(name))
                await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Name, u => name));

            if(gender != null)
                await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Gender, u => gender));

            if (birthday != null)
                await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Birthday, u => birthday));

            if (!string.IsNullOrEmpty(login))
                await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Login, u => login));

            if (!string.IsNullOrEmpty(password))
                await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Password, u => password));

            //if (modifiedOn != null)
                await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.ModifiedOn, u => DateTime.Now));

            if (!string.IsNullOrEmpty(modifiedBy))
                await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.ModifiedBy, u => modifiedBy));

            return id;
        }

/*        public async Task<Guid> UpdatePassword(Guid id, string password)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Password, u => password));

            return id;
        }

        public async Task<Guid> UpdateLogin(Guid id, string login)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Login, u => login));

            return id;
        }*/
        
        public async Task<Guid> LightDelete(Guid id, string revokedBy)
        {
            if (!string.IsNullOrEmpty(revokedBy))
            {
                await _context.Users
                    .Where(u => u.Id == id)
                    .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.RevokedOn, u => DateTime.Now)
                    .SetProperty(u => u.RevokedBy, u => revokedBy));
            }

            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }

        public async Task<Guid> Restore(Guid id)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.RevokedOn, u => DateTime.MinValue)
                .SetProperty(u => u.RevokedBy, u => string.Empty));

            return id;
        }

    }
}
