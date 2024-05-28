using System.ComponentModel;

namespace UsersStore.Api.Contracts
{
    public class UpdateUserRequest
    {
        [DefaultValue("")]
        public string? login { get; set; } 

        [DefaultValue("")]
        public string? password { get; set; } 

        [DefaultValue("")]
        public string? name { get; set; } 

        [DefaultValue(null)]
        public int? gender { get; set; }

        [DefaultValue(null)]
        public DateTime? birthday { get; set; } = null;

        [DefaultValue("")]
        public string? ModifiedBy { get; set; } 
    }
}
