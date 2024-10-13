using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace API.Shared
{
    public class AdminDto
    {
        [JsonPropertyName("id")]        
       public required int Id { get;set; }

       [JsonPropertyName("email")]
       public required string Email { get;set; }

       [JsonPropertyName("password")]
       public required string Password { get;set; }

       [JsonPropertyName("username")]
       public required string UserName { get;set; }
    }
}