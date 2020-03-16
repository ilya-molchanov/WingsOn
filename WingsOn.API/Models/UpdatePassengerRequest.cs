using System;
using System.ComponentModel.DataAnnotations;

namespace Enprecis.Users.Api.Models
{
    public class UpdatePassengerRequest
    { 
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int PersonId { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(400)]
        public string Address { get; set; }
    }
}
