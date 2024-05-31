using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using UsersStore.Api.Contracts;
using UsersStore.Api.Helpers;
using UsersStore.Application.Services;
using UsersStore.Core.Interfaces;
using UsersStore.Core.Models;

namespace UsersStore.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IAuthService _authService;
        
        public UsersController(IUsersService usersService, IAuthService authService)
        {
            _usersService = usersService;
            _authService = authService;
        }

        [HttpGet("AllActive")]
        public async Task<ActionResult<List<UsersResponse>>> GetAllActiveUsers(string login, string password)
        {
            var authResult = await _authService.Authenticate(login, password);

            if (authResult.IsFailure)
            {
                return Unauthorized(authResult.Error);
            }

            var authUser = authResult.Value;
            if (!_authService.isAdmin(authUser))
                return new ForbiddenResult("Это действие доступно только администратору.");

            var users = await _usersService.GetActiveUsers();

            var response = users
                .Select(u => new UsersResponse(
                    u.Id, 
                    u.Login, 
                    u.Password, 
                    u.Name, 
                    u.Gender, 
                    u.Birthday, 
                    u.Admin, 
                    u.CreatedOn, 
                    u.CreatedBy, 
                    u.ModifiedOn, 
                    u.ModifiedBy, 
                    u.RevokedOn, 
                    u.RevokedBy));
                
            return Ok(response);
        }

        [HttpGet("ByLogin")]
        public async Task<ActionResult<UserGetResponse>> GetUser(string login, string password, string userLogin)
        {
            var authResult = await _authService.Authenticate(login, password);

            if (authResult.IsFailure)
            {
                return Unauthorized(authResult.Error);
            }

            var authUser = authResult.Value;
            if (!_authService.isAdmin(authUser))
                return new ForbiddenResult("Это действие доступно только администратору.");

            var user = await _usersService.GetUserByLogin(userLogin);

            if (user.IsFailure)
                return BadRequest(user.Error);

            var response = new UserGetResponse(
                user.Value.Name, 
                user.Value.Gender, 
                user.Value.Birthday, 
                user.Value.RevokedOn, 
                user.Value.RevokedBy);

            return Ok(response);
        }

        [HttpGet("Profile")]
        public async Task<ActionResult<UsersResponse>> GetUsersProfile(string login, string password,string userLogin, string userPassword)
        {
            var authResult = await _authService.Authenticate(login, password);
            if (authResult.IsFailure)
                return Unauthorized(authResult.Error);

            else if (authResult.Value.RevokedBy != null && authResult.Value.RevokedOn != DateTime.MinValue)
                return new RevokedResult("Невозможно выполнить действие. Пользователь отстранен администратором");

            else if(login != userLogin && !_authService.isAdmin(authResult.Value))
                return new ForbiddenResult("Это действие доступно только администратору.");

            var user = await _usersService.GetUserProfile(userLogin, userPassword);
            
            if (user.IsFailure)
                return BadRequest(user.Error);

            var response = new UsersResponse(
                user.Value.Id, 
                user.Value.Login, 
                user.Value.Password, 
                user.Value.Name, 
                user.Value.Gender, 
                user.Value.Birthday, 
                user.Value.Admin, 
                user.Value.CreatedOn, 
                user.Value.CreatedBy, 
                user.Value.ModifiedOn, 
                user.Value.ModifiedBy, 
                user.Value.RevokedOn, 
                user.Value.RevokedBy);

            return Ok(response);
        }

        [HttpGet("ByAge")]
        public async Task<ActionResult<List<UsersResponse>>> GetAgedUsers(string login, string password, int age)
        {
            var authResult = await _authService.Authenticate(login, password);

            if (authResult.IsFailure)
            {
                return Unauthorized(authResult.Error);
            }

            var authUser = authResult.Value;
            if (!_authService.isAdmin(authUser))
                return new ForbiddenResult("Это действие доступно только администратору.");

            var users = await _usersService.GetUsersWithAge(age);

            var response = users
                .Select(u => new UsersResponse(
                    u.Id, 
                    u.Login, 
                    u.Password, 
                    u.Name, 
                    u.Gender, 
                    u.Birthday, 
                    u.Admin, 
                    u.CreatedOn, 
                    u.CreatedBy, 
                    u.ModifiedOn, 
                    u.ModifiedBy, 
                    u.RevokedOn, 
                    u.RevokedBy));

            return Ok(users);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> CreateUser(string login, string password, [FromBody] UsersRequest request)
        {
            var authResult = await _authService.Authenticate(login, password);

            if (authResult.IsFailure)
            {
                return Unauthorized(authResult.Error);
            }

            var authUser = authResult.Value;
            if (!_authService.isAdmin(authUser))
                return new ForbiddenResult("Это действие доступно только администратору.");


            var user = Users.Create(
                Guid.NewGuid(),
                request.login,
                request.password,
                request.name,
                request.gender,
                request.birthday,
                request.admin,
                DateTime.Now,
                authResult.Value.Name);

            if (user.IsFailure)
            {
                return BadRequest(user.Error);
            }

            var userId = await _usersService.CreateUser(user.Value);
            
            if (userId.IsFailure)
            {
                return BadRequest(userId.Error);
            }

            return Ok(userId.Value);
        }

        [HttpPut("UpdateInfo")]
        public async Task<ActionResult<Guid>> UpdateUser(string login, string password, Guid id, [FromBody] UpdateUserRequest request)
        {
            var user = await _authService.Authenticate(login, password);
            if (user.IsFailure)
                return Unauthorized(user.Error);

            else if (user.Value.RevokedBy != null && user.Value.RevokedOn != DateTime.MinValue)
                return new RevokedResult("Невозможно выполнить действие. Пользователь отстранен администратором");

            var modify = user.Value.Name;

            if (!_authService.isAdmin(user.Value))
                modify = request.name;


            var userId = await _usersService.UpdateUserInfo(
                id, 
                request.name, 
                request.gender, 
                request.birthday, 
                request.login, 
                request.password, 
                modify);

            if (userId.IsFailure)
                return BadRequest(userId.Error);

            return Ok(userId.Value);
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<Guid>> DeleteUser(string login, string password, Guid id)
        {
            var authResult = await _authService.Authenticate(login, password);

            if (authResult.IsFailure)
            {
                return Unauthorized(authResult.Error);
            }

            var authUser = authResult.Value;
            if (!_authService.isAdmin(authUser))
                return new ForbiddenResult("Это действие доступно только администратору.");

            return Ok(await _usersService.DeleteUser(id));
        }

        [HttpPatch("Revoke")]
        public async Task<ActionResult<Guid>> RevokeUser(string login, string password, Guid id)
        {
            var authResult = await _authService.Authenticate(login, password);

            if (authResult.IsFailure)
            {
                return Unauthorized(authResult.Error);
            }

            var authUser = authResult.Value;
            if (!_authService.isAdmin(authUser))
                return new ForbiddenResult("Это действие доступно только администратору.");

            return Ok(await _usersService.SoftDeleteUser(id, authResult.Value.Name));
        }

        [HttpPatch("Unban")]
        public async Task<ActionResult<Guid>> UnbanUser(string login, string password, Guid id)
        {
            var authResult = await _authService.Authenticate(login, password);

            if (authResult.IsFailure)
            {
                return Unauthorized(authResult.Error);
            }

            var authUser = authResult.Value;
            if (!_authService.isAdmin(authUser))
                return new ForbiddenResult("Это действие доступно только администратору.");

            return Ok(await _usersService.RestoreUser(id));
        }
    }
}
