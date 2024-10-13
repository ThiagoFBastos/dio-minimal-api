using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domain.Entities
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get;set; }

        [Required]
        [StringLength(255)]
        public required string Email { get;set; }

        [Required]
        [StringLength(50)]
        public required string Password { get;set; }

        [Required]
        [StringLength(10)]
        public required string UserName { get;set; }
    }
}