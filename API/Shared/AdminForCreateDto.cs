using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.Shared
{
    public class AdminForCreateDto
    {
        [JsonPropertyName("email")]
       public required string Email { get;set; }

       [JsonPropertyName("password")]
       public required string Password { get;set; }

       [JsonPropertyName("username")]
       public required string UserName { get;set; }
    }
}