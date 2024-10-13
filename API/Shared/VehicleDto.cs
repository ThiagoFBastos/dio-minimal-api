using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.Shared
{
    public class VehicleDto
    {
        [JsonPropertyName("id")]
        public required int Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("make")]
        public required string Make { get;set; }

        [JsonPropertyName("year")]
        public required int Year { get;set; }
    }
}