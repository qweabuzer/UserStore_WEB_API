using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel;
using System.Reflection;
using UsersStore.Core.Models;

namespace UsersStore.Api.Contracts
{
    /*    public record UsersRequest(
            string login,
            string password,
            string name,
            int gender,
            DateTime birthday,
            bool admin,
            string createdBy);*/
    public class UsersRequest
    {

        [DefaultValue("")]
        public string login { get; set; } = string.Empty;

        [DefaultValue("")]
        public string password { get; set; } = string.Empty;

        [DefaultValue("")]
        public string name { get; set; } = string.Empty;

        [DefaultValue(-1)]
        public int gender { get; set; } = -1;

        [DefaultValue(null)]
        public DateTime? birthday { get; set; } = null;

        [DefaultValue(false)]
        public bool admin { get; set; } = false;

        [DefaultValue("")]
        public string createdBy { get; set; } = string.Empty;
    }
}

