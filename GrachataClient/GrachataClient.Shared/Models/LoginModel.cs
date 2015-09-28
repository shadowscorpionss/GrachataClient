using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace GrachataClient.Models
{
    [DataContract]
    public class LoginModel
    {
        public const string GrantTypeString = "grant_type", UserNameString = "username", PasswordString = "password";

        [DataMember(Name = GrantTypeString)]
        public string GrantType { get; set; } = "password";

        [DataMember(Name = UserNameString)]
        public string UserName { get; set; }

        [DataMember(Name = PasswordString)]
        public string Password { get; set; }
    }
}
