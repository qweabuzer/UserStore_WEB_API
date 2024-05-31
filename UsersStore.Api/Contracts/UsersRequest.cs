using System.ComponentModel;

namespace UsersStore.Api.Contracts
{
    public class UsersRequest
    {

        [DefaultValue("")]
        public string login { get; set; } = string.Empty;

        [DefaultValue("")]
        public string password { get; set; } = string.Empty;

        [DefaultValue("")]
        public string name { get; set; } = string.Empty;

        [DefaultValue(2)]
        public int gender { get; set; } = 2;

        [DefaultValue(null)]
        public DateTime? birthday { get; set; } = null;

        [DefaultValue(false)]
        public bool admin { get; set; } = false;
    }
}

