using System;

namespace Entities.DataTransferObjects
{
    public class AccountForDetailsDto
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string? AccountType { get; set; }
        public OwnerForDetailsDto Owner { get; set; }
    }
}
