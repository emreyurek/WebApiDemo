using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class AccountForUpdateDto
    {
        [Required(ErrorMessage = "Data created is required")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "Account type is required")]
        public string? AccountType { get; set; }
    }
}
