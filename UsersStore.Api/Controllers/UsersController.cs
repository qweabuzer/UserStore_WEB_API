using Microsoft.AspNetCore.Mvc;
using UsersStore.Api.Contracts;
using UsersStore.Application.Services;
using UsersStore.Core.Models;

namespace UsersStore.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet("All")]
        public async Task<ActionResult<List<UsersResponse>>> GetUsers()
        {
            var users = await _usersService.GetAllUsers();

            var response = users
                .Select(u => new UsersResponse(u.Id, u.Login, u.Password, u.Name, u.Gender, u.Birthday, u.Admin, u.CreatedOn, u.CreatedBy, u.ModifiedOn, u.ModifiedBy, u.RevokedOn, u.RevokedBy));

            return Ok(response);
        }

        [HttpGet("AllActive")]
        public async Task<ActionResult<List<UsersResponse>>> GetAllActiveUsers()
        {
            var users = await _usersService.GetActiveUsers();

            var response = users
                .Select(u => new UsersResponse(u.Id, u.Login, u.Password, u.Name, u.Gender, u.Birthday, u.Admin, u.CreatedOn, u.CreatedBy, u.ModifiedOn, u.ModifiedBy, u.RevokedOn, u.RevokedBy));
                
            return Ok(response);
        }

        [HttpGet("ByLogin")]
        public async Task<ActionResult<UserGetResponse>> GetUser(string login)
        {
            var user = await _usersService.GetUserByLogin(login);

            if (user.IsFailure)
                return BadRequest(user.Error);
            //
            var response = new UserGetResponse(user.Value.Name, user.Value.Gender, user.Value.Birthday, user.Value.RevokedOn);
            return Ok(response);
        }

        [HttpGet("Profile")]
        public async Task<ActionResult<UsersResponse>> GetUsersProfile(string login, string password)
        {
            var user = await _usersService.GetUserProfile(login, password);

            if (user.IsFailure)
                return BadRequest(user.Error);

            var response = new UsersResponse(user.Value.Id, user.Value.Login, user.Value.Password, user.Value.Name, user.Value.Gender, user.Value.Birthday, user.Value.Admin, user.Value.CreatedOn, user.Value.CreatedBy, user.Value.ModifiedOn, user.Value.ModifiedBy, user.Value.RevokedOn, user.Value.RevokedBy);
            return Ok(response);
        }

        [HttpGet("Aged")]
        public async Task<ActionResult<List<UsersResponse>>> GetAgedUsers(int age)
        {
            var users = await _usersService.GetUsersWithAge(age);

            var response = users
                .Select(u => new UsersResponse(u.Id, u.Login, u.Password, u.Name, u.Gender, u.Birthday, u.Admin, u.CreatedOn, u.CreatedBy, u.ModifiedOn, u.ModifiedBy, u.RevokedOn, u.RevokedBy));

            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateUser([FromBody] UsersRequest request)
        {

            var user = Users.Create(
                Guid.NewGuid(),
                request.login,
                request.password,
                request.name,
                request.gender,
                request.birthday,
                request.admin,
                DateTime.Now,
                request.createdBy);

            if (user.IsFailure)
            {
                return BadRequest(user.Error);
            }

            var userId = await _usersService.CreateUser(user.Value);

            return Ok(userId);
        }

        [HttpPut]
        public async Task<ActionResult<Guid>> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {

            var userId = await _usersService.UpdateUserInfo(id, request.name, request.gender, request.birthday, request.login, request.password, request.ModifiedBy);

            return Ok(userId);
        }

        [HttpDelete]
        public async Task<ActionResult<Guid>> DeleteUser(Guid id)
        {
            return Ok(await _usersService.DeleteUser(id));
        }

        [HttpPatch("Revoke")]
        public async Task<ActionResult<Guid>> RevokeUser(Guid id, string revokedBy)
        {
            return Ok(await _usersService.SoftDeleteUser(id, revokedBy));
        }

        [HttpPatch("Unban")]
        public async Task<ActionResult<Guid>> UnbanUser(Guid id)
        {
            return Ok(await _usersService.RestoreUser(id));
        }
    }
}
