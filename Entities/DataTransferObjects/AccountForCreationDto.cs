
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class AccountForCreationDto
    {
        [Required(ErrorMessage = "Date created is required")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "Account type is required")]
        public string? AccountType { get; set; }

        [Required(ErrorMessage = "OwnerId is required")]
        public Guid? OwnerId { get; set; }
    }
}
