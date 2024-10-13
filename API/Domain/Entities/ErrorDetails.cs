using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Domain.Entities
{
    public class ErrorDetails
    {
        public required string Title { get; set; }
        public required int Status { get; set; }
        public required string TraceId { get; set; }
        public string? Type { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}