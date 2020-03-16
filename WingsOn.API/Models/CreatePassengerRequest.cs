using System;
using System.ComponentModel.DataAnnotations;
using WingsOn.Domain;

namespace WingsOn.API.Web.Models
{
    public class CreatePassengerRequest
    {
        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime DateBirth { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(400)]
        public string Address { get; set; }

        [Required]
        [Range(0, 1, ErrorMessage = "Please enter valid gender")]
        public GenderType Gender { get; set; }

        [Required]
        [MinLength(1)]
        public string FlightNumber { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int CustomerId { get; set; } 
    }
}
