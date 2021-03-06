﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GrachataClient.Models
{
    [DataContract]
    public class TokenModel
    {
        [DataMember(Name =".expires")]
        public string Expires { get; set; }
        [DataMember(Name = ".issued")]
        public string Issued { get; set; }
        [DataMember(Name = "expires_in")]
        public long ExpiresIn { get; set; }
        [DataMember (Name = "token_type")]
        public string TokenType { get; set; }
        [DataMember (Name = "access_token")]
        public string AccessToken { get; set; }
        [DataMember (Name = "userName")]
        public string UserName { get; set; }

    }
}
