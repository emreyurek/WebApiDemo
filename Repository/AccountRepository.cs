using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Account> AccountsByOwner(Guid ownerId)
        {
            return FindByCondition(a => a.OwnerId.Equals(ownerId)).ToList();
        }

        public void CreateAccount(Account account)
        {
            Create(account);
        }

        public void DeleteAccount(Account account)
        {
            Delete(account);
        }

        public Account GetAccountById(Guid accountId)
        {
            return FindByCondition(a => a.Id.Equals(accountId))
                   .FirstOrDefault();
        }

        public Account GetAccountWithDetails(Guid accountId)
        {
            return FindByCondition(a => a.Id.Equals(accountId))
                    .Include(o => o.Owner)
                    .FirstOrDefault();
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return FindAll();
        }

        public void UpdateAccount(Account account)
        {
            Update(account);
        }
    }
}
