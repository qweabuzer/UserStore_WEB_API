using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace UsersStore.Core.Models
{
    public class Users
    {
        private Users(Guid id, string login, string password, string name, int gender, DateTime? birthday, bool admin, DateTime createdOn ,string createdBy)
        {
            Id = id;
            Login = login;
            Password = password;
            Name = name;
            Gender = gender;
            Birthday = birthday;
            Admin = admin;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
        }
        
        private Users(Guid id, string login, string password, string name, int gender, DateTime? birthday, bool admin, DateTime createdOn, string createdBy, DateTime modifiedOn, string modifiedBy, DateTime revokedOn, string revokedBy)
        {
            Id = id;
            Login = login;
            Password = password;
            Name = name;
            Gender = gender;
            Birthday = birthday;
            Admin = admin;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            ModifiedOn = modifiedOn;
            ModifiedBy = modifiedBy;
            RevokedOn = revokedOn;
            RevokedBy = revokedBy;
        }

        public Guid Id { get; }
        public string Login {  get; }
        public string Password { get; }
        public string Name { get; }
        public int Gender { get; } = 2;
        public DateTime? Birthday { get; }
        public bool Admin { get; }
        public DateTime CreatedOn { get; }
        public string CreatedBy { get; }
        public DateTime ModifiedOn { get; } = DateTime.MinValue;
        public string ModifiedBy { get; } = String.Empty;
        public DateTime RevokedOn { get; } = DateTime.MinValue;
        public string RevokedBy { get; } = String.Empty;

        public static Result<Users> Create(Guid id, string login, string password, string name, int gender, DateTime? birthday, bool admin,DateTime createdOn, string createdBy)
        {
            if (!Regex.IsMatch(login, "^[a-zA-Z0-9]+$"))
                return Result.Failure<Users>("Для поля Login запрещены все символы кроме латинских букв и цифр");

            if (!Regex.IsMatch(password, "^[a-zA-Z0-9]+$"))
                return Result.Failure<Users>("Для поля Password запрещены все символы кроме латинских букв и цифр");

            if (!Regex.IsMatch(name, "^[a-zA-Zа-яА-Я]+$"))
                return Result.Failure<Users>("Для поля Name запрещены все символы кроме латинских и русских букв");

            if (gender < 0 || gender > 2)
                return Result.Failure<Users>("Для поля Gender доступны значения: 0 - женщина, 1 - мужчина, 2 - неизвестно");

            if (createdBy == string.Empty)
                return Result.Failure<Users>("Укажите логин пользователя, от имени которого этот пользователь создается");

            var user = new Users(
                id, 
                login, 
                password, 
                name, 
                gender, 
                birthday, 
                admin,createdOn, 
                createdBy);

            return Result.Success<Users>(user);
        }

        public static Users TransferCreate(Guid id, string login, string password, string name, int gender, DateTime? birthday, bool admin, DateTime createdOn, string createdBy, DateTime modifiedOn, string modifiedBy, DateTime revokedOn, string revokedBy)
        {
            return new Users(
                id, 
                login, 
                password, 
                name, gender,
                birthday, 
                admin, 
                createdOn, 
                createdBy, 
                modifiedOn, 
                modifiedBy, 
                revokedOn, 
                revokedBy);
        }
    }
}
