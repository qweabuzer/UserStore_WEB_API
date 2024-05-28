
namespace UsersStore.DataAccess.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Gender { get; set; } = 2;
        public DateTime? Birthday { get; set; }
        public bool Admin { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = String.Empty;
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; } = String.Empty;
        public DateTime RevokedOn { get; set; }
        public string RevokedBy { get; set; } = String.Empty;
    }
}
