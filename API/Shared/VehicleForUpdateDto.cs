using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Shared
{
    public class VehicleForUpdateDto
    {
        public required string Name { get; set; }
        public required string Make { get;set; }
        public required int Year { get;set; }
    }
}